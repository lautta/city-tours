using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApi.DAL;
using SampleApi.Models;

namespace SampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private IItineraryDAO _itineraryDAO;

        public ItineraryController(IItineraryDAO itineraryDAO)
        {
            _itineraryDAO = itineraryDAO;
        }

        [HttpGet("{id}", Name = "GetItinerary")]
        public ActionResult<Itinerary> GetItinerary(int id)
        {
            Itinerary itinerary = _itineraryDAO.GetItinerary(id);

            if (itinerary != null)
            {
                return itinerary;
            }

            return NotFound();
        }

        [HttpGet]
        public ActionResult<List<Itinerary>> GetAllItineraries()
        {
            List<Itinerary> itineraries = _itineraryDAO.GetAllItinerary();
            return itineraries;
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateItinerary(Itinerary itinerary)
        {
            string user = User.Identity.Name;
            _itineraryDAO.CreateItinerary(itinerary);

            return CreatedAtRoute("GetItinerary", new { id = itinerary.Id }, itinerary);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            Itinerary itinerary = _itineraryDAO.GetItinerary(id);

            string userId = User.Claims.Where(c => c.Type == "id").FirstOrDefault()?.Value;

            if (itinerary == null)
            {
                return NotFound();
            }

            if (int.Parse(userId) != itinerary.OwnerID)
            {
                return Unauthorized();
            }

            _itineraryDAO.DeleteItinerary(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult UpdateItinerary(int id, Itinerary updatedItenerary)
        {
            Itinerary existingItinerary = _itineraryDAO.GetItinerary(id);

            string userId = User.Claims.Where(c => c.Type == "id").FirstOrDefault()?.Value;

            if (existingItinerary == null)
            {
                return NotFound();
            }

            if (int.Parse(userId) != existingItinerary.OwnerID)
            {
                return Unauthorized();
            }

            existingItinerary.Name = updatedItenerary.Name;
            existingItinerary.StartLat = updatedItenerary.StartLat;
            existingItinerary.StartLng = updatedItenerary.StartLng;
            existingItinerary.LandmarkIDs = updatedItenerary.LandmarkIDs;

            _itineraryDAO.UpdateItinerary(existingItinerary);

            return NoContent();
        }
    }
}