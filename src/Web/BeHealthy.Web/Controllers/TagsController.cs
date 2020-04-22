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

    using static BeHealthy.Common.GlobalConstants;

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
            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId))
            {
                return this.BadRequest();
            }

            if (await this.tagService.ExerciseTagExistsAsync(inputModel.ExerciseId, inputModel.TagName))
            {
                return this.BadRequest();
            }

            var result = await this.tagService.CreateExerciseTagAsync<TagViewModel>(inputModel.ExerciseId, inputModel.TagName);

            return this.Created($"api/Tags/Exercise", result);
        }

        [HttpDelete("Exercise")]
        public async Task<ActionResult> DeleteExerciseTag(ExerciseTagDeleteInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (!await this.exerciseService.IsUserExerciseCreatorAsync(inputModel.ExerciseId, userId) && !this.User.IsInRole(AdministratorRoleName))
            {
                return this.BadRequest();
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
