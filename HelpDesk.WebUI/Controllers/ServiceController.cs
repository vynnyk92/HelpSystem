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

    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        HelpdeskContext db = new HelpdeskContext();
        // GET: Service

        [HttpGet]
        public ActionResult Departments()
        {
            ViewBag.Departments = db.Departments;
            return View();
        }

        [HttpPost]
        public ActionResult Departments(Department dep)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(dep);
                db.SaveChanges();
            }
            ViewBag.Departments = db.Departments;
            return View(dep);
        }

        public ActionResult DeleteDepartment(int id)
        {
            Department dep = db.Departments.Find(id);
            db.Departments.Remove(dep);
            db.SaveChanges();
            return RedirectToAction("Departments");
        }

        [HttpGet]
        public ActionResult Activ()
        {
            ViewBag.Activs = db.Activs.Include(a => a.Department);
            ViewBag.Departments = new SelectList(db.Departments,"Id","Name"); 
            return View();
        }

        [HttpPost]
        public ActionResult Activ(Activ activ)
        {
            if (ModelState.IsValid)
            {
                db.Activs.Add(activ);
                db.SaveChanges();
            }
            ViewBag.Activs = db.Activs.Include(a => a.Department);
            ViewBag.Departments = new SelectList(db.Departments, "Id", "Name");
            return View(activ);
        }

        public ActionResult DeleteActiv(int id)
        {
            Activ activ = db.Activs.Find(id);
            db.Activs.Remove(activ);
            db.SaveChanges();
            return RedirectToAction("Activ");
        }

        [HttpGet]
        public ActionResult Categories()
        {
            ViewBag.Categories = db.Categories.ToList();
           
            return View();
        }

        [HttpPost]
        public ActionResult Categories(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
            }
            ViewBag.Categories = db.Categories.ToList();
            return View(cat);
        }

        public ActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Find(id);
            db.Categories.Remove(cat);
            db.SaveChanges();
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public ActionResult Roles()
        {
            ViewBag.Roles = db.Roles;
            return View();
        }

        [HttpPost]
        public ActionResult Roles(Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
            }
            ViewBag.Roles = db.Roles;
            return View(role);
        }

      
    }
}