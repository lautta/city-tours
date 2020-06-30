using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApi.DAL;
using SampleApi.Models;
using System.Drawing;

namespace SampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandmarkController : ControllerBase
    {
        private ILandmarkDAO _landmarkDAO;
        private const string uploadPath = @"../../frontend/public/uploads/";

        public LandmarkController(ILandmarkDAO landmarkDAO)
        {
            _landmarkDAO = landmarkDAO;
        }

        [HttpGet("{id}", Name = "GetLandmark")]
        public ActionResult<Landmark> GetLandmark(int id)
        {
            Landmark landmark = _landmarkDAO.GetLandmark(id);
            

            if (landmark != null)
            {
                landmark.ImageNames = GetImageList(id);
                return Ok(landmark);
            }

            return NotFound();
        }

        private string[] GetImageList(int id)
        {
            List<string> imageNames = new List<string>();

            if(Directory.Exists(uploadPath + id))
            {
                DirectoryInfo d = new DirectoryInfo(uploadPath + id);
                FileInfo[] Files = d.GetFiles(); 
                foreach (FileInfo file in Files)
                {
                    imageNames.Add(file.Name);
                }
            }

            return imageNames.ToArray();
        }

        [HttpPost("approve/{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult ApproveLandmark(int id)
        {
            Landmark existingLandmark = _landmarkDAO.GetLandmark(id);

            if (existingLandmark == null)
            {
                return NotFound();
            }

            existingLandmark.IsApproved = true;
            _landmarkDAO.UpdateLandmark(existingLandmark);

            return NoContent();
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult CreateLandmark([FromBody] Landmark landmark)
        {
            bool isCreated = _landmarkDAO.AddLandmark(landmark);

            return CreatedAtRoute("GetLandmark", new { id = landmark.Id }, landmark);
        }

        [HttpPost("suggest")]
        [Authorize]
        public ActionResult SuggestLandmark([FromBody] Landmark landmark)
        {
            if (landmark.IsApproved)
            {
                return Unauthorized();
            }

            bool isCreated = _landmarkDAO.AddLandmark(landmark);

            return CreatedAtRoute("GetLandmark", new { id = landmark.Id }, landmark);
        }

        [HttpGet]
        public ActionResult<IList<Landmark>> GetAll()
        {
            return Ok(_landmarkDAO.GetAllLandmarks().ToList());
        }

        [HttpGet("unapproved")]
        [Authorize(Roles = "admin")]
        public ActionResult<IList<Landmark>> GetAllUnapproved()
        {
            return Ok(_landmarkDAO.GetUnapprovedLandmarks().ToList());
        }

        [HttpGet("approved")]
        public ActionResult<IList<Landmark>> GetAllApproved()
        {
            return Ok(_landmarkDAO.GetApprovedLandmarks().ToList());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Landmark landmark = _landmarkDAO.GetLandmark(id);

            if (landmark == null)
            {
                return NotFound();
            }

            _landmarkDAO.DeleteLandmark(id);

            return NoContent();
        }

        [HttpPost("{id}/upload")]
        [Authorize]
        public ActionResult Upload(int id, [FromForm]Image image)
        {
            IFormFile file = image.File;

            if (file.FileName == null)
            {
                return BadRequest();
            }

            Directory.CreateDirectory(uploadPath + id);

            if (file.Length > 0)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(uploadPath + id + "/" + fileName, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return Ok(new { status = true, message = fileName });
            }
            return BadRequest();

        }
    }
}