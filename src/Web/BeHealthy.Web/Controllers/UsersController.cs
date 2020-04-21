namespace BeHealthy.Web.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Services.Data.Users;
    using BeHealthy.Web.Dtos.Users.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("{controller}/{username}")]
        public async Task<IActionResult> Details(string username)
        {
            if (!this.userService.UserExists(username))
            {
                this.TempData["ErrorMessage"] = $"User {username} does not exist.";
                return this.RedirectToAction("Index", "Home");
            }

            var viewModel = await this.userService.GetUserDetailsAsync<UserDetailsViewModel>(username);

            return this.View(viewModel);
        }
    }
}
