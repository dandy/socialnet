using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Socialnet.Models;
using Socialnet.Helpers;
namespace Socialnet.Controllers
{
    public class HomeController : Controller
    {
        SocialContext _db = new SocialContext();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {

                var userFriends = _db.Friends.Where(u => u.Username == User.Identity.Name).Select(u => u.FriendName).ToList();

                var AllFriendsPhotos = _db.Photos.Where(u => userFriends.Contains(u.Username)).ToList();
                var AllFriendStatusMessages = _db.StatusMessages.Where(u => userFriends.Contains(u.Username)).ToList();

                Console.WriteLine(AllFriendsPhotos);
                System.Diagnostics.Debug.WriteLine(AllFriendsPhotos);

                List<Photo> AllFriendPhotos = AllFriendsPhotos.OrderByDescending(u => u.DatePosted).ToList();
                List<StatusMessage> AllStatusMessages = AllFriendStatusMessages.OrderByDescending(u => u.DatePosted).ToList();

                List<NewsFeed> UserNewsFeed = new List<NewsFeed>();

                foreach (Photo p in AllFriendPhotos)
                {
                    var dbQuery = _db.Profiles.FirstOrDefault(u => u.Username == p.Username);

                    NewsFeed story = new NewsFeed()
                    {
                        StoryComments = _db.Comments.Where(m=>m.PostId==p.PhotoId).ToList(),
                        PostedOn = p.DatePosted,
                        StoryType = "Photo",
                        FeedEntry = p.Location,
                        UserDisplayName = dbQuery.FirstName +" "+ dbQuery.LastName,
                        UserDisplayPicture = dbQuery.ProfilePicture,
                        PostId = p.PhotoId    
                    };

                    UserNewsFeed.Add(story);
                }

                foreach (StatusMessage p in AllStatusMessages)
                {
                    var dbQuery = _db.Profiles.FirstOrDefault(u => u.Username == p.Username);

                    NewsFeed story = new NewsFeed()
                    {
                        StoryComments = _db.Comments.Where(m=>m.PostId==p.StatusId).ToList(),
                        PostedOn = p.DatePosted,
                        StoryType = "status",
                        FeedEntry = p.Status,
                        PostId = p.StatusId,
                        
                        
                        UserDisplayName = dbQuery.FirstName +" "+ dbQuery.LastName,
                        UserDisplayPicture = dbQuery.ProfilePicture

                    };

                    UserNewsFeed.Add(story);
                }

                List<NewsFeed> sortedNews = UserNewsFeed.OrderByDescending(u => u.PostedOn).ToList();
                var model = new ProfileViewModel()
                {
                    PostComment = new Comment(){

                    },
                    UserFeeds = UserNewsFeed,
                    UserProfile = _db.Profiles.FirstOrDefault(u=>u.Username==User.Identity.Name),
                    UserPhotos = _db.Photos.Where(u => u.Username == User.Identity.Name).ToList(),
                    StatusMessage = new StatusMessage() { Username = User.Identity.Name },
                    UserStatusMessages = _db.StatusMessages.Where(m => m.Username == User.Identity.Name).ToList(),
                    MyFriendRequests = _db.FriendRequests.Where(m => m.WantsFriend  == User.Identity.Name).ToList(),
                    
                };

                ViewData["username"] = User.Identity.Name;
                ViewData["myname"] = "ijlal";
                ViewBag.username = User.Identity.Name;
                return View(model);

            }
            else
                return View("Login", new LoginViewModel());
        }

        [HttpPost]
        public ActionResult PostComment(ProfileViewModel c)
        {
            var CommentEntity = new Comment()
            {
                Username = User.Identity.Name,
                PostId = c.PostComment.PostId,
                DatePosted = DateTime.Now,
                Text = c.PostComment.Text
            };
           
            _db.Comments.Add(CommentEntity);
            int rowChanged = _db.SaveChanges();
            if (rowChanged == 1)
            {
                return Json(new { commentText = CommentEntity.Text, username=CommentEntity.Username });
            }
            else
            {
                return Content("No comment posted");
            }
        }

        public ActionResult PostStatus(ProfileViewModel StatusEntry)
        {
            if (ModelState.IsValid)
            {
                StatusEntry.StatusMessage.DatePosted = DateTime.Now;
                _db.StatusMessages.Add(StatusEntry.StatusMessage);
                _db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. Status can not be posted right now");

            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Friends()
        {
            var users = _db.Friends.Where(u => u.Username == User.Identity.Name).Select(u => u.FriendName).ToList();

            var userFriends = _db.Profiles.Where(u => users.Contains(u.Username)).ToList();
            return View(userFriends);
        }

        public ActionResult Login()
        {
           

            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (isValidUser(user.Username, user.Password))
                {
                    _db.SetCurrentUserId(user.Username);
                    FormsAuthentication.SetAuthCookie(user.Username, true);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username/password invalid");
                }


            }
            else
            {
                ModelState.AddModelError("", "Model state is not valid");
            }

            return View(new LoginViewModel());
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        
        }

        public ActionResult Register()
        {

            return View(new RegisterViewModel()
            {

                UserProfile = new Profile()
                {
                    ProfilePicture = "",

                },
                UserDetails = new User()
                {
                    Date = DateTime.Now
                }


            });

        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel RegisterEntity)
        {
            RegisterEntity.UserProfile.Username = RegisterEntity.UserDetails.Username;
            RegisterEntity.UserDetails.Date = DateTime.Now;
            RegisterEntity.UserProfile.ProfilePicture = "";

          //  return Json(new { formErrors = RegisterEntity }, JsonRequestBehavior.AllowGet);

            if (ModelState.IsValid)
            {

                RegisterEntity.UserProfile.Username = RegisterEntity.UserDetails.Username;


                _db.Users.Add(RegisterEntity.UserDetails);
                _db.Profiles.Add(RegisterEntity.UserProfile);

                _db.SaveChanges();

                return Content("Your data has been saved successfully");

            }
            else
            {
               
                //return Content("No data has been saved successfully");

                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { formErrors = allErrors }, JsonRequestBehavior.AllowGet);

            }

        }

        public bool isValidUser(String Username, String Password)
        {
            var user = _db.Users.FirstOrDefault(m=>m.Username==Username && m.Password==Password);
            if (user != null )
            {
                return true;
            }
            return false;
        }

       

    }
}
