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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Friend> Friends { get; set; }
    }

    public class Friend
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public DateTime FriendshipDate { get; set; }

    }

   
}