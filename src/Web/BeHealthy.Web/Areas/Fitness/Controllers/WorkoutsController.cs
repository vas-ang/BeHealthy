namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Workouts;
    using BeHealthy.Web.Dtos.Fitness.Workouts.InputModels;
    using BeHealthy.Web.Dtos.Fitness.Workouts.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Area("Fitness")]
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutService workoutService;
        private readonly UserManager<ApplicationUser> userManager;

        public WorkoutsController(IWorkoutService workoutService, UserManager<ApplicationUser> userManager)
        {
            this.workoutService = workoutService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Schedule()
        {
            var userId = this.userManager.GetUserId(this.User);

            var viewModel = await this.workoutService.GetAllUserWorkoutsAsync<WorkoutListViewModel>(userId);

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkoutInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Create();
            }

            string creatorId = this.userManager.GetUserId(this.User);

            string workoutId = await this.workoutService.CreateAsync(inputModel, creatorId);

            return this.RedirectToAction(nameof(this.Details), new { workoutId });
        }

        public async Task<IActionResult> Edit(string workoutId)
        {
            if (!await this.workoutService.WorkoutExistsAsync(workoutId))
            {
                return this.RedirectToAction(nameof(this.Schedule));
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.workoutService.IsUserWorkoutCreatorAsync(workoutId, userId))
            {
                return this.RedirectToAction(nameof(this.Schedule));
            }

            var viewModel = await this.workoutService.GetWorkoutAsync<WorkoutEditInputModel>(workoutId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkoutEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Edit), inputModel.Id);
            }

            if (!await this.workoutService.WorkoutExistsAsync(inputModel.Id))
            {
                return this.NotFound();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.workoutService.IsUserWorkoutCreatorAsync(inputModel.Id, userId))
            {
                return this.RedirectToAction(nameof(this.Schedule));
            }

            await this.workoutService.EditAsync(inputModel);

            return this.RedirectToAction(nameof(this.Details), new { workoutId = inputModel.Id });
        }

        public async Task<IActionResult> Delete(string workoutId)
        {
            if (!await this.workoutService.WorkoutExistsAsync(workoutId))
            {
                return this.NotFound();
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!await this.workoutService.IsUserWorkoutCreatorAsync(workoutId, userId))
            {
                return this.RedirectToAction(nameof(this.Schedule));
            }

            await this.workoutService.DeleteAsync(workoutId);

            return this.RedirectToAction(nameof(this.Schedule));
        }

        public async Task<IActionResult> Details(string workoutId)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (!await this.workoutService.IsUserWorkoutCreatorAsync(workoutId, userId))
            {
                return this.RedirectToAction(nameof(this.Schedule));
            }

            var viewModel = await this.workoutService.GetWorkoutAsync<WorkoutDetailsViewModel>(workoutId);

            return this.View(viewModel);
        }
    }
}
