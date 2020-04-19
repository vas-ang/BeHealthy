namespace BeHealthy.Web.Areas.Fitness.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.WorkoutExercises;
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
        private readonly IExerciseService exerciseService;
        private readonly IWorkoutExerciseService workoutExerciseService;
        private readonly UserManager<ApplicationUser> userManager;

        public WorkoutsController(
            IWorkoutService workoutService,
            IExerciseService exerciseService,
            IWorkoutExerciseService workoutExerciseService,
            UserManager<ApplicationUser> userManager)
        {
            this.workoutService = workoutService;
            this.exerciseService = exerciseService;
            this.workoutExerciseService = workoutExerciseService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Schedule()
        {
            var userId = this.userManager.GetUserId(this.User);

            var viewModel = await this.workoutService.GetAllUserWorkoutsAsync<WorkoutViewModel>(userId);

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

            await this.workoutService.DeleteWorkoutAsync(workoutId);

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

        public async Task<IActionResult> AddExercise(string exerciseId)
        {
            var userId = this.userManager.GetUserId(this.User);

            var viewModel = new WorkoutExerciseCreateInputModel
            {
                ExerciseId = exerciseId,
            };

            this.TempData["WorkoutSelectListItems"] = await this.workoutService.GetAllUserWorkoutsAsync<WorkoutSelectListItemViewModel>(userId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddExercise(WorkoutExerciseCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData["ErrorMessage"] = "Entered data is invalid.";
                return this.RedirectToAction(nameof(this.AddExercise), new { inputModel.ExerciseId });
            }

            if (!await this.exerciseService.ExerciseExistsAsync(inputModel.ExerciseId) ||
                !await this.exerciseService.IsExercisePublishedAsync(inputModel.ExerciseId))
            {
                this.TempData["ErrorMessage"] = "Exercise id is invalid.";
                return this.RedirectToAction(nameof(this.AddExercise), new { inputModel.ExerciseId });
            }

            var userId = this.userManager.GetUserId(this.User);

            if (await this.workoutExerciseService.WorkoutExerciseExistsAsync(inputModel.WorkoutId, inputModel.ExerciseId))
            {
                this.TempData["ErrorMessage"] = "This exercise is already bound to the selected workout.";
                return this.RedirectToAction(nameof(this.AddExercise), new { inputModel.ExerciseId });
            }

            if (!await this.workoutService.IsUserWorkoutCreatorAsync(inputModel.WorkoutId, userId) ||
                !await this.workoutService.WorkoutExistsAsync(inputModel.WorkoutId))
            {
                this.TempData["ErrorMessage"] = "Workout id is invalid.";
                return this.RedirectToAction(nameof(this.AddExercise), new { inputModel.ExerciseId });
            }

            await this.workoutExerciseService.AddWorkoutExerciseAsync(inputModel);

            return this.Redirect("/Fitness/Exercises");
        }

        public async Task<IActionResult> RemoveExercise(string workoutId, string exerciseId)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (!await this.workoutService.IsUserWorkoutCreatorAsync(workoutId, userId))
            {
                this.TempData["ErrorMessage"] = "Invalid workout id.";
                return this.RedirectToAction(nameof(this.Schedule));
            }

            var success = await this.workoutExerciseService.DeleteWorkoutExerciseAsync(workoutId, exerciseId);

            if (!success)
            {
                this.TempData["ErrorMessage"] = "Exercise could not be removed.";
            }

            return this.RedirectToAction(nameof(this.Details), new { workoutId });
        }
    }
}
