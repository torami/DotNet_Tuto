using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstWebsite.Models;
using System.Security.Claims;
using MyFirstWebsite.CustomLibraries;

namespace MyFirstWebsite.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new MainDbContext();
            return View(db.Lists.Where(x => x.Public == "YES").ToList());
        }
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
            using (var db = new MainDbContext())
                {   
                    var emailcheck = db.Users.FirstOrDefault(u => u.Email == model.Email );
                    var getpassword = db.Users.Where(u =>u.Email == model.Email).Select(u => u.Password);
                    var materializePassword = getpassword.ToList();
                    var password = materializePassword[0];
                    var decryptedpassword = CustomDecrypt.Decrypt(password);
                if (model.Email != null && model.Password == decryptedpassword)
                {
                    //fetch the name from database
                    var getName = db.Users.Where(u => u.Email == model.Email).Select(u => u.Name);
                    var materializeName = getName.ToList();
                    var name = materializeName[0];
                    //fetch the country from the database
                    var getCountry = db.Users.Where(u => u.Email == model.Email).Select(u => u.Country);
                    var materializeCountry = getCountry.ToList();
                    var country = materializeCountry[0];
                    //fetch email from the database
                    var getEmail = db.Users.Where(u => u.Email == model.Email).Select(u => u.Email);
                    var materializeEmail = getEmail.ToList();
                    var email = materializeEmail[0];
                    // prepare the cookies session 
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name,name),
                        new Claim(ClaimTypes.Country,country),
                        new Claim(ClaimTypes.Email,email)},"ApplicationCookie");
                        var ctx = Request.GetOwinContext();
                        var authManager = ctx.Authentication;
                        authManager.SignIn(identity);
                        return RedirectToAction("Index","Home");
                    }
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
        [HttpPost]
        public ActionResult Registration(Users model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MainDbContext())
                {
                    var queryUser = db.Users.FirstOrDefault(u => u.Email == model.Email);
                    if (queryUser == null)
                    {
                        var encryptedPassword = CustomEnrypt.Encrypt(model.Password);
                        var user = db.Users.Create();
                        user.Email = model.Email;
                        user.Password = encryptedPassword;
                        user.Country = model.Country;
                        user.Name = model.Name;
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            else { ModelState.AddModelError("", "One or more fields have been"); }
            return View();
        }
        

}
}