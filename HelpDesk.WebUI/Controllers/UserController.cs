using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using HelpDesk.WebUI.Concrete;
using HelpDesk.WebUI.Entities;
using HelpDesk.WebUI.Models;

namespace HelpDesk.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Moderator, Executor")]
    public class UserController : Controller
    {
        private HelpdeskContext db = new HelpdeskContext();
        

        public ActionResult Index()
        {
            List<Department> departments = db.Departments.ToList();
            //Добавляем в список возможность выбора всех
            departments.Insert(0, new Department { Name = "All", Id = 0 });
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            List<Role> roles = db.Roles.ToList();
            roles.Insert(0, new Role { Name = "All", Id = 0 });
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View();
        }

        public ActionResult GetPeople(int department = 0, int role = 0)
        {
            
            IEnumerable<User> allUsers = null;
            if (role == 0 && department == 0)
            {
                allUsers = db.Users.Include(u => u.Department).Include(u => u.Role).ToList();
               
               
            }
            else if (role == 0 && department != 0)
            {
                allUsers = db.Users.Where(p=>p.DepartmentId==department).Include(u => u.Department).Include(u => u.Role).ToList();
                
            }
            else if (role != 0 && department == 0)
            {
                allUsers = db.Users.Where(p => p.RoleId == role).Include(u => u.Department).Include(u => u.Role).ToList();
                
            }
            else
            {
                allUsers = db.Users.Where(p => p.DepartmentId == department && p.RoleId==role).Include(u => u.Department).Include(u => u.Role).ToList();
                
            }
            return PartialView(allUsers);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            SelectList departments = new SelectList(db.Departments, "Id", "Name");
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectList departments = new SelectList(db.Departments, "Id", "Name");
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = roles;

            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            SelectList departments = new SelectList(db.Departments, "Id", "Name");
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectList departments = new SelectList(db.Departments, "Id", "Name");
            ViewBag.Departments = departments;
            SelectList roles = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = roles;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}