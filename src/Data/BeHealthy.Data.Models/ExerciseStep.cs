namespace BeHealthy.Data.Models
{
    using BeHealthy.Data.Common.Models;

    public class ExerciseStep : BaseDeletableModel<int>
    {
        public string Heading { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public int ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public int? PreviousId { get; set; }

        public virtual ExerciseStep Previous { get; set; }

        public int? NextId { get; set; }

        public virtual ExerciseStep Next { get; set; }
    }
}
