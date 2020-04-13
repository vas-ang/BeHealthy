﻿namespace BeHealthy.Services.Data
{
    using System.Threading.Tasks;

    public interface IExerciseService
    {
        public Task<string> CreateAsync<T>(T exerciseInputModel, string creatorId);

        public Task<T> GetExerciseAsync<T>(string exerciseId);

        public Task<bool> IsUserExerciseCreatorAsync(string exerciseId, string userId);
    }
}