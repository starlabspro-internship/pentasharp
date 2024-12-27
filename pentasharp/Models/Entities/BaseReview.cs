namespace pentasharp.Models.Entities
{    /// <summary>
     /// Represents a base review with common properties shared across different types of reviews.
     /// </summary>
    public class BaseReview
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text content of the review.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the timestamp indicating when the review was created.
        /// Defaults to the current date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the rating for the review.
        /// Typically, this is a numeric value (e.g., from 1 to 5) representing the user's rating.
        /// </summary>
        public int Rating { get; set; }
    }
}