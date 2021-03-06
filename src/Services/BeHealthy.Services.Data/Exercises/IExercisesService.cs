﻿namespace BeHealthy.Services.Data.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using BeHealthy.Data.Models;

    public interface IExercisesService
    {
        public Task<string> CreateAsync<T>(T exerciseInputModel, string creatorId);

        public Task<T> GetExerciseAsync<T>(string exerciseId);

        public Task<bool> IsExercisePublishedAsync(string exerciseId);

        public Task<string> GetExerciseIdByStepIdAsync(int exerciseStepId);

        public Task<bool> IsUserExerciseCreatorAsync(string exerciseId, string userId);

        public Task<bool> ChangePublishState(string exerciseId);

        public Task<bool> ExerciseExistsAsync(string exerciseId);

        public Task<IEnumerable<T>> GetPublishedExercisesAsync<T, TKey>(int page, int perPage, Expression<Func<Exercise, TKey>> orderCriteria);

        public Task<int> GetPublishedExercisesPagesCountAsync(int perPage);

        public Task<IEnumerable<T>> GetPublishedExercisesWithTagAsync<T, TKey>(int page, int perPage, string tag, Expression<Func<Exercise, TKey>> orderCriteria);

        public Task<int> GetPublishedExercisesWithTagPagesCountAsync(int perPage, string tag);

        public Task<IEnumerable<T>> GetUnpublishedExercisesAsync<T>(string userId);

        public Task DeleteExerciseAsync(string exerciseId);
    }
}
