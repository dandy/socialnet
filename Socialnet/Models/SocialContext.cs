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
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public int CurrentUser { get; set; }
        public void GetUserId(String UserName)
        {
           

        }

        internal void SetCurrentUserId(string p)
        {
            var User = Users.FirstOrDefault(u => u.Username == p);
            CurrentUser = User.UserId;
        }
    }
}