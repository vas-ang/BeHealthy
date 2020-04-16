namespace BeHealthy.Data.Models
{
    public class ExerciseReview
    {
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public int Rating { get; set; }
    }
}
