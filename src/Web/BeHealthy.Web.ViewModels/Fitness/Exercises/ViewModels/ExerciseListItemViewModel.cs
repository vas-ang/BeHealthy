namespace BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels
{
    using System.Linq;

    using AutoMapper;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseListItemViewModel : IMapFrom<Exercise>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }

        public string CreatorUserName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Exercise, ExerciseListItemViewModel>()
                .ForMember(x => x.Rating, x => x.MapFrom(y => y.ExerciseReviews.Sum(z => z.Rating) / (double)y.ExerciseReviews.Count));
        }
    }
}
