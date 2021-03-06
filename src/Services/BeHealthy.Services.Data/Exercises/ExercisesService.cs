﻿namespace BeHealthy.Services.Data.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ExercisesService : IExercisesService
    {
        private readonly IDeletableEntityRepository<Exercise> exerciseRepository;

        public ExercisesService(IDeletableEntityRepository<Exercise> exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public async Task<string> CreateAsync<T>(T exerciseInputModel, string creatorId)
        {
            var exercise = exerciseInputModel.To<Exercise>();

            exercise.CreatorId = creatorId;

            await this.exerciseRepository.AddAsync(exercise);

            await this.exerciseRepository.SaveChangesAsync();

            return exercise.Id;
        }

        public async Task<bool> IsUserExerciseCreatorAsync(string exerciseId, string userId)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == exerciseId && x.CreatorId == userId);

        public async Task<T> GetExerciseAsync<T>(string exerciseId)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .Where(x => x.Id == exerciseId)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task<bool> ChangePublishState(string exerciseId)
        {
            var exercise = await this.exerciseRepository.All().FirstOrDefaultAsync(x => x.Id == exerciseId);

            exercise.IsPublished = !exercise.IsPublished;

            this.exerciseRepository.Update(exercise);

            await this.exerciseRepository.SaveChangesAsync();

            return exercise.IsPublished;
        }

        public async Task<bool> ExerciseExistsAsync(string exerciseId)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == exerciseId);

        public async Task<string> GetExerciseIdByStepIdAsync(int exerciseStepId)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .Where(x => x.ExerciseSteps.Any(es => es.Id == exerciseStepId))
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetPublishedExercisesAsync<T, TKey>(int page, int perPage, Expression<Func<Exercise, TKey>> orderCriteria)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .Where(x => x.IsPublished)
                .OrderByDescending(orderCriteria)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .To<T>()
                .ToArrayAsync();

        public async Task<IEnumerable<T>> GetUnpublishedExercisesAsync<T>(string userId)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .Where(x => !x.IsPublished && x.CreatorId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToArrayAsync();

        public async Task<bool> IsExercisePublishedAsync(string exerciseId)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == exerciseId && x.IsPublished);

        public async Task<IEnumerable<T>> GetPublishedExercisesWithTagAsync<T, TKey>(int page, int perPage, string tag, Expression<Func<Exercise, TKey>> orderCriteria)
            => await this.exerciseRepository
                .AllAsNoTracking()
                .Where(x => x.ExerciseTags
                    .Any(y => y.Tag.Name == tag) && x.IsPublished)
                .OrderByDescending(orderCriteria)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .To<T>()
                .ToArrayAsync();

        public async Task<int> GetPublishedExercisesPagesCountAsync(int perPage)
            => (int)Math.Ceiling(await this.exerciseRepository
                .AllAsNoTracking()
                .Where(x => x.IsPublished)
                .CountAsync() / (double)perPage);

        public async Task<int> GetPublishedExercisesWithTagPagesCountAsync(int perPage, string tag)
             => (int)Math.Ceiling(await this.exerciseRepository
                .AllAsNoTracking()
                .Where(x => x.ExerciseTags
                    .Any(y => y.Tag.Name == tag) && x.IsPublished)
                .CountAsync() / (double)perPage);

        public async Task DeleteExerciseAsync(string exerciseId)
        {
            var exercise = await this.exerciseRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == exerciseId);

            this.exerciseRepository.Delete(exercise);

            await this.exerciseRepository.SaveChangesAsync();
        }
    }
}
