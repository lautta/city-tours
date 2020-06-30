using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApi.DAL;
using SampleApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace SampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private ILandmarkReviewDAO _landmarkReviewDAO;
        private IItineraryReviewDAO _itineraryReviewDAO;

        public ReviewController(ILandmarkReviewDAO landmarkReviewDAO, IItineraryReviewDAO itineraryReviewDAO)
        {
            _landmarkReviewDAO = landmarkReviewDAO;
            _itineraryReviewDAO = itineraryReviewDAO;
        }

        [HttpGet("landmark/all/{id}")]
        public ActionResult<List<Review>> GetAllLandmarkReviews(int id)
        {
            List<Review> reviews = _landmarkReviewDAO.GetAllReviews(id);

            return reviews;
        }

        [HttpPost("landmark")]
        [Authorize]
        public IActionResult CreateLandmarkReview(Review review)
        {
            _landmarkReviewDAO.CreateReview(review);

            return CreatedAtRoute("GetLandmarkReview", new { id = review.Id }, review);
        }

        [HttpPost("landmark/upvote")]
        public IActionResult UpvoteLandmarkReview([FromBody]int id)
        {
            _landmarkReviewDAO.Upvote(id);

            return NoContent();
        }

        [HttpPost("landmark/downvote")]
        public IActionResult DownvoteLandmarkReview([FromBody]int id)
        {
            _landmarkReviewDAO.Downvote(id);

            return NoContent();
        }

        [HttpGet("landmark/{id}", Name = "GetLandmarkReview")]
        public ActionResult<Review> GetLandmarkReview(int id)
        {
            Review review = _landmarkReviewDAO.GetReview(id);

            if (review != null)
            {
                return review;
            }

            return NotFound();
        }

        [HttpPut("landmark/{id}")]
        [Authorize]
        public ActionResult UpdateLandmarkReview(int id, Review updatedReview)
        {
            Review existingReview = _landmarkReviewDAO.GetReview(id);

            string userId = User.Claims.Where(c => c.Type == "id").FirstOrDefault()?.Value;

            if (existingReview == null)
            {
                return NotFound();
            }

            if (int.Parse(userId) != existingReview.UserId)
            {
                return Unauthorized();
            }

            existingReview.Rating = updatedReview.Rating;
            existingReview.Title = updatedReview.Title;
            existingReview.Detail = updatedReview.Detail;

            _landmarkReviewDAO.UpdateReview(existingReview);

            return NoContent();
        }

        [HttpDelete("landmark/{id}")]
        [Authorize]
        public ActionResult DeleteLandmarkReview(int id)
        {
            Review review = _landmarkReviewDAO.GetReview(id);

            string userId = User.Claims.Where(c => c.Type == "id").FirstOrDefault()?.Value;

            if (review == null)
            {
                return NotFound();
            }

            if (int.Parse(userId) != review.UserId)
            {
                return Unauthorized();
            }

            _landmarkReviewDAO.DeleteReview(id);

            return NoContent();
        }

        // Itinerary Review

        [HttpGet("itinerary/all/{id}")]
        public ActionResult<List<Review>> GetAllItineraryReviews(int id)
        {
            List<Review> reviews = _itineraryReviewDAO.GetAllReviews(id);

            return reviews;
        }

        [HttpPost("itinerary/upvote")]
        public IActionResult UpvoteItineraryReview([FromBody]int id)
        {
            _itineraryReviewDAO.Upvote(id);

            return NoContent();
        }

        [HttpPost("itinerary/downvote")]
        public IActionResult DownvoteItineraryReview([FromBody]int id)
        {
            _itineraryReviewDAO.Downvote(id);

            return NoContent();
        }


        [HttpPost("itinerary")]
        [Authorize]
        public IActionResult CreateItineraryReview(Review review)
        {
            _itineraryReviewDAO.CreateReview(review);

            return CreatedAtRoute("GetItineraryReview", new { id = review.Id }, review);
        }

        [HttpGet("itinerary/{id}", Name = "GetItineraryReview")]
        public ActionResult<Review> GetItineraryReview(int id)
        {
            Review review = _itineraryReviewDAO.GetReview(id);

            if (review != null)
            {
                return review;
            }

            return NotFound();
        }

        [HttpPut("itinerary/{id}")]
        [Authorize]
        public ActionResult UpdateItineraryReview(int id, Review updatedReview)
        {
            Review existingReview = _itineraryReviewDAO.GetReview(id);

            string userId = User.Claims.Where(c => c.Type == "id").FirstOrDefault()?.Value;

            if (existingReview == null)
            {
                return NotFound();
            }

            if (int.Parse(userId) != existingReview.UserId)
            {
                return Unauthorized();
            }

            existingReview.Rating = updatedReview.Rating;
            existingReview.Title = updatedReview.Title;
            existingReview.Detail = updatedReview.Detail;

            _itineraryReviewDAO.UpdateReview(existingReview);

            return NoContent();
        }

        [HttpDelete("itinerary/{id}")]
        [Authorize]
        public ActionResult DeleteItineraryReview(int id)
        {
            Review review = _itineraryReviewDAO.GetReview(id);

            string userId = User.Claims.Where(c => c.Type == "id").FirstOrDefault()?.Value;

            if (review == null)
            {
                return NotFound();
            }

            if (int.Parse(userId) != review.UserId)
            {
                return Unauthorized();
            }

            _itineraryReviewDAO.DeleteReview(id);

            return NoContent();
        }
    }
}