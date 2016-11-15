using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HelpDesk.WebUI.Concrete;
using HelpDesk.WebUI.Entities;

namespace HelpDesk.WebUI.Controllers
{
    [Authorize]
    public class ProposalController : Controller
    {
        HelpdeskContext db = new HelpdeskContext();

        [HttpGet]
        public ActionResult Create()
        {
            
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                
                var cabs = from cab in db.Activs
                           where cab.DepartmentId == user.DepartmentId
                           select cab;
                ViewBag.Cabs = new SelectList(cabs, "Id", "RoomNumber");

                ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");

                return View();
            }
            return RedirectToAction("LogOff", "Account");
        }

        
        [HttpPost]
        public ActionResult Create(Proposal proposal, HttpPostedFileBase error)
        {
            
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (ModelState.IsValid)
            {
                
                proposal.Status = (int)ProposalStatus.Opened;
                
                DateTime current = DateTime.Now;

                
                Lifecycle newLifecycle = new Lifecycle() { Opened = current };
                proposal.Lifecycle = newLifecycle;

                
                db.Lifecycles.Add(newLifecycle);

               
                proposal.UserId = user.Id;

                
                if (error != null)
                {

                    
                    string ext = error.FileName.Substring(error.FileName.LastIndexOf('.'));
                    
                    string path = current.ToString("dd.mm.yyyy hh:mm:ss").Replace(":", "_").Replace("/", ".") + ext;
                    error.SaveAs(Server.MapPath("~/Files/" + path));
                    proposal.File = path;
                }
                
                db.Proposals.Add(proposal);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(proposal);
        }

        public ActionResult Index()
        {
            
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).First();

            var proposals = db.Proposals.Where(r => r.UserId == user.Id) 
                                        .Include(r => r.Category)  
                                        .Include(r => r.Lifecycle)  
                                        .Include(r => r.User)        
                                        .OrderByDescending(r => r.Lifecycle.Opened);  

            return View(proposals.ToList());
        }

        
        public ActionResult Delete(int id)
        {
            Proposal proposal = db.Proposals.Find(id);
       
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).First();
            if (proposal != null && proposal.UserId == user.Id)
            {
                Lifecycle lifecycle = db.Lifecycles.Find(proposal.LifecycleId);
                db.Lifecycles.Remove(lifecycle);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult Details(int id)
        {
            Proposal proposal = db.Proposals.Find(id);

            if (proposal != null)
            {
      
                var activ = db.Activs.Where(m => m.Id == proposal.ActivId);
                if (activ.Count() > 0)
                {
                    proposal.Activ = activ.First();
                }
            
                proposal.Category = db.Categories.Where(m => m.Id == proposal.CategoryId).First();
                return PartialView("_Details", proposal);

            }
            return View("Index");
        }

      
        public ActionResult Executor(int id)
        {
            User executor = db.Users.Where(m => m.Id == id).First();

            if (executor != null)
            {
                return PartialView("_Executor", executor);
            }
            return View("Index");
        }

      
        public ActionResult Lifecycle(int id)
        {
            Lifecycle lifecycle = db.Lifecycles.Where(m => m.Id == id).First();

            if (lifecycle != null)
            {
                return PartialView("_Lifecycle", lifecycle);
            }
            return View("Index");
        }

       
        [Authorize(Roles = "Admin")]
        public ActionResult ProposalList()
        {
            var proposals = db.Proposals.Include(r => r.Category)  
                                        .Include(r => r.Lifecycle) 
                                        .Include(r => r.User)         
                                        .OrderByDescending(r => r.Lifecycle.Opened); 

            return View(proposals.ToList());
        }

    
        public ActionResult Download(int id)
        {
            Proposal r = db.Proposals.Find(id);
            if (r != null)
            {
                string filename = Server.MapPath("~/Files/" + r.File);
                string contentType = "image/jpeg";

                string ext = filename.Substring(filename.LastIndexOf('.'));
                switch (ext)
                {
                    case "txt":
                        contentType = "text/plain";
                        break;
                    case "png":
                        contentType = "image/png";
                        break;
                    case "tiff":
                        contentType = "image/tiff";
                        break;
                }
                return File(filename, contentType, filename);
            }

            return Content("File is absent");
        }


     
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public ActionResult Distribute()
        {
            var requests = db.Proposals.Include(r => r.User)
                                    .Include(r => r.Lifecycle)
                                    .Include(r => r.Executor)
                                    .Where(r => r.ExecutorId == null)
                                    .Where(r => r.Status != (int)ProposalStatus.Closed);
            List<User> executors = db.Users.Include(e => e.Role).Where(e => e.Role.Name == "Executor").ToList<User>();

            ViewBag.Executors = new SelectList(executors, "Id", "Name");
            return View(requests);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public ActionResult Distribute(int? requestId, int? executorId)
        {
            if (requestId == null && executorId == null)
            {
                return RedirectToAction("Distribute");
            }
            Proposal req = db.Proposals.Find(requestId);
            User ex = db.Users.Find(executorId);
            if (req == null && ex == null)
            {
                return RedirectToAction("Distribute");
            }
            req.ExecutorId = executorId;

            req.Status = (int)ProposalStatus.Distributed;
            Lifecycle lifecycle = db.Lifecycles.Find(req.LifecycleId);
            lifecycle.Distributed = DateTime.Now;
            db.Entry(lifecycle).State = EntityState.Modified;

            db.Entry(req).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Distribute");
        }

        
        [HttpGet]
        [Authorize(Roles = "Executor")]
        public ActionResult ChangeStatus()
        {
        
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).First();
            if (user != null)
            {
                var requests = db.Proposals.Include(r => r.User)
                                    .Include(r => r.Lifecycle)
                                    .Include(r => r.Executor)
                                    .Where(r => r.ExecutorId == user.Id)
                                    .Where(r => r.Status != (int)ProposalStatus.Closed);
                return View(requests);
            }
            return RedirectToAction("LogOff", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "Executor")]
        public ActionResult ChangeStatus(int requestId, int status)
        {
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).First();
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            Proposal req = db.Proposals.Find(requestId);
            if (req != null)
            {
                req.Status = status;
                Lifecycle lifecycle = db.Lifecycles.Find(req.LifecycleId);
                if (status == (int)ProposalStatus.Processing)
                {
                    lifecycle.Processing = DateTime.Now;
                }
                else if (status == (int)ProposalStatus.Checking)
                {
                    lifecycle.Checking = DateTime.Now;
                }
                else if (status == (int)ProposalStatus.Closed)
                {
                    lifecycle.Closed = DateTime.Now;
                }
                db.Entry(lifecycle).State = EntityState.Modified;
                db.Entry(req).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ChangeStatus");
        }

    }
}
