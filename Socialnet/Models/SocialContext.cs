using System;
using System.Data.Entity;
using System.Linq;

namespace Socialnet.Models
{
    public class Seeder : DropCreateDatabaseIfModelChanges<SocialContext>
    {
        protected override void Seed(SocialContext context)
        {
            base.Seed(context);
            User user = new User()
            {
                Username = "ijlal",
                Password = "ijlal",
                Email = "ijlal2@gmail.com",
                RegistrationDate = DateTime.Now
            };

            Profile userProfile = new Profile(){
                Username = "ijlal",
                FirstName = "M. Ijlal",
                LastName = "Hasnain",
                ProfilePicture = ""
            };

            context.Profiles.Add(userProfile);
            

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
    public class SocialContext : DbContext
    {
        public SocialContext()
            : base("SocialNetConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Share> Shares { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public int CurrentUser { get; set; }

        public void GetUserId(String UserName)
        {
        }

        public Boolean IsFriendWith(String user, String FriendWith)
        {
            Boolean checkIfFriend = Friends.Any(u => u.Username == user && u.FriendName == FriendWith);
            return checkIfFriend;
        }

        internal string GetUserDisplayName(string Username)
        {
            var User = Profiles.FirstOrDefault(u => u.Username == Username);
            return User.FirstName + " " + User.LastName;
            throw new NotImplementedException();
        }
    }
}