using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SampleApi.Models;

namespace SampleApi.DAL
{
    public class LandmarkReviewSqlDAO : ILandmarkReviewDAO
    {
        private readonly string _connectionString;

        public LandmarkReviewSqlDAO(string connectionString)
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

                    string sql = $"INSERT INTO landmarkReview (rating, title, body, landmarkId, userID) "
                        + "VALUES(@rating, @title, @body, @landmarkId, @userID)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@rating", review.Rating);
                    cmd.Parameters.AddWithValue("@title", review.Title);
                    cmd.Parameters.AddWithValue("@body", review.Detail);
                    cmd.Parameters.AddWithValue("@landmarkId", review.TypeId);
                    cmd.Parameters.AddWithValue("@userID", review.UserId);

                    review.Id = Convert.ToInt32(cmd.ExecuteScalar());

                    isCreated = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not add landmark review submission");
                Console.WriteLine(e.Message);
            }

            return isCreated;
        }

        public bool DeleteReview(int id)
        {
            bool isDeleted = false;

            try
            {
                using(SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM landmarkReview WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
                isDeleted = true;
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not delete review.");
                Console.WriteLine(e.Message);
            }

            return isDeleted;
        }

        public void Upvote(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("update landmarkReview set upvoteCount = upvoteCount + 1 where id = @id", conn);
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

                    SqlCommand cmd = new SqlCommand("update landmarkReview set downvoteCount = downvoteCount + 1 where id = @id", conn);
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

        public List<Review> GetAllReviews(int landmarkId)
        {
            List<Review> reviews = new List<Review>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"SELECT landmarkReview.id, rating, title, upvoteCount, downvoteCount, body, landmarkId, userID, [user].username FROM landmarkReview JOIN " +
                        $"[user] ON [user].id = userID WHERE landmarkId = @landmarkId;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@landmarkId", landmarkId);

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
                    SqlCommand cmd = new SqlCommand("SELECT landmarkReview.id, upvoteCount, downvoteCount, rating, title, body, landmarkId, userID, [user].username FROM landmarkReview " +
                        "JOIN [user] ON [user].id = userID WHERE landmarkReview.id = @reviewId;", conn);
                    cmd.Parameters.AddWithValue("@reviewId", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        review = ReadReview(reader);
                    }
                }
                return review;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
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

                    string sql = $"UPDATE landmarkReview SET rating = @rating, title = @title, " +
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
            Review review = new Review
            {
                Id = Convert.ToInt32(reader["id"]),
                Title = Convert.ToString(reader["title"]),
                Rating = Convert.ToDecimal(reader["rating"]),
                Detail = Convert.ToString(reader["body"]),
                TypeId = Convert.ToInt32(reader["landmarkId"]),
                UpvoteCount = Convert.ToInt32(reader["upvoteCount"]),
                DownvoteCount = Convert.ToInt32(reader["downvoteCount"]),
                UserId = Convert.ToInt32(reader["userID"]),
                UserName = Convert.ToString(reader["username"])
            };

            return review;
        }
    }
}
