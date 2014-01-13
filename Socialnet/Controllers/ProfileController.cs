using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Socialnet.Models;
using System.Data.Entity;

namespace Socialnet.Controllers
{
    public class ProfileController : Controller
    {
        SocialContext _db = new SocialContext();
        [Authorize]
        public ActionResult Index()
        {
            ProfileViewModel model = new ProfileViewModel()
            {
                UserPhotos = _db.Photos.Where(u => u.Username == User.Identity.Name).ToList()
            };
            return View(model);
        }

        public ActionResult ShowUserProfile(string username)
        {
            ViewData["name"] = username;
            ProfileViewModel model = new ProfileViewModel()
            {
                UserProfile = _db.Profiles.FirstOrDefault(u=>u.Username==username),
                UserPhotos = _db.Photos.Where(u => u.Username == username).ToList(),
                UserStatusMessages = _db.StatusMessages.Where(m => m.Username == username).ToList(),
            };
            return View(model);

        }

        [HttpPost]
        public ActionResult AddFriend(string newFriendId)
        {
            if (!_db.IsFriendWith(User.Identity.Name, newFriendId))
            {
                var model = new FriendRequest()
                {
                    Username = User.Identity.Name,
                    WantsFriend = newFriendId,
                    IsProcessed = 0

                };
                _db.FriendRequests.Add(model);
                int status = _db.SaveChanges();

                return Json(new { d = status }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { d = 2 });
            
        }

        public ActionResult EditProfile(){
            var profileModel = _db.Profiles.FirstOrDefault(u => u.Username == User.Identity.Name);
            return View(profileModel);
        }
        [HttpPost]
        public ActionResult EditProfile(Profile profile, HttpPostedFileBase ProfilePic, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                profile.Username = User.Identity.Name;

                if (ProfilePic != null)
                {
                    string pic = System.IO.Path.GetFileName(ProfilePic.FileName);

                    string path = System.IO.Path.Combine(
                    Server.MapPath("~/images"), pic);
                    // file is uploaded
                    ProfilePic.SaveAs(path);

                    profile.ProfilePicture = pic;

                }
                else
                {
                    profile.ProfilePicture = frm["CurrentProfilePic"];
                }
                _db.Entry(profile).State = EntityState.Modified;                
                _db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Errors found in the model");
            }

            // save the image path path to the database or you can send image 
            // directly to database
            // in-case if you want to store byte[] ie. for DB

            return View(profile);
        }

        public ActionResult MakeFriends(String user, String Friend)
        {
            _db.Friends.Add(new Friend() { 
                Username = user,
                FriendName = Friend,
                FriendshipDate = DateTime.Now
            });

            _db.Friends.Add(new Friend()
            {
                Username = Friend,
                FriendName = user,
                FriendshipDate = DateTime.Now
            });


             int i = _db.SaveChanges();

             if (i == 0)
             {
                 ViewData["success"] = "No friends";
             }
             else
             {
                 ViewData["success"] = "Made friends";
                 var deleteRow = _db.FriendRequests.FirstOrDefault(u => u.Username == user && u.WantsFriend == Friend);
                 _db.FriendRequests.Remove(deleteRow);
                 _db.SaveChanges();
             }

            return View();
        }

        public ActionResult Friends()
        {
            var userEntitys = _db.Friends.Where(m => m.Username == User.Identity.Name).ToList();
            return View(userEntitys);
        }

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                Server.MapPath("~/images"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                var photoEntry = new Photo()
                {

                    Username = User.Identity.Name,
                    Location = pic,
                    DatePosted = DateTime.Now
                };

                _db.Photos.Add(photoEntry);
                _db.SaveChanges();

            }
            // after successfully uploading redirect the user
            return RedirectToAction("ShowUserProfile", "Profile", new { username = User.Identity.Name });
        }
    }

}
