using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApi.DAL;
using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApi.Tests.DAL
{
    [TestClass]
    public class ItineraryReviewDAOTests : ParentTest
    {
        [TestMethod]
        public void Test_GetAllReviews()
        {
            ItineraryReviewSqlDAO testDAO = new ItineraryReviewSqlDAO(connectionString);
            int testItineraryId = AddTestItinerary();
            int startingReviewCount = testDAO.GetAllReviews(testItineraryId).Count;
            AddTestItineraryReview();

            Assert.AreEqual(startingReviewCount + 1, testDAO.GetAllReviews(testItineraryId).Count);
        }

        [TestMethod]
        public void Test_GetReview()
        {
            ItineraryReviewSqlDAO testDAO = new ItineraryReviewSqlDAO(connectionString);
            int testItineraryId = AddTestItinerary();
            int testUserId = AddTestUser();
            int testReviewId = AddTestItineraryReview();

            Assert.AreEqual((decimal)(2.5), testDAO.GetReview(testReviewId).Rating);
            Assert.AreEqual("test review title", testDAO.GetReview(testReviewId).Title);
            Assert.AreEqual("test review body", testDAO.GetReview(testReviewId).Detail);
            Assert.AreEqual(testItineraryId, testDAO.GetReview(testReviewId).TypeId);
            Assert.AreEqual(testUserId, testDAO.GetReview(testReviewId).UserId);
        }

        [TestMethod]
        public void Test_CreateReview()
        {
            ItineraryReviewSqlDAO testDAO = new ItineraryReviewSqlDAO(connectionString);
            Review testReview = new Review();

            int testUserId = AddTestUser();
            int testItineraryId = AddTestItinerary();

            testReview.Rating = 2;
            testReview.Title = "anything";
            testReview.Detail = "anything";
            testReview.UserName = "anyUsername";
            testReview.UserId = testUserId;
            testReview.TypeId = testItineraryId;

            Assert.AreEqual(true, testDAO.CreateReview(testReview));
        }

        [TestMethod]
        public void Test_DeleteReview()
        {
            ItineraryReviewSqlDAO testDAO = new ItineraryReviewSqlDAO(connectionString);
            int testReviewId = AddTestItineraryReview();
            Review review = testDAO.GetReview(testReviewId);

            Assert.AreEqual(true, testDAO.DeleteReview(testReviewId));
        }

        [TestMethod]
        public void Test_UpdateReview()
        {
            ItineraryReviewSqlDAO testDAO = new ItineraryReviewSqlDAO(connectionString);
            Review testReview = new Review();
            int reviewToUpdate = AddTestItineraryReview();
            

            int testUserId = AddTestUser();
            int testItineraryId = AddTestItinerary();

            testReview.Id = reviewToUpdate;
            testReview.Rating = 2;
            testReview.Title = "anything";
            testReview.Detail = "anything";
            testReview.UserName = "anyUsername";
            testReview.UserId = testUserId;
            testReview.TypeId = testItineraryId;

            Assert.AreEqual(true, testDAO.UpdateReview(testReview));
        }
    }
}
