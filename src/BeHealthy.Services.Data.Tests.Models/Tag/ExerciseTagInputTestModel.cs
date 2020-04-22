﻿namespace BeHealthy.Services.Data.Tests.Models.Tag
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class ExerciseTagInputTestModel : IMapTo<ExerciseTag>
    {
        public string ExerciseId { get; set; }

        public int TagId { get; set; }
    }
}
