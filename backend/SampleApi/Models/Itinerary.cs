using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Models
{
    public class Itinerary
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double StartLat { get; set; }
        [Required]
        public double StartLng { get; set; }
        [Required]
        public int[] LandmarkIDs { get; set; }
        [Required]
        public int OwnerID { get; set; }
        public List<Landmark> Landmarks { get; set; }
        
        public Itinerary()
        {
            Landmarks = new List<Landmark>();
        }
    }
}
