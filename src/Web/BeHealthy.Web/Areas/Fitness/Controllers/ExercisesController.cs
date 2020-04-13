namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data;
    using BeHealthy.Web.Controllers;
    using BeHealthy.Web.Dtos.Fitness.Exercises.InputModels;
    using BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Area("Fitness")]
    public class ExercisesController : BaseController
    {
        private readonly IExerciseService exerciseService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExercisesController(IExerciseService exerciseService, UserManager<ApplicationUser> userManager)
        {
            this.exerciseService = exerciseService;
            this.userManager = userManager;
        }

        public IActionResult Browse()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseInputModel exerciseInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Create();
            }

            string creatorId = this.userManager.GetUserId(this.User);

            string exerciseId = await this.exerciseService.CreateAsync(exerciseInputModel, creatorId);

            return this.RedirectToAction(nameof(this.Edit), new { exerciseId });
        }

        public IActionResult Edit(string exerciseId)
        {
            var exerciseEditViewModel = this.exerciseService.GetExercise<ExerciseEditViewModel>(exerciseId);

            return this.View(exerciseEditViewModel);
        }

        public IActionResult Details(string exerciseId)
        {
            return this.View();
        }
    }
}
