using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public abstract class Share
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ShareId { get; set; }
        public String Username { get; set; }
        public DateTime DatePosted { get; set; }

        public virtual List<Comment> Comments { get; set; }
    }
    public class StatusMessage : Share
    {
        public String Status { get; set; }
    }

    public class Photo : Share
    {
        public String Location { get; set; }
    }

    public class Comment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public String Username { get; set; }
        public int PostId { get; set; }
        public String Text { get; set; }
        public DateTime DatePosted { get; set; }

        public virtual int ShareId { get; set; }
    }

   
}