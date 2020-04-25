namespace BeHealthy.Web.Controllers
{
    using System.Threading.Tasks;

    using BeHealthy.Services.Data.Users;
    using BeHealthy.Web.Dtos.Users.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static BeHealthy.Common.GlobalConstants;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService userService)
        {
            this.usersService = userService;
        }

        [Route("{controller}/{username:required}")]
        public async Task<IActionResult> Details(string username)
        {
            if (!await this.usersService.UserExistsAsync(username))
            {
                this.TempData[ErrorMessageKey] = $"User {username} does not exist.";
                return this.RedirectToAction("Index", "Home");
            }

            var viewModel = await this.usersService.GetUserDetailsAsync<UserDetailsViewModel>(username);

            return this.View(viewModel);
        }
    }
}
