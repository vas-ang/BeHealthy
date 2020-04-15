namespace BeHealthy.Web.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.Tags;
    using BeHealthy.Web.Dtos.Tags.InputModels;
    using BeHealthy.Web.Dtos.Tags.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("/api/{controller}")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService tagService;
        private readonly IExerciseService exerciseService;
        private readonly UserManager<ApplicationUser> userManager;

        public TagsController(ITagService tagService, IExerciseService exerciseService, UserManager<ApplicationUser> userManager)
        {
            this.tagService = tagService;
            this.exerciseService = exerciseService;
            this.userManager = userManager;
        }

        [HttpPost("Exercise")]
        public async Task<ActionResult<TagViewModel>> PostExerciseTag(ExerciseTagCreateInputModel inputModel)
        {
            if (!await this.exerciseService.ExerciseExistsAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId))
            {
                return this.Unauthorized();
            }

            if (await this.tagService.ExerciseTagExistsAsync(inputModel.ExerciseId, inputModel.TagName))
            {
                return this.BadRequest();
            }

            var result = await this.tagService.CreateExerciseTagAsync<TagViewModel>(inputModel.ExerciseId, inputModel.TagName);

            return this.Created($"api/Tags/Exercises/{inputModel.ExerciseId}", result);
        }

        [HttpDelete("Exercise")]
        public async Task<ActionResult> DeleteExerciseTag(ExerciseTagDeleteInputModel inputModel)
        {
            if (!await this.exerciseService.ExerciseExistsAsync(inputModel.ExerciseId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId))
            {
                return this.Unauthorized();
            }

            if (!await this.tagService.ExerciseTagExistsAsync(inputModel.ExerciseId, inputModel.TagId))
            {
                return this.BadRequest();
            }

            await this.tagService.DeleteExerciseTagAsync(inputModel);

            return this.Ok();
        }
    }
}
