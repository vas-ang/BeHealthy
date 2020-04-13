namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data;
    using BeHealthy.Web.Dtos.Fitness.Exercises.InputModels;
    using BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Area("Fitness")]
    public class ExercisesController : Controller
    {
        private readonly IExerciseService exerciseService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExercisesController(IExerciseService exerciseService, UserManager<ApplicationUser> userManager)
        {
            this.exerciseService = exerciseService;
            this.userManager = userManager;
        }

        [HttpGet("/{area}/{controller}/")]
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

        public async Task<IActionResult> Edit(string exerciseId)
        {
            var accessorId = this.userManager.GetUserId(this.User);

            // If user is not the creator of the exercise cannot access that page.
            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, accessorId))
            {
                return this.RedirectToAction(nameof(this.Browse));
            }

            var exerciseEditViewModel = await this.exerciseService.GetExerciseAsync<ExerciseEditViewModel>(exerciseId);

            return this.View(exerciseEditViewModel);
        }

        public IActionResult Publish()
        {
            return this.RedirectToAction(nameof(this.Details));
        }

        public IActionResult Details(string exerciseId)
        {
            return this.View();
        }
    }
}
