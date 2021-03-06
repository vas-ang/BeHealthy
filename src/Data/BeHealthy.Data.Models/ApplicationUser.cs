﻿namespace BeHealthy.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeHealthy.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.CreatedExercises = new HashSet<Exercise>();
            this.ExerciseReviews = new HashSet<ExerciseRating>();
            this.Workouts = new HashSet<Workout>();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Exercise> CreatedExercises { get; set; }

        public virtual ICollection<ExerciseRating> ExerciseReviews { get; set; }

        public virtual ICollection<Workout> Workouts { get; set; }
    }
}
