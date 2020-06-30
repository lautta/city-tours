using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApi.DAL;
using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApi.Tests.DAL
{
    [TestClass]
    public class UserDAOTests : ParentTest
    {
        [TestMethod]
        public void Test_GetUser()
        {
            UserSqlDAO testDAO = new UserSqlDAO(connectionString);
            int newUserId = AddTestUser();

            Assert.AreEqual("usernameTest", testDAO.GetUser("usernameTest").Username);
            Assert.AreEqual("passwordTest", testDAO.GetUser("usernameTest").Password);
            Assert.AreEqual("roleTest", testDAO.GetUser("usernameTest").Role);
            Assert.AreEqual("saltTest", testDAO.GetUser("usernameTest").Salt);
        }

        [TestMethod]
        public void Test_CreateUser()
        {
            UserSqlDAO testDAO = new UserSqlDAO(connectionString);
            User testUser = new User();

            testUser.Password = "password";
            testUser.Username = "username";
            testUser.Role = "role";
            testUser.Salt = "salt";

            Assert.AreEqual(true, testDAO.CreateUser(testUser));
        }

        [TestMethod]
        public void Test_UpdateUser()
        {
            UserSqlDAO testDAO = new UserSqlDAO(connectionString);
            User testUser = new User();
            int userToUpdate = AddTestUser();

            testUser.Id = userToUpdate;
            testUser.Password = "password";
            testUser.Username = "username";
            testUser.Role = "role";
            testUser.Salt = "salt";

            Assert.AreEqual(true, testDAO.UpdateUser(testUser));
        }

        [TestMethod]
        public void Test_DeleteUser()
        {
            UserSqlDAO testDAO = new UserSqlDAO(connectionString);
            AddTestUser();
            User testUser = testDAO.GetUser("usernameTest");

            Assert.AreEqual(true, testDAO.DeleteUser(testUser));
        }
    }
}
