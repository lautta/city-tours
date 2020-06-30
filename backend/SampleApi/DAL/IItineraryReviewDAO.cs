using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public interface IItineraryReviewDAO
    {
        Review GetReview(int id);

        List<Review> GetAllReviews(int id);

        bool CreateReview(Review review);

        bool DeleteReview(int id);

        bool UpdateReview(Review review);

        void Upvote(int id);

        void Downvote(int id);
    }
}
