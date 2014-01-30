using Socialnet.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Socialnet.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new ProfileViewModel()
                {
                    PostComment = new Comment(),
                    UserFeeds = unitOfWork.NewsFeedEngine.GetNewsFeedFor(User.Identity.Name),
                    UserProfile = unitOfWork.Context.Profiles.FirstOrDefault(u => u.Username == User.Identity.Name),
                    StatusMessage = new StatusMessage() { Username = User.Identity.Name },
                    MyFriendRequests = unitOfWork.Context.FriendRequests.Where(m => m.WantsFriend == User.Identity.Name).ToList(),
                };

                ViewData["username"] = User.Identity.Name;
                ViewData["displayName"] = unitOfWork.Context.GetUserDisplayName(User.Identity.Name);
                return View(model);
            }
            else
            // FormsAuthentication.SignOut();
            {
                return View("Login", new LoginViewModel());
            }
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

            unitOfWork.Context.Comments.Add(CommentEntity);
            int rowChanged = unitOfWork.Context.SaveChanges();
            if (rowChanged == 1)
            {
                return Json(new { commentText = CommentEntity.Text, username = CommentEntity.Username });
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
                unitOfWork.Context.Shares.Add(StatusEntry.StatusMessage);
                unitOfWork.Context.SaveChanges();
                return Json(new { commentText = StatusEntry.StatusMessage.Status, username = User.Identity.Name });
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. Status can not be posted right now");
                return Json(new { status = 0 });
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Friends()
        {
            var users = unitOfWork.Context.Friends.Where(u => u.Username == User.Identity.Name).Select(u => u.FriendName).ToList();

            var userFriends = unitOfWork.Context.Profiles.Where(u => users.Contains(u.Username)).ToList();
            return View(userFriends);
        }

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(new LoginViewModel());
            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (isValidUser(user.Username, user.Password))
                {
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
            return RedirectToAction("Index", "Home");
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
                    RegistrationDate = DateTime.Now
                }
            });
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel RegisterEntity)
        {
            RegisterEntity.UserProfile.Username = RegisterEntity.UserDetails.Username;
            RegisterEntity.UserDetails.RegistrationDate = DateTime.Now;
            RegisterEntity.UserProfile.ProfilePicture = "";

            //  return Json(new { formErrors = RegisterEntity }, JsonRequestBehavior.AllowGet);

            if (ModelState.IsValid)
            {
                RegisterEntity.UserProfile.Username = RegisterEntity.UserDetails.Username;

                unitOfWork.Context.Users.Add(RegisterEntity.UserDetails);
                unitOfWork.Context.Profiles.Add(RegisterEntity.UserProfile);

                unitOfWork.Context.SaveChanges();

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
            var user = unitOfWork.Context.Users.FirstOrDefault(m => m.Username == Username && m.Password == Password);
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}