using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpDesk.WebUI.Concrete;
using HelpDesk.WebUI.Entities;
using System.Data.Entity;
using System.Data;

namespace HelpDesk.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        HelpdeskContext db = new HelpdeskContext();


        public ActionResult Index()
        {
            List<User> users = db.Users.Include(u => u.Department).Include(u=>u.Role).ToList();
            return View(users);
        }
    }
}