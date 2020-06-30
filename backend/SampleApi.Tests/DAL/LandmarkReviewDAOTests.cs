using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApi.DAL;
using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApi.Tests.DAL
{
    [TestClass]
    public class LandmarkReviewDAOTests : ParentTest
    {
        [TestMethod]
        public void Test_GetAllReviews()
        {
            LandmarkReviewSqlDAO testDAO = new LandmarkReviewSqlDAO(connectionString);
            int testLandmarkId = AddTestLandmark();
            int startingReviewCount = testDAO.GetAllReviews(testLandmarkId).Count;
            AddTestLandmarkReview();

            Assert.AreEqual(startingReviewCount + 1, testDAO.GetAllReviews(testLandmarkId).Count);
        }

        [TestMethod]
        public void Test_GetReview()
        {
            LandmarkReviewSqlDAO testDAO = new LandmarkReviewSqlDAO(connectionString);
            int testLandmarkId = AddTestLandmark();
            int testUserId = AddTestUser();
            int testReviewId = AddTestLandmarkReview();

            Review test = testDAO.GetReview(testReviewId);

            Assert.AreEqual((decimal)(1.5), testDAO.GetReview(testReviewId).Rating);
            Assert.AreEqual("test review title", testDAO.GetReview(testReviewId).Title);
            Assert.AreEqual("test review body", testDAO.GetReview(testReviewId).Detail);
            Assert.AreEqual(testLandmarkId, testDAO.GetReview(testReviewId).TypeId);
            Assert.AreEqual(testUserId, testDAO.GetReview(testReviewId).UserId);
        }

        [TestMethod]
        public void Test_CreateReview()
        {
            LandmarkReviewSqlDAO testDAO = new LandmarkReviewSqlDAO(connectionString);
            Review testReview = new Review();

            int testUserId = AddTestUser();
            int testLandmarkId = AddTestLandmark();

            testReview.Rating = 2;
            testReview.Title = "anything";
            testReview.Detail = "anything";
            testReview.UserName = "anyUsername";
            testReview.UserId = testUserId;
            testReview.TypeId = testLandmarkId;

            Assert.AreEqual(true, testDAO.CreateReview(testReview));
        }

        [TestMethod]
        public void Test_DeleteReview()
        {
            LandmarkReviewSqlDAO testDAO = new LandmarkReviewSqlDAO(connectionString);
            int testReviewId = AddTestLandmarkReview();

            Assert.AreEqual(true, testDAO.DeleteReview(testReviewId));
        }

        [TestMethod]
        public void Test_UpdateReview()
        {
            LandmarkReviewSqlDAO testDAO = new LandmarkReviewSqlDAO(connectionString);
            Review testReview = new Review();
            int reviewToUpdate = AddTestLandmarkReview();


            int testUserId = AddTestUser();
            int testLandmarkId = AddTestLandmark();

            testReview.Id = reviewToUpdate;
            testReview.Rating = 2;
            testReview.Title = "anything";
            testReview.Detail = "anything";
            testReview.UserName = "anyUsername";
            testReview.UserId = testUserId;
            testReview.TypeId = testLandmarkId;

            Assert.AreEqual(true, testDAO.UpdateReview(testReview));
        }
    }
}
