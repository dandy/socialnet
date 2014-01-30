using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socialnet.Hubs
{
    public class CommentDivData
    {
        public string commentText { get; set; }
        public string username { get; set; }
    }
    [HubName("commentshub")]
    public class CommentHub : Hub
    {
        public void SendCommentNotification(CommentDivData result, dynamic resultDiv   )
        {
            Clients.All.sendnotification(result, resultDiv);
        }

        public void AddStatusServer(CommentDivData result)
        {
            Clients.All.addstatusclient(result);
        }
    }
}