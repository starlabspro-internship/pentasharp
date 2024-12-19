using pentasharp.Models.Entities;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    public interface IReviewService
    {
        Task<bool> SubmitReviewAsync(UserReview review, int? userId);
    }
}