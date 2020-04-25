namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.ExerciseSteps;
    using BeHealthy.Services.Data.Ratings;
    using BeHealthy.Web.Dtos.Fitness.Exercises.InputModels;
    using BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using static BeHealthy.Common.GlobalConstants;
    using static BeHealthy.Common.Messages;

    [Authorize]
    [Area("Fitness")]
    public class ExercisesController : Controller
    {
        private readonly IExercisesService exercisesService;
        private readonly IRatingsService reviewsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExercisesController(
            IExercisesService exerciseService,
            IRatingsService reviewService,
            UserManager<ApplicationUser> userManager)
        {
            this.exercisesService = exerciseService;
            this.reviewsService = reviewService;
            this.userManager = userManager;
        }

        [Route("/{area}/{controller}/{page:int:min(1)=1}")]
        public async Task<IActionResult> Browse(int page)
        {
            var lastPage = await this.exercisesService.GetPublishedExercisesPagesCountAsync(ElementsPerPage);

            if (lastPage < page && lastPage != 0)
            {
                return this.RedirectToAction(nameof(this.Browse), new { page = 1 });
            }

            var viewModel = await this.exercisesService.GetPublishedExercisesAsync<ExerciseListItemViewModel, System.DateTime?>(page, ElementsPerPage, x => x.ModifiedOn);

            this.ViewData[CurrentPageKey] = page;
            this.ViewData[LastPageKey] = lastPage;

            return this.View(viewModel);
        }

        [Route("/{area}/{controller}/{tag:regex(^[[a-z\\-]]+$)}/{page:int:min(1)=1}")]
        public async Task<IActionResult> AllWithTag(int page, string tag)
        {
            var lastPage = await this.exercisesService.GetPublishedExercisesWithTagPagesCountAsync(ElementsPerPage, tag);

            if (lastPage < page && lastPage != 0)
            {
                return this.RedirectToAction(nameof(this.AllWithTag), new { page = 1, tag });
            }

            var viewModel = await this.exercisesService.GetPublishedExercisesWithTagAsync<ExerciseListItemViewModel, System.DateTime?>(page, ElementsPerPage, tag, x => x.ModifiedOn);

            this.ViewData[CurrentPageKey] = page;
            this.ViewData[TagKey] = tag;
            this.ViewData[LastPageKey] = lastPage;

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            string creatorId = this.userManager.GetUserId(this.User);

            string exerciseId = await this.exercisesService.CreateAsync(inputModel, creatorId);

            this.TempData[InfoMessageKey] = CreatedSuccessfully;

            return this.RedirectToAction(nameof(this.Details), new { exerciseId });
        }

        public async Task<IActionResult> Details(string exerciseId)
        {
            var viewModel = await this.exercisesService.GetExerciseAsync<ExerciseViewModel>(exerciseId);

            if (viewModel == null)
            {
                this.TempData[ErrorMessageKey] = InvalidExercise;
                return this.RedirectToAction(nameof(this.Browse));
            }

            var accessor = await this.userManager.GetUserAsync(this.User);
            bool isAccessorCreator = await this.exercisesService.IsUserExerciseCreatorAsync(exerciseId, accessor.Id);

            // If user is not the creator of the exercise when it's not published, he/she cannot access that page.
            if (!viewModel.IsPublished && !isAccessorCreator && !this.User.IsInRole(AdministratorRoleName))
            {
                this.TempData[ErrorMessageKey] = InvalidExercise;
                return this.RedirectToAction(nameof(this.Browse));
            }

            viewModel.IsAcessorCreator = isAccessorCreator;
            viewModel.ExerciseReviewUserRating = await this.reviewsService.GetExerciseReviewRatingAsync(exerciseId, accessor.Id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> ChangePublishState(string exerciseId)
        {
            var accessorId = this.userManager.GetUserId(this.User);

            if (!await this.exercisesService.IsUserExerciseCreatorAsync(exerciseId, accessorId))
            {
                this.TempData[ErrorMessageKey] = InvalidExercise;
                return this.RedirectToAction(nameof(this.Browse));
            }

            await this.exercisesService.ChangePublishState(exerciseId);

            this.TempData[InfoMessageKey] = ChangesSavedSuccessfully;

            return this.RedirectToAction(nameof(this.Details), new { exerciseId });
        }

        public async Task<IActionResult> Delete(string exerciseId)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exercisesService.IsUserExerciseCreatorAsync(exerciseId, userId) && !this.User.IsInRole(AdministratorRoleName))
            {
                this.TempData[ErrorMessageKey] = InvalidExercise;
                return this.RedirectToAction(nameof(this.Browse));
            }

            await this.exercisesService.DeleteExerciseAsync(exerciseId);

            this.TempData[InfoMessageKey] = DeletedSuccessfully;

            return this.RedirectToAction(nameof(this.Browse));
        }
    }
}
