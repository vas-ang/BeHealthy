﻿namespace BeHealthy.Services.Data.Tests.Models.Exercise
{
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;

    public class CreateExerciseTestModel : IMapTo<Exercise>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
