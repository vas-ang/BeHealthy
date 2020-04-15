namespace BeHealthy.Data.Models
{
    using System.Collections.Generic;

    using BeHealthy.Data.Common.Models;

    public class Tag : BaseDeletableModel<int>
    {
        public Tag()
        {
            this.ExerciseTags = new HashSet<ExerciseTag>();
        }

        public string Name { get; set; }

        public virtual ICollection<ExerciseTag> ExerciseTags { get; set; }
    }
}
