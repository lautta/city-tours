using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public class LandmarkSqlDAO : ILandmarkDAO
    {
        private readonly string _connectionString;

        public LandmarkSqlDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Landmark GetLandmark(int id)
        {
            Landmark landmark = new Landmark();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM landmark WHERE id = @landmarkId", conn);
                    cmd.Parameters.AddWithValue("@landmarkId", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        landmark = ReadLandmark(reader);
                    }
                }
                return landmark;
            }
            catch
            {
                return landmark;
            }
        }

        public bool AddLandmark(Landmark landmark)
        {
            bool isAdded = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"INSERT INTO landmark(longName,shortName,latitude,longitude,city,details,streetNumber,zip,streetName,state,stateCode,isApproved)" +
                        " VALUES(@longName,@shortName,@latitude,@longitude,@city,@details,@streetNumber,@zip,@streetName,@state,@stateCode,@isApproved)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@longName", landmark.LongName);
                    cmd.Parameters.AddWithValue("@shortName", landmark.ShortName);
                    cmd.Parameters.AddWithValue("@latitude", landmark.Latitude);
                    cmd.Parameters.AddWithValue("@longitude", landmark.Longitude);
                    cmd.Parameters.AddWithValue("@city", landmark.City);
                    cmd.Parameters.AddWithValue("@details", landmark.Details);
                    cmd.Parameters.AddWithValue("@streetNumber", landmark.StreetNumber);
                    cmd.Parameters.AddWithValue("@zip", landmark.Zip);
                    cmd.Parameters.AddWithValue("@streetName", landmark.StreetName);
                    cmd.Parameters.AddWithValue("@state", landmark.State);
                    cmd.Parameters.AddWithValue("@stateCode", landmark.StateCode);
                    cmd.Parameters.AddWithValue("@isApproved", landmark.IsApproved);

                    cmd.ExecuteNonQuery();
                    isAdded = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not add landmark");
                Console.WriteLine(e.Message);
            }

            return isAdded;
        }

        public IList<Landmark> GetAllLandmarks()
        {
            List<Landmark> landmarks = new List<Landmark>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM landmark ORDER BY longName;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        landmarks.Add(ReadLandmark(reader));
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not retrieve landmarks from database.");
                Console.WriteLine(e.Message);
            }

            return landmarks;
        }

        public IList<Landmark> GetUnapprovedLandmarks()
        {
            List<Landmark> landmarks = new List<Landmark>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM landmark WHERE isApproved = 0 ORDER BY longName;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        landmarks.Add(ReadLandmark(reader));
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not retrieve landmarks from database.");
                Console.WriteLine(e.Message);
            }

            return landmarks;
        }

        public IList<Landmark> GetApprovedLandmarks()
        {
            List<Landmark> landmarks = new List<Landmark>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM landmark WHERE isApproved = 1 ORDER BY longName;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        landmarks.Add(ReadLandmark(reader));
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not retrieve landmarks from database.");
                Console.WriteLine(e.Message);
            }

            return landmarks;
        }

        public bool DeleteLandmark(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM landmark_itinerary WHERE landmarkId = @landmarkId", conn);
                    cmd.Parameters.AddWithValue("@landmarkId", id);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("DELETE FROM landmarkReview WHERE landmarkId = @landmarkId", conn);
                    cmd.Parameters.AddWithValue("@landmarkId", id);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("DELETE FROM landmark WHERE id = @landmarkId", conn);
                    cmd.Parameters.AddWithValue("@landmarkId", id);

                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool UpdateLandmark(Landmark landmark)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"UPDATE landmark SET longName = @longName, shortName = @shortName, " +
                        $"latitude=@latitude, longitude=@longitude, city=@city, details=@details, streetNumber=@streetNumber, " +
                        $"zip=@zip, streetName=@streetName, state=@state, stateCode=@stateCode, isApproved=@isApproved " +
                        $"WHERE id=@id;";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", landmark.Id);
                    cmd.Parameters.AddWithValue("@longName", landmark.LongName);
                    cmd.Parameters.AddWithValue("@shortName", landmark.ShortName);
                    cmd.Parameters.AddWithValue("@latitude", landmark.Latitude);
                    cmd.Parameters.AddWithValue("@longitude", landmark.Longitude);
                    cmd.Parameters.AddWithValue("@city", landmark.City);
                    cmd.Parameters.AddWithValue("@details", landmark.Details);
                    cmd.Parameters.AddWithValue("@streetNumber", landmark.StreetNumber);
                    cmd.Parameters.AddWithValue("@zip", landmark.Zip);
                    cmd.Parameters.AddWithValue("@streetName", landmark.StreetName);
                    cmd.Parameters.AddWithValue("@state", landmark.State);
                    cmd.Parameters.AddWithValue("@stateCode", landmark.StateCode);
                    cmd.Parameters.AddWithValue("@isApproved", landmark.IsApproved);
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        landmark = ReadLandmark(reader);
                    }

                    isUpdated = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not update landmark.");
                Console.WriteLine(e.Message);
            }

            return isUpdated;
        }

        private Landmark ReadLandmark(SqlDataReader reader)
        {
            Landmark landmark = new Landmark
            {
                Id = Convert.ToInt32(reader["id"]),
                LongName = Convert.ToString(reader["longName"]),
                ShortName = Convert.ToString(reader["shortName"]),
                Latitude = Convert.ToDouble(reader["latitude"]),
                Longitude = Convert.ToDouble(reader["longitude"]),
                City = Convert.ToString(reader["city"]),
                Details = Convert.ToString(reader["details"]),
                StreetNumber = Convert.ToInt32(reader["streetNumber"]),
                Zip = Convert.ToInt32(reader["zip"]),
                StreetName = Convert.ToString(reader["streetName"]),
                State = Convert.ToString(reader["state"]),
                StateCode = Convert.ToString(reader["stateCode"]),
                IsApproved = Convert.ToBoolean(reader["isApproved"])
            };

            return landmark;
        }
    }
}
