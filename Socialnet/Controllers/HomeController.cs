using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Socialnet.Models;

namespace Socialnet.Controllers
{
    public class HomeController : Controller
    {
        SocialContext _db = new SocialContext();
        public ActionResult Index()
        {
           return View(new User());
        }

        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {

                FormsAuthentication.SetAuthCookie("username", true);
                return RedirectToAction("Index", "Profile");
            }

            return Content("No user found");
        }


        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                User registerEntry = new User()
                {
                    Username = user.Username,
                    Password = user.Password,
                    Email = user.Email,
                    Date = DateTime.Now
                };

                _db.Users.Add(registerEntry);
                _db.SaveChanges();

                return Content("Your data has been saved successfully"); 

            }
            return View();
        }

       

    }
}
