namespace BeHealthy.Data.Models
{
    using BeHealthy.Data.Common.Models;

    using BeHealthy.Data.Models.Enumerators;

    public class ExerciseReview
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public Rating Rating { get; set; }

        public string Comment { get; set; }
    }
}
