using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }
        public string Username { get; set; }
        [Display(Name="First name")]
        public String FirstName { get; set; }
        [Display(Name="Last name")]
        public String LastName { get; set; }
        [Display(Name="Profile picture")]
        public String ProfilePicture { get; set; }

    }

   
   

   

}