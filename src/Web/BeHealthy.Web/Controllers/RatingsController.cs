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
        private readonly IRatingsService ratingsService;
        private readonly IExercisesService exercisesService;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingsController(IRatingsService reviewService, IExercisesService exerciseService, UserManager<ApplicationUser> userManager)
        {
            this.ratingsService = reviewService;
            this.exercisesService = exerciseService;
            this.userManager = userManager;
        }

        [HttpPost("Exercise")]
        public async Task<ActionResult<ExerciseRatingViewModel>> PostExerciseReview(ExerciseRatingInputModel inputModel)
        {
            if (!await this.exercisesService.ExerciseExistsAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            if (!await this.exercisesService.IsExercisePublishedAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (await this.exercisesService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId))
            {
                return this.Unauthorized();
            }

            if (await this.ratingsService.ExerciseReviewExistsAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            var viewModel = await this.ratingsService.CreateExerciseReviewAsync<ExerciseRatingInputModel, ExerciseRatingViewModel>(inputModel, userId);

            return this.Created("api/Reviews/Exercise", viewModel);
        }

        [HttpPut("Exercise")]
        public async Task<ActionResult<ExerciseRatingViewModel>> PutExerciseReview(ExerciseRatingInputModel inputModel)
        {
            if (!await this.exercisesService.ExerciseExistsAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            if (!await this.exercisesService.IsExercisePublishedAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (await this.exercisesService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId))
            {
                return this.Unauthorized();
            }

            if (!await this.ratingsService.ExerciseReviewExistsAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            var viewModel = await this.ratingsService.EditExerciseReviewAsync<ExerciseRatingInputModel, ExerciseRatingViewModel>(inputModel, userId);

            return this.Created("api/Reviews/Exercise", viewModel);
        }

        [HttpDelete("Exercise")]
        public async Task<ActionResult<ExerciseRatingViewModel>> DeleteExerciseReview([FromBody]string exerciseId)
        {
            if (!await this.exercisesService.ExerciseExistsAsync(exerciseId))
            {
                return this.BadRequest();
            }

            if (!await this.exercisesService.IsExercisePublishedAsync(exerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.ratingsService.ExerciseReviewExistsAsync(exerciseId, userId))
            {
                return this.BadRequest();
            }

            if (await this.exercisesService.IsUserExerciseCreatorAsync(exerciseId, userId))
            {
                return this.Unauthorized();
            }

            await this.ratingsService.DeleteExerciseReviewAsync(exerciseId, userId);

            return this.Ok();
        }
    }
}
