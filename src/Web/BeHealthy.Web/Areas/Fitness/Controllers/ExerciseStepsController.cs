namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Cloudinary;
    using BeHealthy.Services.Data;
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
                return this.RedirectToAction("Browse", "Exercises");
            }

            if (this.exerciseStepService.GetExerciseStepsCount(exerciseId) >= 10)
            {
                return this.RedirectToAction("Edit", "Exercises", new { exerciseId });
            }

            this.TempData["ExerciseId"] = exerciseId;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseStepInputModel exerciseStepInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseStepInputModel.ExerciseId, user.Id))
            {
                return this.RedirectToAction("Browse", "Exercises");
            }

            if (this.exerciseStepService.GetExerciseStepsCount(exerciseStepInputModel.ExerciseId) >= 10)
            {
                return this.RedirectToAction("Edit", "Exercises", new { exerciseStepInputModel.ExerciseId });
            }

            var imageUrl = await this.cloudinaryService.UploadImageAsync($"{Guid.NewGuid()}_{user.UserName}", user.UserName, exerciseStepInputModel.ImageUpload);

            await this.exerciseStepService.CreateExerciseStepAsync(exerciseStepInputModel, imageUrl);

            return this.RedirectToAction("Edit", "Exercises", new { exerciseStepInputModel.ExerciseId });
        }

        public IActionResult Edit(string exerciseStepid)
        {
            return this.View();
        }

        public IActionResult Delete(string exerciseStepId)
        {
            return this.RedirectToAction("/");
        }
    }
}
