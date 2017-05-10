using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstWebsite.Models;

namespace MyFirstWebsite.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login(string retrunUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = retrunUrl
            };

            return View();
        }
    }
}