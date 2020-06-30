using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SampleApi.Models;

namespace SampleApi.DAL
{
    public class ItineraryReviewSqlDAO : IItineraryReviewDAO
    {
        private string _connectionString;

        public ItineraryReviewSqlDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CreateReview(Review review)
        {
            bool isCreated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"INSERT INTO itineraryReview (rating, title, body, itineraryId, userID) "
                        + "VALUES(@rating, @title, @body, @itineraryId, @userID)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@rating", review.Rating);
                    cmd.Parameters.AddWithValue("@title", review.Title);
                    cmd.Parameters.AddWithValue("@body", review.Detail);
                    cmd.Parameters.AddWithValue("@itineraryId", review.TypeId);
                    cmd.Parameters.AddWithValue("@userID", review.UserId);

                    review.Id = Convert.ToInt32(cmd.ExecuteScalar());

                    isCreated = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not add itinerary review");
                Console.WriteLine(e.Message);
            }

            return isCreated;
        }

        public void Upvote(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("update itineraryReview set upvoteCount = upvoteCount + 1 where id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not upvote review.");
                Console.WriteLine(e.Message);
            }

        }

        public void Downvote(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("update itineraryReview set downvoteCount = downvoteCount + 1 where id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not upvote review.");
                Console.WriteLine(e.Message);
            }
        }


        public bool DeleteReview(int id)
        {
            bool isDeleted = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM itineraryReview WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();

                    isDeleted = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not delete review.");
                Console.WriteLine(e.Message);
            }

            return isDeleted;
        }

        public List<Review> GetAllReviews(int id)
        {
            List<Review> reviews = new List<Review>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"SELECT itineraryReview.id, rating, title, body, upvoteCount, downvoteCount, itineraryId, userID, [user].username FROM itineraryReview JOIN " +
                        $"[user] ON [user].id = userID WHERE itineraryId = @itineraryId;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@itineraryId", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Review review = ReadReview(reader);

                        reviews.Add(review);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not retrieve reviews from database.");
                Console.WriteLine(e.Message);
            }

            return reviews;
        }

        public Review GetReview(int id)
        {
            Review review = new Review();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT itineraryReview.id, rating, title, upvoteCount, downvoteCount, body, itineraryId, userID, [user].username FROM itineraryReview JOIN " +
                        "[user] ON [user].id = userID WHERE itineraryReview.id = @reviewId;", conn);
                    cmd.Parameters.AddWithValue("@reviewId", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        review = ReadReview(reader);
                    }
                }
                return review;
            }
            catch
            {
                return review;
            }
        }

        public bool UpdateReview(Review review)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"UPDATE itineraryReview SET rating = @rating, title = @title, " +
                        $"body = @body WHERE id=@id;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", review.Id);
                    cmd.Parameters.AddWithValue("@rating", review.Rating);
                    cmd.Parameters.AddWithValue("@title", review.Title);
                    cmd.Parameters.AddWithValue("@body", review.Detail);
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        review = ReadReview(reader);
                    }

                    isUpdated = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not update review.");
                Console.WriteLine(e.Message);
            }
            return isUpdated;
        }

        private Review ReadReview(SqlDataReader reader)
        {
            Review review = new Review();

            review.Id = Convert.ToInt32(reader["id"]);
            review.Title = Convert.ToString(reader["title"]);
            review.Rating = Convert.ToDecimal(reader["rating"]);
            review.Detail = Convert.ToString(reader["body"]);
            review.TypeId = Convert.ToInt32(reader["itineraryId"]);
            review.UserId = Convert.ToInt32(reader["userID"]);
            review.UserName = Convert.ToString(reader["username"]);
            review.UpvoteCount = Convert.ToInt32(reader["upvoteCount"]);
            review.DownvoteCount = Convert.ToInt32(reader["downvoteCount"]);

            return review;
        }
    }
}
