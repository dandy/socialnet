using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Socialnet.Models;

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

        public ActionResult Show(string username)
        {
            ViewData["name"] = username;
            ProfileViewModel model = new ProfileViewModel()
            {
                UserPhotos = _db.Photos.Where(u => u.Username == username).ToList()
            };
            return View("Index",model);

        }

        public ActionResult Friends()
        {
            var userEntitys = _db.Friends.Where(m => m.UserId == _db.CurrentUser).ToList();
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
                    DateAdded = DateTime.Now
                };

                _db.Photos.Add(photoEntry);
                _db.SaveChanges();

            }
            // after successfully uploading redirect the user
            return RedirectToAction("Index", "Profile");
        }
    }

}
