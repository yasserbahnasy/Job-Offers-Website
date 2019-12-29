using Job_Offers_Website.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            return View(job);
        }

        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string Message)
        {
            var UserId = User.Identity.GetUserId();
            var JobId = (int)Session["JobId"];

            var check = db.ApplayForJobs.Where(a=>a.JobId==JobId && a.UserId == UserId).ToList();

            if (check.Count < 1)
            {
                var job = new ApplayForJob();
                job.UserId = UserId;
                job.JobId = JobId;
                job.message = Message;
                job.ApplayDate = DateTime.Now;

                db.ApplayForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "تمت الإضافه بنجاح";
            }
            else
            {
                ViewBag.Result = "المعذرة, لقد سبق وتقدمت لهذة الوظيفة من قبل!";
            }
            


            return View();
        }

        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var UserId = User.Identity.GetUserId();
            var Jobs = db.ApplayForJobs.Where(a => a.UserId == UserId);
            return View(Jobs.ToList());
        }

        [Authorize]
        public ActionResult DetailsOfJob(int id)
        {
            var job = db.ApplayForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: EditOfJob/Edit/5
        public ActionResult EditOfJob(int id)
        {
            var job = db.ApplayForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: EditOfJob/Edit/5
        [HttpPost]
        public ActionResult EditOfJob(ApplayForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplayDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
        }

        // GET: DeleteOfJob/Delete/5
        public ActionResult DeleteOfJob(int id)
        {
            var job = db.ApplayForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: DeleteOfJob/Delete/5
        [HttpPost]
        public ActionResult DeleteOfJob(ApplayForJob job)
        {
            // TODO: Add delete logic here
            var Myjob = db.ApplayForJobs.Find(job.id);
            db.ApplayForJobs.Remove(Myjob);
            db.SaveChanges();
            return RedirectToAction("GetJobsByUser");
        }

        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            var UserId = User.Identity.GetUserId();
            var Jobs = from app in db.ApplayForJobs
                       join Job in db.Jobs
                       on app.JobId equals Job.id
                       where Job.User.Id == UserId
                       select app;

            var grouped = from j in Jobs
                          group j by j.job.JobTitle
                          into gr
                          select new JobViewModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };
                  
            return View(grouped.ToList());
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(searchName)
            || a.JobContent.Contains(searchName)
            || a.Category.CategoryName.Contains(searchName)
            || a.Category.CategoryDescription.Contains(searchName)).ToList();

            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {

            

            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("yasserbahnasy@gmail.com", "5544884");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("yasserbahnasy@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = "اسم المرسل: " + contact.name + "<br>" +
                            "بريد المرسل: " + contact.Email + "<br>" +
                            "عنوان الرسالة: " + contact.Subject + "<br>" +
                            "نص الرسالة: <b>" + contact.Message + "</b>";
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            return RedirectToAction("Index");
        }


    }
}