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
    public class BatchesController : Controller
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
        // GET: Batches
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                var batchs = db.Batchs.Include(b => b.Department);
                return View(batchs.ToList());
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Batches/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Batch batch = db.Batchs.Find(id);
                if (batch == null)
                {
                    return HttpNotFound();
                }
                return View(batch);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Batches/Create
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

        // POST: Batches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BatchNo,DepartmentId")] Batch batch)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Batchs.Add(batch);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", batch.DepartmentId);
                return View(batch);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Batches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Batch batch = db.Batchs.Find(id);
                if (batch == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", batch.DepartmentId);
                return View(batch);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Batches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BatchNo,DepartmentId")] Batch batch)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(batch).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", batch.DepartmentId);
                return View(batch);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Batches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Batch batch = db.Batchs.Find(id);
                if (batch == null)
                {
                    return HttpNotFound();
                }
                return View(batch);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                Batch batch = db.Batchs.Find(id);
                db.Batchs.Remove(batch);
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
