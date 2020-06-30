using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Models
{
    public class Landmark
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string LongName { get; set; }
        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        [Required]
        [StringLength(2000)]
        public string Details { get; set; }
        public int StreetNumber { get; set; }
        public int Zip { get; set; }
        public string StreetName { get; set; }
        public bool IsApproved { get; set; }
        public string[] ImageNames { get; set; }
    }
}
