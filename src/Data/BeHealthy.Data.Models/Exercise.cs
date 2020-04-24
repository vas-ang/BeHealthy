namespace BeHealthy.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeHealthy.Data.Common.Models;

    public class Exercise : BaseDeletableModel<string>
    {
        public Exercise()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ExerciseSteps = new HashSet<ExerciseStep>();
            this.ExerciseTags = new HashSet<ExerciseTag>();
            this.ExerciseReviews = new HashSet<ExerciseRating>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<ExerciseStep> ExerciseSteps { get; set; }

        public virtual ICollection<ExerciseTag> ExerciseTags { get; set; }

        public virtual ICollection<ExerciseRating> ExerciseReviews { get; set; }
    }
}
