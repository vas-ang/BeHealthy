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

    using static BeHealthy.Common.GlobalConstants;
    using static BeHealthy.Common.Messages;

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
                this.TempData[ErrorMessageKey] = InvalidExercise;
                return this.RedirectToAction("Browse", "Exercises");
            }

            if (await this.exerciseStepService.GetExerciseStepsCountAsync(exerciseId) > MaxExerciseStepsCount)
            {
                this.TempData[ErrorMessageKey] = string.Format(ExerciseStepMaxLimit, MaxExerciseStepsCount);
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
                this.TempData[ErrorMessageKey] = InvalidExercise;
                return this.RedirectToAction("Browse", "Exercises");
            }

            if (await this.exerciseStepService.GetExerciseStepsCountAsync(inputModel.ExerciseId) > MaxExerciseStepsCount)
            {
                this.TempData[ErrorMessageKey] = string.Format(ExerciseStepMaxLimit, MaxExerciseStepsCount);
                return this.RedirectToAction("Details", "Exercises", new { inputModel.ExerciseId });
            }

            var imageUrl = await this.cloudinaryService.UploadImageAsync($"{Guid.NewGuid()}_{user.UserName}", user.UserName, inputModel.ImageUpload);

            await this.exerciseStepService.CreateExerciseStepAsync(inputModel, imageUrl);

            this.TempData[InfoMessageKey] = CreatedSuccessfully;

            return this.RedirectToAction("Details", "Exercises", new { inputModel.ExerciseId });
        }

        public async Task<IActionResult> Edit(int exerciseStepId)
        {
            var exerciseId = await this.exerciseService.GetExerciseIdByStepIdAsync(exerciseStepId);

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, userId) && !this.User.IsInRole(AdministratorRoleName))
            {
                this.TempData[ErrorMessageKey] = InvalidExerciseStep;
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

            var exerciseId = await this.exerciseService.GetExerciseIdByStepIdAsync(exerciseStepEditInputModel.Id);

            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, user.Id) && !this.User.IsInRole(AdministratorRoleName))
            {
                this.TempData[ErrorMessageKey] = InvalidExerciseStep;
                return this.RedirectToAction("Browse", "Exercises");
            }

            string newImageUrl = string.Empty;

            if (exerciseStepEditInputModel.ImageUpload != null)
            {
                newImageUrl = await this.cloudinaryService.UploadImageAsync($"{Guid.NewGuid()}_{user.UserName}", user.UserName, exerciseStepEditInputModel.ImageUpload);
            }

            await this.exerciseStepService.UpdateExerciseStepAsync(exerciseStepEditInputModel, newImageUrl);

            this.TempData[InfoMessageKey] = ChangesSavedSuccessfully;

            return this.RedirectToAction("Details", "Exercises", new { exerciseId });
        }

        public async Task<IActionResult> Delete(int exerciseStepId)
        {
            var exerciseId = await this.exerciseService.GetExerciseIdByStepIdAsync(exerciseStepId);

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, userId) && !this.User.IsInRole(AdministratorRoleName))
            {
                this.TempData[ErrorMessageKey] = InvalidExerciseStep;
                return this.RedirectToAction("Browse", "Exercises");
            }

            await this.exerciseStepService.DeleteExerciseStepAsync(exerciseStepId);

            this.TempData[InfoMessageKey] = DeletedSuccessfully;

            return this.RedirectToAction("Details", "Exercises", new { exerciseId });
        }
    }
}
