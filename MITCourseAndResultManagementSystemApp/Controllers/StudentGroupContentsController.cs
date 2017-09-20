using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class StudentGroupContentsController : Controller
    {
        private AccountDBContext db = new AccountDBContext();

        // GET: StudentGroupContents
        public ActionResult Index()
        {
            if (Session["StudentId"] != null)
            {
                int UserId = Convert.ToInt32(Session["StudentId"]);
                var StudentInfo = db.Students.Where(s => s.Id == UserId).FirstOrDefault();

                var Content=db.StudentGroupContent.Where(
                    x => x.DepartmentId == StudentInfo.DepartmentId && x.BatchId == StudentInfo.BatchId).ToList();

                return View(Content);

            }
            else
            {
                return RedirectToAction("LoginStudent", "Students");
            }
        }

        // GET: StudentGroupContents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGroupContent studentGroupContent = db.StudentGroupContent.Find(id);
            if (studentGroupContent == null)
            {
                return HttpNotFound();
            }
            return View(studentGroupContent);
        }

        // GET: StudentGroupContents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentGroupContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,StudentId,DepartmentId,BatchId,DateTime,FilePath")] StudentGroupContent studentGroupContent)
        {
            if (ModelState.IsValid)
            {
                db.StudentGroupContent.Add(studentGroupContent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentGroupContent);
        }

        // GET: StudentGroupContents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGroupContent studentGroupContent = db.StudentGroupContent.Find(id);
            if (studentGroupContent == null)
            {
                return HttpNotFound();
            }
            return View(studentGroupContent);
        }

        // POST: StudentGroupContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,StudentId,DepartmentId,BatchId,DateTime,FilePath")] StudentGroupContent studentGroupContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentGroupContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentGroupContent);
        }

        // GET: StudentGroupContents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGroupContent studentGroupContent = db.StudentGroupContent.Find(id);
            if (studentGroupContent == null)
            {
                return HttpNotFound();
            }
            return View(studentGroupContent);
        }

        // POST: StudentGroupContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentGroupContent studentGroupContent = db.StudentGroupContent.Find(id);
            db.StudentGroupContent.Remove(studentGroupContent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
