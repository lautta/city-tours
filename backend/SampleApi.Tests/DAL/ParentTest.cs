using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace SampleApi.Tests.DAL
{
    public class ParentTest
    {
        private TransactionScope trans;

        protected string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CityTours;Integrated Security=True";

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope();
        }

        [TestCleanup]
        public void Reset()
        {
            trans.Dispose();
        }

        protected int AddTestLandmark()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO landmark (longName, streetNumber,streetName,city,[state], stateCode,zip,latitude,longitude,details,isApproved) output inserted.id " +
                    "VALUES('testLongName', 1, 'Main St', 'Columbus', 'Ohio', 'OH', 43230, 30, -80, 'testDescription', 0)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                return (int)cmd.ExecuteScalar();
            }
        }

        protected int AddTestItinerary()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO itinerary ([name], startingLatitude, startingLongitude, ownerID) output inserted.id " +
                    "VALUES('test', 1, 1, 1)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                return (int)cmd.ExecuteScalar();
            }
        }

        protected int AddTestUser()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO [user](username, password, salt, role) output inserted.id " +
                    "VALUES('usernameTest', 'passwordTest', 'saltTest', 'roleTest')";
                SqlCommand cmd = new SqlCommand(sql, conn);

                return (int)cmd.ExecuteScalar();
            }
        }

        protected int AddTestLandmarkReview()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO landmarkReview(rating, title, body, landmarkId, userId) output inserted.id " +
                    "VALUES(1.5, 'test review title', 'test review body', (select max(id) from landmark), (select max(id) from [user]))";
                SqlCommand cmd = new SqlCommand(sql, conn);

                return (int)cmd.ExecuteScalar();
            }
        }

        protected int AddTestItineraryReview()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO itineraryReview(rating, title, body, itineraryId, userId) output inserted.id " +
                    "VALUES(2.5, 'test review title', 'test review body', (select max(id) from itinerary), (select max(id) from [user]))";
                SqlCommand cmd = new SqlCommand(sql, conn);

                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
