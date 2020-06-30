using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Models
{
    public class Image
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
