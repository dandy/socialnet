using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String ProfilePicture { get; set; }

        public virtual User User { get; set; }
        public virtual List<Photo> Photos { get; set; }
    }

    public class ProfileViewModel
    {
        public Profile UserProfile { get; set; }
        public List<Photo>
            UserPhotos { get; set; }

    }

    public class RegisterViewModel
    {
        public String Username { get; set; }
        public String Email { get; set; }
        public int MyProperty { get; set; }
    }

}