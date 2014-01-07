using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{
    public class StatusMessage
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int StatusId { get; set; }
        public String Username { get; set; }
        public String Status { get; set; }
        public DateTime DatePosted { get; set; }
    }

    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public String Username { get; set; }
        public int PostId { get; set; }
        public String Text { get; set; }
        public DateTime DatePosted { get; set; }
    }

    public class NewsFeed
    {
        public String UserDisplayName { get; set; }
        public String UserDisplayPicture { get; set; }
        public String StoryType { get; set; }
        public String FeedEntry { get; set; }
        public DateTime PostedOn { get; set; }
        public int PostId { get; set; }

        public List<Comment> StoryComments { get; set; }

    }
}