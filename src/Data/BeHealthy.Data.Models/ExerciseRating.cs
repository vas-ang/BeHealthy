namespace BeHealthy.Data.Models
{
    public class ExerciseRating
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public int Rating { get; set; }
    }
}
