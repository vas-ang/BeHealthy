namespace BeHealthy.Data.Models
{
    using BeHealthy.Data.Common.Models;

    public class ExerciseStep : BaseDeletableModel<int>
    {
        public string Heading { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public int OrderNumber { get; set; }

        public string ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }
    }
}
