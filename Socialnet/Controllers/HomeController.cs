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
            if (Request.IsAuthenticated)
                return View("Index");
            else
                return View("Login",new User());
        }

        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                if (isValidUser(user.Username, user.Password))
                {
                    _db.SetCurrentUserId(user.Username);
                    FormsAuthentication.SetAuthCookie(user.Username, true);
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Login details are not okay");
                }
                
            }

            return View("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
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
