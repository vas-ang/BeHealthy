namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Cloudinary;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.ExerciseSteps;
    using BeHealthy.Web.Dtos.Fitness.ExerciseSteps.InputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Area("Fitness")]
    public class ExerciseStepsController : Controller
    {
        private readonly IExerciseStepService exerciseStepService;
        private readonly IExerciseService exerciseService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExerciseStepsController(
            IExerciseStepService exerciseStepService,
            IExerciseService exerciseService,
            ICloudinaryService cloudinaryService,
            UserManager<ApplicationUser> userManager)
        {
            this.exerciseStepService = exerciseStepService;
            this.exerciseService = exerciseService;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(string exerciseId)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, userId))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            if (this.exerciseStepService.GetExerciseStepsCount(exerciseId) > 10)
            {
                this.TempData["ErrorMessage"] = $"Exercise cannot have more than {10} steps.";
                return this.RedirectToAction("Details", "Exercises", new { exerciseId });
            }

            var viewModel = new ExerciseStepCreateInputModel
            {
                ExerciseId = exerciseId,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseStepCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, user.Id))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            if (this.exerciseStepService.GetExerciseStepsCount(inputModel.ExerciseId) > 10)
            {
                this.TempData["ErrorMessage"] = $"Exercise cannot have more than {10} steps.";
                return this.RedirectToAction("Details", "Exercises", new { inputModel.ExerciseId });
            }

            var imageUrl = await this.cloudinaryService.UploadImageAsync($"{Guid.NewGuid()}_{user.UserName}", user.UserName, inputModel.ImageUpload);

            await this.exerciseStepService.CreateExerciseStepAsync(inputModel, imageUrl);

            return this.RedirectToAction("Details", "Exercises", new { inputModel.ExerciseId });
        }

        public async Task<IActionResult> Edit(int exerciseStepId)
        {
            if (!await this.exerciseStepService.ExerciseStepExistsAsync(exerciseStepId))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise step.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            var exerciseId = await this.exerciseService.GetExerciseIdByStepIdAsync(exerciseStepId);

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, userId))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            var viewModel = await this.exerciseStepService.GetExerciseStepAsync<ExerciseStepEditInputModel>(exerciseStepId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExerciseStepEditInputModel exerciseStepEditInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(exerciseStepEditInputModel);
            }

            if (!await this.exerciseStepService.ExerciseStepExistsAsync(exerciseStepEditInputModel.Id))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise step.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            var exerciseId = await this.exerciseService.GetExerciseIdByStepIdAsync(exerciseStepEditInputModel.Id);

            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, user.Id))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            string newImageUrl = string.Empty;

            if (exerciseStepEditInputModel.ImageUpload != null)
            {
                newImageUrl = await this.cloudinaryService.UploadImageAsync($"{Guid.NewGuid()}_{user.UserName}", user.UserName, exerciseStepEditInputModel.ImageUpload);
            }

            await this.exerciseStepService.UpdateExerciseStepAsync(exerciseStepEditInputModel, newImageUrl);

            return this.RedirectToAction("Details", "Exercises", new { exerciseId });
        }

        public async Task<IActionResult> Delete(int exerciseStepId)
        {
            if (!await this.exerciseStepService.ExerciseStepExistsAsync(exerciseStepId))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise step.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            var exerciseId = await this.exerciseService.GetExerciseIdByStepIdAsync(exerciseStepId);

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, userId))
            {
                this.TempData["ErrorMessage"] = "Invalid exercise.";
                return this.RedirectToAction("Browse", "Exercises");
            }

            await this.exerciseStepService.DeleteExerciseStepAsync(exerciseStepId);

            return this.RedirectToAction("Details", "Exercises", new { exerciseId });
        }
    }
}
