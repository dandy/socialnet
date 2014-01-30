using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public class User
    {
        [Key]
        [Display(Prompt="Username")]
        public String Username { get; set; }
        [Required]
        [Display(Prompt = "Password")]
        public String Password { get; set; }
        [Required]
        [Display(Prompt = "Email")]
        public String Email { get; set; }
        public DateTime RegistrationDate { get; set; }

    }

    public class Friend
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FriendId { get; set; }
        public String Username { get; set; }
        public String FriendName { get; set; }
        public DateTime FriendshipDate { get; set; }

    }

    public class FriendRequest
    {
        [Key]
        public int RequestId { get; set; }
        public String Username { get; set; }
        public String WantsFriend { get; set; }
        public int IsProcessed { get; set; }
    }

   
}