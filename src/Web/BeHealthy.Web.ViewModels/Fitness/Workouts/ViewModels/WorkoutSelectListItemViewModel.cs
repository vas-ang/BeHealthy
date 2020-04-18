namespace BeHealthy.Web.Dtos.Fitness.Workouts.ViewModels
{
    using AutoMapper;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class WorkoutSelectListItemViewModel : SelectListItem, IHaveCustomMappings
    {
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Workout, WorkoutSelectListItemViewModel>()
                .ForMember(x => x.Text, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Value, y => y.MapFrom(z => z.Id));
        }
    }
}
