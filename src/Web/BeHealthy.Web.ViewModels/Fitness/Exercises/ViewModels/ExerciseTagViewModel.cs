namespace BeHealthy.Web.Dtos.Fitness.Exercises.ViewModels
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class ExerciseTagViewModel : IMapFrom<ExerciseTag>
    {
        public int TagId { get; set; }

        public string TagName { get; set; }
    }
}
