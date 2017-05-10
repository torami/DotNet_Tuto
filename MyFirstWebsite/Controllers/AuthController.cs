using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstWebsite.Models;
using System.Security.Claims;

namespace MyFirstWebsite.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
        
            return View();
        }
        [HttpPost]
        public ActionResult Login (Users model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Email == "admin@admin.com" && model.Password == "12345")
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,"Rami"),
                    new Claim(ClaimTypes.Email,"Rami@email.com"),
                    new Claim(ClaimTypes.Country,"France") }, "ApplicationCookie"
                );
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
                return RedirectToAction("Index","Home");
            }
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Auth");
        }
        public ActionResult Registration()
        {
            return View();
        }

    }
}