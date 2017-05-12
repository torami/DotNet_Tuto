using MyFirstWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MyFirstWebsite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var db = new MainDbContext();
            return View(db.Lists.ToList());
        }
        [HttpPost]
        public ActionResult Index(Lists list)
        {
            if (ModelState.IsValid)
            {
                string timeToday = DateTime.Now.ToString("h:mm:ss tt");
                string dateToday = DateTime.Now.ToString("M/dd/yyyy");
                using (var db = new MainDbContext())
                {
                    Claim sessionEmail = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email);
                    string userEmail = sessionEmail.Value;
                    var userIdQuery = db.Users.Where(u => u.Email == userEmail).Select(u => u.Id);
                    var userId = userIdQuery.ToList();
                    string new_item = Request.Form["new_item"];
                    string check_public = Request.Form["check_public"];

                    var dbList = db.Lists.Create();
                    dbList.Details = new_item;
                    dbList.Date_Posted = dateToday;
                    dbList.Time_Posted = timeToday;
                    //dbList.User_Id = userId[0];
                    if (check_public != null) { dbList.Public = "YES"; }
                    else { dbList.Public = "NO"; }
                    db.Lists.Add(dbList);
                    db.SaveChanges();
                }

            }
            else
            {
                ModelState.AddModelError("", "Incorrect format has been placed");
            }
            var listTable = new MainDbContext();
            return View(listTable.Lists.ToList());
        }

    }

}
