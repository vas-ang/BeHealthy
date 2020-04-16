namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.Reviews;
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
        private readonly IReviewService reviewService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExercisesController(IExerciseService exerciseService, IReviewService reviewService, UserManager<ApplicationUser> userManager)
        {
            this.exerciseService = exerciseService;
            this.reviewService = reviewService;
            this.userManager = userManager;
        }

        [HttpGet("/{area}/{controller}/{page:int:min(1)=1}")]
        public async Task<IActionResult> Browse(int page)
        {
            var viewModel = await this.exerciseService.GetPublishedExercisesAsync<ExerciseListItemViewModel, System.DateTime>(page, 5, x => x.CreatedOn);

            return this.View(viewModel);
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

            return this.RedirectToAction(nameof(this.Details), new { exerciseId });
        }

        public async Task<IActionResult> Details(string exerciseId)
        {
            // If the exercise is deleted, should not display.
            if (!await this.exerciseService.ExerciseExistsAsync(exerciseId))
            {
                return this.NotFound();
            }

            var exerciseViewModel = await this.exerciseService.GetExerciseAsync<ExerciseViewModel>(exerciseId);

            var accessor = await this.userManager.GetUserAsync(this.User);
            bool isAccessorCreator = await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, accessor.Id);

            // If user is not the creator of the exercise when it's not published, he/she cannot access that page.
            if (!exerciseViewModel.IsPublished && !isAccessorCreator)
            {
                return this.RedirectToAction(nameof(this.Browse));
            }

            exerciseViewModel.IsAcessorCreator = isAccessorCreator;

            exerciseViewModel.ExerciseReviewUserRating = await this.reviewService.GetExerciseReviewRatingAsync(exerciseId, accessor.Id);

            return this.View(exerciseViewModel);
        }

        public async Task<IActionResult> ChangePublishState(string exerciseId)
        {
            // If the exercise is deleted, should not display.
            if (!await this.exerciseService.ExerciseExistsAsync(exerciseId))
            {
                return this.NotFound();
            }

            var accessorId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, accessorId))
            {
                return this.RedirectToAction(nameof(this.Browse));
            }

            await this.exerciseService.ChangePublishState(exerciseId);

            return this.RedirectToAction(nameof(this.Details), new { exerciseId });
        }
    }
}
