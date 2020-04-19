namespace BeHealthy.Web.Dtos.Users.ViewModels
{
    using System;
    using System.Collections.Generic;

    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<UserCreatedExerciseViewModel> CreatedExercises { get; set; }
    }
}
