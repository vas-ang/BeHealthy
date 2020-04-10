namespace BeHealthy.Data.Models
{
    using System.Collections.Generic;

    using BeHealthy.Data.Common.Models;

    public class Exercise : BaseDeletableModel<int>
    {
        public Exercise()
        {
            this.ExerciseSteps = new HashSet<ExerciseStep>();
            this.ExerciseTags = new HashSet<ExerciseTag>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<ExerciseStep> ExerciseSteps { get; set; }

        public virtual ICollection<ExerciseTag> ExerciseTags { get; set; }
    }
}
