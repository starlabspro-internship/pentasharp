using pentasharp.Data;
using pentasharp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using pentasharp.Interfaces;

namespace pentasharp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SubmitReviewAsync(UserReview review, int? userId)
        {
            if (review == null)
                return false;

            string userName = "Anonymous";

            if (userId.HasValue)
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId.Value);

                if (user != null)
                {
                    userName = user.FirstName;
                    review.UserId = userId.Value;
                }
            }

            review.UserName ??= userName;

            _context.Add(review);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}