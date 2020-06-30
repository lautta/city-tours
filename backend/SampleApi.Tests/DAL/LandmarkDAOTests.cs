using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApi.DAL;
using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApi.Tests.DAL
{

    [TestClass]
    public class LandmarkDAOTests :ParentTest
    {

        [TestMethod]
        public void Test_GetLandmark()
        {
            LandmarkSqlDAO testDAO = new LandmarkSqlDAO(connectionString);
            int testId = AddTestLandmark();

            Assert.AreEqual("testLongName", testDAO.GetLandmark(testId).LongName);
            Assert.AreEqual("Main St", testDAO.GetLandmark(testId).StreetName);
            Assert.AreEqual(30, testDAO.GetLandmark(testId).Latitude);
            Assert.AreEqual(-80, testDAO.GetLandmark(testId).Longitude);
        }

        [TestMethod]
        public void Test_AddLandmark()
        {
            LandmarkSqlDAO testDAO = new LandmarkSqlDAO(connectionString);

            Landmark testLandmark = new Landmark();

            testLandmark.LongName = "Anything";
            testLandmark.ShortName = "Anything";
            testLandmark.Latitude = 10;
            testLandmark.Longitude = -20;
            testLandmark.City = "Here";
            testLandmark.State = "Ohio";
            testLandmark.StateCode = "OH";
            testLandmark.Details = "details";
            testLandmark.StreetNumber = 1;
            testLandmark.StreetName = "Street";
            testLandmark.IsApproved = true;

            Assert.AreEqual(true, testDAO.AddLandmark(testLandmark));
    }

        [TestMethod]
        public void Test_GetAllLandmarks()
        {
            LandmarkSqlDAO testDAO = new LandmarkSqlDAO(connectionString);

            int startingCount = testDAO.GetAllLandmarks().Count;
            int newTestLandmarkId = AddTestLandmark();

            Assert.AreEqual(startingCount + 1, testDAO.GetAllLandmarks().Count);
        }

        [TestMethod]
        public void Test_UpdateLandmark()
        {
            LandmarkSqlDAO testDAO = new LandmarkSqlDAO(connectionString);
            int landmarkToUpdate = AddTestLandmark();
            Landmark testLandmark = new Landmark();

            testLandmark.Id = landmarkToUpdate;
            testLandmark.LongName = "Anything";
            testLandmark.ShortName = "Anything";
            testLandmark.Latitude = 10;
            testLandmark.Longitude = -20;
            testLandmark.City = "Here";
            testLandmark.State = "Ohio";
            testLandmark.StateCode = "OH";
            testLandmark.Details = "details";
            testLandmark.StreetNumber = 1;
            testLandmark.StreetName = "Street";
            testLandmark.IsApproved = true;

            Assert.AreEqual(true, testDAO.UpdateLandmark(testLandmark));
        }

        [TestMethod]
        public void Test_GetUnapprovedLandmarks()
        {
            LandmarkSqlDAO testDAO = new LandmarkSqlDAO(connectionString);

            int startingCount = testDAO.GetUnapprovedLandmarks().Count;
            int unapprovedLandmarkId = AddTestLandmark();

            Assert.AreEqual(startingCount + 1, testDAO.GetUnapprovedLandmarks().Count);
        }

        [TestMethod]
        public void Test_GetApprovedLandmarks()
        {
            LandmarkSqlDAO testDAO = new LandmarkSqlDAO(connectionString);

            int startingCount = testDAO.GetApprovedLandmarks().Count;
            int unapprovedLandmarkId = AddTestLandmark();

            Landmark unapprovedLandmark = testDAO.GetLandmark(unapprovedLandmarkId);
            Assert.AreEqual(startingCount, testDAO.GetApprovedLandmarks().Count);

            unapprovedLandmark.IsApproved = true;
            testDAO.UpdateLandmark(unapprovedLandmark);
            Assert.AreEqual(startingCount + 1, testDAO.GetApprovedLandmarks().Count);

        }
    }
}
