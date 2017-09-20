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
    public class CoursesController : Controller
    {
        private AccountDBContext db = new AccountDBContext();
        //session Control Method
        public bool AdminSessionControl()
        {
            if (Session["AdminId"] != null && Convert.ToInt32(Session["Power"]) == 1)
            {
                return true;
            }
            return false;
        }
        public bool ModeratSessionControl()
        {
            if (Session["AdminId"] != null && Convert.ToInt32(Session["Power"]) == 2)
            {
                return true;
            }
            return false;
        }
        public bool BatchCoordinatorSessionControl()
        {
            if (Session["AdminId"] != null && Convert.ToInt32(Session["Power"]) == 3)
            {
                return true;
            }
            return false;
        }
        public bool StaffSessionControl()
        {
            if (Session["AdminId"] != null && Convert.ToInt32(Session["Power"]) == 4)
            {
                return true;
            }
            return false;
        }
        public ActionResult AdminLoginFaild()
        {
            return RedirectToAction("LoginAdmin", "Admins");
        }

        // GET: Courses
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                var courses = db.Courses.Include(c => c.Department);
                return View(courses.ToList());
            }
            else
            {
               return AdminLoginFaild();
            }

        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
            else
            {
               return AdminLoginFaild();
            }

        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            if (AdminSessionControl())
            {
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseCode,CourseName,Credit,Description,Semester,DepartmentId")] Course course)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", course.DepartmentId);
                return View(course);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", course.DepartmentId);
                return View(course);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseCode,CourseName,Credit,Description,Semester,DepartmentId")] Course course)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", course.DepartmentId);
                return View(course);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                Course course = db.Courses.Find(id);
                db.Courses.Remove(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return AdminLoginFaild();
            }
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
