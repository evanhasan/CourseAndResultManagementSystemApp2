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
    public class DaysController : Controller
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


        // GET: Days
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                return View(db.Day.ToList());
            }
            else
            {
                return AdminLoginFaild();
            }

        }

        // GET: Days/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Day day = db.Day.Find(id);
                if (day == null)
                {
                    return HttpNotFound();
                }
                return View(day);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Days/Create
        public ActionResult Create()
        {
            if (AdminSessionControl())
            {
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Days/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DayName")] Day day)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Day.Add(day);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(day);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Days/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Day day = db.Day.Find(id);
                if (day == null)
                {
                    return HttpNotFound();
                }
                return View(day);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Days/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DayName")] Day day)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(day).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(day);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Days/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Day day = db.Day.Find(id);
                if (day == null)
                {
                    return HttpNotFound();
                }
                return View(day);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Days/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                Day day = db.Day.Find(id);
                db.Day.Remove(day);
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
