namespace BeHealthy.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        public IActionResult Index()
        {
            int settingsViewModel = 5;

            return this.View(settingsViewModel);
        }
    }
}
