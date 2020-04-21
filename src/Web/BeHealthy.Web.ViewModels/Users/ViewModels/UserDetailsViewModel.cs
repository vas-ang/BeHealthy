namespace BeHealthy.Web.Dtos.Users.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<UserCreatedExerciseViewModel> CreatedExercises { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ApplicationUser, UserDetailsViewModel>()
                .ForMember<IEnumerable<UserCreatedExerciseViewModel>>(x => x.CreatedExercises, x => x.MapFrom(y => y.CreatedExercises.Where(z => z.IsPublished)));
        }
    }
}
