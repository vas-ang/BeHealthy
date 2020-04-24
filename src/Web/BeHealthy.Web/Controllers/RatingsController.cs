namespace BeHealthy.Web.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.Ratings;
    using BeHealthy.Web.Dtos.Ratings.InputModels;
    using BeHealthy.Web.Dtos.Ratings.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("/api/{controller}")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsService reviewService;
        private readonly IExerciseService exerciseService;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingsController(IRatingsService reviewService, IExerciseService exerciseService, UserManager<ApplicationUser> userManager)
        {
            this.reviewService = reviewService;
            this.exerciseService = exerciseService;
            this.userManager = userManager;
        }

        [HttpPost("Exercise")]
        public async Task<ActionResult<ExerciseRatingViewModel>> PostExerciseReview(ExerciseRatingInputModel inputModel)
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
                return this.Unauthorized();
            }

            if (await this.reviewService.ExerciseReviewExistsAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            var viewModel = await this.reviewService.CreateExerciseReviewAsync<ExerciseRatingInputModel, ExerciseRatingViewModel>(inputModel, userId);

            return this.Created("api/Reviews/Exercise", viewModel);
        }

        [HttpPut("Exercise")]
        public async Task<ActionResult<ExerciseRatingViewModel>> PutExerciseReview(ExerciseRatingInputModel inputModel)
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
                return this.Unauthorized();
            }

            if (!await this.reviewService.ExerciseReviewExistsAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            var viewModel = await this.reviewService.EditExerciseReviewAsync<ExerciseRatingInputModel, ExerciseRatingViewModel>(inputModel, userId);

            return this.Created("api/Reviews/Exercise", viewModel);
        }

        [HttpDelete("Exercise")]
        public async Task<ActionResult<ExerciseRatingViewModel>> DeleteExerciseReview([FromBody]string exerciseId)
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
                return this.Unauthorized();
            }

            await this.reviewService.DeleteExerciseReviewAsync(exerciseId, userId);

            return this.Ok();
        }
    }
}
