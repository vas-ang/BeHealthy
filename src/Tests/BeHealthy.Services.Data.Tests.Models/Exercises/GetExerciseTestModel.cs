namespace BeHealthy.Services.Data.Tests.Models.Exercises
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class GetExerciseTestModel : IMapFrom<Exercise>
    {
        public string Id { get; set; }
    }
}
