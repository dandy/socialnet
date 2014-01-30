using System;
using System.Collections.Generic;
using System.Linq;

namespace Socialnet.Models.Repositories
{
    public class NewsFeedEngine
    {
        private String Username;
        private SocialContext _db;

        public NewsFeedEngine(SocialContext c)
        {
            _db = c;
        }

        public List<NewsFeed> GetPostByUser(String Username)
        {

            var UserShares = _db.Shares.
                                  Where(u=>u.Username == Username).
                                  OrderByDescending(u => u.DatePosted).
                                  ToList();

            List<NewsFeed> UserPostedItems = new List<NewsFeed>();

            foreach (Share p in UserShares)
            {
                var dbQuery = _db.Profiles.FirstOrDefault(u => u.Username == p.Username);

                NewsFeed story = new NewsFeed()
                {
                    FeedShareItem = p,
                    UserDisplayName = dbQuery.FirstName + " " + dbQuery.LastName,
                    UserDisplayPicture = dbQuery.ProfilePicture
                };

                UserPostedItems.Add(story);
            }

            List<NewsFeed> sortedNews = UserPostedItems.OrderByDescending(u => u.FeedShareItem.DatePosted).ToList();

            return sortedNews;

        }

        public List<NewsFeed> GetNewsFeedFor(String Username)
        {
            var userFriends = _db.Friends.Where(u => u.Username == Username).Select(u => u.FriendName).ToList();

            var AllFriendShares = _db.Shares.
                                  Where(u => userFriends.Contains(u.Username)).
                                  OrderByDescending(u => u.DatePosted).
                                  ToList();

            List<NewsFeed> UserNewsFeed = new List<NewsFeed>();

            foreach (Share p in AllFriendShares)
            {
                var dbQuery = _db.Profiles.FirstOrDefault(u => u.Username == p.Username);

                NewsFeed story = new NewsFeed()
                {
                    FeedShareItem = p,
                    UserDisplayName = dbQuery.FirstName + " " + dbQuery.LastName,
                    UserDisplayPicture = dbQuery.ProfilePicture
                };

                UserNewsFeed.Add(story);
            }

            List<NewsFeed> sortedNews = UserNewsFeed.OrderByDescending(u => u.FeedShareItem.DatePosted).ToList();

            return sortedNews;
        }
    }
}