namespace BeHealthy.Web.Dtos.Users.ViewModels
{
    using System.Linq;

    using AutoMapper;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class UserCreatedExerciseViewModel : IMapFrom<Exercise>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Exercise, UserCreatedExerciseViewModel>()
                .ForMember(x => x.Rating, x => x.MapFrom(y => (double)y.ExerciseReviews.Sum(z => z.Rating) / y.ExerciseReviews.Count));
        }
    }
}
