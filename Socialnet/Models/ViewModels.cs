﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Socialnet.Models
{

    public class RegisterViewModel
    {
        public User UserDetails { get; set; }
        public Profile UserProfile { get; set; }

    }

    public class ProfileViewModel
    {
        public Profile UserProfile { get; set; }

        public StatusMessage StatusMessage { get; set; }
        public Comment PostComment { get; set; }

        public ICollection<FriendRequest> MyFriendRequests { get; set; }
        public List<NewsFeed> UserFeeds { get; set; }

    }

    public class LoginViewModel
    {
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }
    }

}