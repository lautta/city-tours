using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApi.DAL;
using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApi.Tests.DAL
{
    [TestClass]
    public class ItineraryDAOTests : ParentTest
    {
        [TestMethod]
        public void Test_GetItinerary()
        {
            ItinerarySqlDAO testDAO = new ItinerarySqlDAO(connectionString);
            int testId = AddTestItinerary();

            testDAO.GetItinerary(testId);

            Assert.AreEqual("test", testDAO.GetItinerary(testId).Name);
            Assert.AreEqual(1, testDAO.GetItinerary(testId).StartLat);
            Assert.AreEqual(1, testDAO.GetItinerary(testId).StartLng);
            Assert.AreEqual(1, testDAO.GetItinerary(testId).OwnerID);
        }

        [TestMethod]
        public void Test_CreateItinerary()
        {
            ItinerarySqlDAO testItineraryDAO = new ItinerarySqlDAO(connectionString);
            LandmarkSqlDAO testLandmarkDAO = new LandmarkSqlDAO(connectionString);
            Itinerary itinerary = new Itinerary();
            int landmarkId = AddTestLandmark();
            Landmark landmark = testLandmarkDAO.GetLandmark(landmarkId);
            

            itinerary.Name = "anything";
            itinerary.StartLng = 98;
            itinerary.StartLat = -83;
            itinerary.OwnerID = 1;
            itinerary.Landmarks.Add(landmark);
            itinerary.LandmarkIDs = new int[] { landmarkId };

            Assert.AreEqual(true, testItineraryDAO.CreateItinerary(itinerary));
        }

        [TestMethod]
        public void Test_DeleteItinerary()
        {
            ItinerarySqlDAO testDAO = new ItinerarySqlDAO(connectionString);
            int testItineraryId = AddTestItinerary();

            Assert.AreEqual(true, testDAO.DeleteItinerary(testItineraryId));
        }

        [TestMethod]
        public void Test_UpdateItinerary()
        {
            ItinerarySqlDAO testDAO = new ItinerarySqlDAO(connectionString);
            LandmarkSqlDAO testLandmarkDAO = new LandmarkSqlDAO(connectionString);
            Itinerary itinerary = new Itinerary();
            int landmarkId = AddTestLandmark();
            Landmark landmark = testLandmarkDAO.GetLandmark(landmarkId);

            itinerary.Name = "anything";
            itinerary.StartLng = 98;
            itinerary.StartLat = -83;
            itinerary.OwnerID = 1;
            itinerary.Landmarks.Add(landmark);
            itinerary.LandmarkIDs = new int[] { landmarkId };


            Assert.AreEqual(true, testDAO.UpdateItinerary(itinerary));
        }
        
    }
}
