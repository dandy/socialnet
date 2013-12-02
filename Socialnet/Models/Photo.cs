using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String PhotoeLocation { get; set; }

        public virtual Profile Profile { get; set; }
    }
}