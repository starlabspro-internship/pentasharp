namespace pentasharp.Models.Entities
{
    /// <summary>
    /// Represents a user-specific review that inherits from BaseReview.
    /// This class includes additional details about the user who posted the review.
    /// </summary>
    public class UserReview : BaseReview
    {
        /// <summary>
        /// Gets or sets the identifier of the user who posted the review.
        /// This serves as a foreign key linking to the user's information.
        /// </summary>
        public int UserId { get; set; } // Foreign key linking to the user who posted the review

        /// <summary>
        /// Gets or sets the name of the user who posted the review.
        /// This can be used to display the review author's name on the UI.
        /// </summary>
        public string UserName { get; set; } // Name of the user who posted the review

    }
}