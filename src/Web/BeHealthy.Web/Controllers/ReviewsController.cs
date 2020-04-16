namespace BeHealthy.Web.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.Reviews;
    using BeHealthy.Web.Dtos.Reviews.InputModels;
    using BeHealthy.Web.Dtos.Reviews.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("/api/{controller}")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService reviewService;
        private readonly IExerciseService exerciseService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewsController(IReviewService reviewService, IExerciseService exerciseService, UserManager<ApplicationUser> userManager)
        {
            this.reviewService = reviewService;
            this.exerciseService = exerciseService;
            this.userManager = userManager;
        }

        [HttpPost("Exercise")]
        public async Task<ActionResult<ExerciseReviewViewModel>> PostExerciseReview(ExerciseReviewInputModel inputModel)
        {
            if (!await this.exerciseService.ExerciseExistsAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            if (!await this.exerciseService.IsExercisePublishedAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (await this.exerciseService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            if (await this.reviewService.ExerciseReviewExistsAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            var viewModel = await this.reviewService.CreateExerciseReviewAsync<ExerciseReviewInputModel, ExerciseReviewViewModel>(inputModel, userId);

            return this.Created("api/Reviews/Exercise", viewModel);
        }

        [HttpPut("Exercise")]
        public async Task<ActionResult<ExerciseReviewViewModel>> PutExerciseReview(ExerciseReviewInputModel inputModel)
        {
            if (!await this.exerciseService.ExerciseExistsAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            if (!await this.exerciseService.IsExercisePublishedAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (await this.exerciseService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            if (!await this.reviewService.ExerciseReviewExistsAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            var viewModel = await this.reviewService.EditExerciseReviewAsync<ExerciseReviewInputModel, ExerciseReviewViewModel>(inputModel, userId);

            return this.Created("api/Reviews/Exercise", viewModel);
        }

        [HttpDelete("Exercise")]
        public async Task<ActionResult<ExerciseReviewViewModel>> DeleteExerciseReview([FromBody]string exerciseId)
        {
            if (!await this.exerciseService.ExerciseExistsAsync(exerciseId))
            {
                return this.BadRequest();
            }

            if (!await this.exerciseService.IsExercisePublishedAsync(exerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.reviewService.ExerciseReviewExistsAsync(exerciseId, userId))
            {
                return this.BadRequest();
            }

            if (await this.exerciseService.IsUserExerciseCreatorAsync(exerciseId, userId))
            {
                return this.BadRequest();
            }

            await this.reviewService.DeleteExerciseReviewAsync(exerciseId, userId);

            return this.Ok();
        }
    }
}
