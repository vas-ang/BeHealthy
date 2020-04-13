namespace BeHealthy.Data.Models
{
    public class ExerciseTag
    {
        public string ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
