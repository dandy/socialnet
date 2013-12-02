using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public class SocialContext : DbContext
    {
        public SocialContext()
            : base("DefaultConnection")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}