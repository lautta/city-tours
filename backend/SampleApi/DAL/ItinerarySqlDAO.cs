using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SampleApi.Models;

namespace SampleApi.DAL
{
    public class ItinerarySqlDAO : IItineraryDAO
    {
        private string _connectionString;

        public ItinerarySqlDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CreateItinerary(Itinerary itinerary)
        {
            bool isCreated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"INSERT INTO itinerary(name, startingLatitude, startingLongitude, ownerID) output inserted.id "
                        + "VALUES(@name, @startingLatitude, @startingLongitude, @ownerID)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@name", itinerary.Name);
                    cmd.Parameters.AddWithValue("@startingLatitude", itinerary.StartLat);
                    cmd.Parameters.AddWithValue("@startingLongitude", itinerary.StartLng);
                    cmd.Parameters.AddWithValue("@ownerID", itinerary.OwnerID);

                    itinerary.Id = Convert.ToInt32(cmd.ExecuteScalar());

                    isCreated = true;
                }

                SaveLandmarks(itinerary);
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not add itinerary submission");
                Console.WriteLine(e.Message);
            }

            return isCreated;
        }

        private void SaveLandmarks(Itinerary itinerary)
        {
            foreach (int id in itinerary.LandmarkIDs)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();

                        string sql = $"INSERT INTO landmark_itinerary (landmarkId, itineraryId)"
                            + "VALUES(@landmarkID, @itineraryID)";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@landmarkID", id);
                        cmd.Parameters.AddWithValue("@itineraryID", itinerary.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Could not add landmark list submission");
                    Console.WriteLine(e.Message);
                }
            }
        }

        public bool DeleteItinerary(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM landmark_itinerary WHERE itineraryId = @itineraryId", conn);
                    cmd.Parameters.AddWithValue("@itineraryId", id);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("DELETE FROM itinerary WHERE id = @itineraryId", conn);
                    cmd.Parameters.AddWithValue("@itineraryId", id);

                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Itinerary> GetAllItinerary()
        {
            List<Itinerary> itineraries = new List<Itinerary>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM itinerary;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Itinerary itinerary = ReadItinerary(reader);
                        //PopulateLandmarks(itinerary);

                        itineraries.Add(itinerary);
                    }
                }
                foreach (Itinerary itinerary in itineraries)
                {
                    PopulateLandmarks(itinerary);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not retrieve itineraries from database.");
                Console.WriteLine(e.Message);
                throw;
            }
            return itineraries;
        }

        private Itinerary PopulateLandmarks(Itinerary itinerary)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string moreSQL = "select id, longName, city, stateCode, latitude, longitude from landmark l join landmark_itinerary li on li.landmarkId = l.id where li.itineraryId = @id";
                    SqlCommand subCmd = new SqlCommand(moreSQL, conn);
                    subCmd.Parameters.AddWithValue("@id", itinerary.Id);

                    SqlDataReader reader = subCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Landmark landmark = new Landmark();
                        landmark.Id = Convert.ToInt32(reader["id"]);
                        landmark.LongName = Convert.ToString(reader["longName"]);
                        landmark.City = Convert.ToString(reader["city"]);
                        landmark.StateCode = Convert.ToString(reader["stateCode"]);
                        landmark.Latitude = Convert.ToDouble(reader["latitude"]);
                        landmark.Longitude = Convert.ToDouble(reader["longitude"]);
                        itinerary.Landmarks.Add(landmark);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not retrieve landmarks from database.");
                Console.WriteLine(e.Message);
                throw;
            }

            return itinerary;
        }

        public Itinerary GetItinerary(int id)
        {
            Itinerary itinerary = new Itinerary();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM itinerary WHERE id = @id;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        itinerary = ReadItinerary(reader);
                    }
                }
                PopulateLandmarks(itinerary);
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not retrieve desired itinerary.");
                Console.WriteLine(e.Message);
                throw;
            }

            return itinerary;
        }

        public bool UpdateItinerary(Itinerary itinerary)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = $"update itinerary set name = @name, startingLatitude = @startLat, " +
                        $"startingLongitude = @startLong WHERE id=@id;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", itinerary.Id);
                    cmd.Parameters.AddWithValue("@name", itinerary.Name);
                    cmd.Parameters.AddWithValue("@startLat", itinerary.StartLat);
                    cmd.Parameters.AddWithValue("@startLong", itinerary.StartLng);
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        itinerary = ReadItinerary(reader);
                    }
                    isUpdated = true;
                }
                DeleteItineraryLandmarks(itinerary.Id);
                SaveLandmarks(itinerary);
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not update itinerary.");
                Console.WriteLine(e.Message);
                throw;
            }
            return isUpdated;
        }

        private void DeleteItineraryLandmarks(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string deleteSQL = $"delete from landmark_itinerary where itineraryId = @id";
                    SqlCommand deleteCmd = new SqlCommand(deleteSQL, conn);
                    deleteCmd.Parameters.AddWithValue("@id", id);
                    deleteCmd.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Could not delete.");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public Itinerary ReadItinerary(SqlDataReader reader)
        {
            Itinerary itinerary = new Itinerary();

            itinerary.Id = Convert.ToInt32(reader["id"]);
            itinerary.Name = Convert.ToString(reader["name"]);
            itinerary.StartLat = Convert.ToDouble(reader["startingLatitude"]);
            itinerary.OwnerID = Convert.ToInt32(reader["ownerID"]);
            itinerary.StartLng = Convert.ToDouble(reader["startingLongitude"]);

            return itinerary;
        }
    }
}
