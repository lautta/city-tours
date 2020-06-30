using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public decimal Rating { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
    }
}
