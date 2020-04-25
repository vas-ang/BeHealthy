namespace BeHealthy.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "UnpublishedExercises")]
    public class UnpublishedExercisesViewComponent : ViewComponent
    {
        private readonly IExercisesService exercisesService;
        private readonly UserManager<ApplicationUser> userManager;

        public UnpublishedExercisesViewComponent(IExercisesService exerciseService, UserManager<ApplicationUser> userManager)
        {
            this.exercisesService = exerciseService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.userManager.GetUserId(this.UserClaimsPrincipal);

            var viewModel = await this.exercisesService.GetUnpublishedExercisesAsync<UnpublishedExerciseViewModel>(userId);

            return this.View(viewModel);
        }
    }
}
