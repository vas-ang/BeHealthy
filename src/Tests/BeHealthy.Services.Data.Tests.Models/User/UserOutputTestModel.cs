namespace BeHealthy.Services.Data.Tests.Models.User
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class UserOutputTestModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
