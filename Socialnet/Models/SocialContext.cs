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
            : base("SocialNetConnection")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<StatusMessage> StatusMessages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        public int CurrentUser { get; set; }
        public void GetUserId(String UserName)
        {
           

        }

        internal void SetCurrentUserId(string p)
        {
            var User = Users.FirstOrDefault(u => u.Username == p);
            CurrentUser = User.UserId;
        }

        public Boolean IsFriendWith(String user, String FriendWith)
        {
            Boolean checkIfFriend = Friends.Any(u => u.Username == user && u.FriendName == FriendWith);
            return checkIfFriend;
        }


    }
}