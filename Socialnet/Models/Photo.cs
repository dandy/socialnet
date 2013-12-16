using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string   Username { get; set; }
        public String Location { get; set; }
        public DateTime DateAdded { get; set; }

    }
}