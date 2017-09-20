using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class DepartmentsController : Controller
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
        // GET: Departments
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                return View(db.Departments.ToList());
            }
            else
            {
                return AdminLoginFaild();
            }

        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department department = db.Departments.Find(id);
                if (department == null)
                {
                    return HttpNotFound();
                }
                return View(department);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Departments/Create
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

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DepartmentCode,DepartmentName")] Department department)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Departments.Add(department);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(department);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department department = db.Departments.Find(id);
                if (department == null)
                {
                    return HttpNotFound();
                }
                return View(department);
            }
            else
            {
                return AdminLoginFaild();
            }

        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DepartmentCode,DepartmentName")] Department department)
        {

            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(department);
            }
            else
            {
                return AdminLoginFaild();

            }


        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department department = db.Departments.Find(id);
                if (department == null)
                {
                    return HttpNotFound();
                }
                return View(department);
            }
            else
            {
                return AdminLoginFaild();

            }

        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                Department department = db.Departments.Find(id);
                db.Departments.Remove(department);
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



        public ActionResult CourseByDepartment()
        {
            if (AdminSessionControl())
            {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");

            return View();
            }
            else
            {
                return RedirectToAction("LoginMaster", "MasterUser");
            }
        }

        //show assign course list
        public JsonResult CourseByDepartmentList(int DepartmentId)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");

           var ListOfCourse2 =
                db.CourseAssigns.Where(x => x.DepartmentId == DepartmentId)
                .Join(db.Teachers, x=>x.TeacherId, tt=>tt.Id, (x,tt)=>new{x,tt})
                .Join(db.Courses, x2=>x2.x.CourseId, cc=>cc.Id,(x2,cc)=>new{x2,cc})
                .Join(db.Batchs,x3=>x3.x2.x.BatchId,bb=>bb.Id,(x3,bb)=>new{x3,bb})
                .Join(db.Departments,x4=>x4.x3.x2.x.DepartmentId, dd=>dd.Id,(x4,dd)=>new {x4,dd})
                .Select(
                        z =>
                            new
                            {
                                Id = z.x4.x3.x2.x.Id,
                                DepartmentName = z.dd.DepartmentName,
                                BatchNumber = z.x4.bb.BatchNo,
                                CourseName = z.x4.x3.cc.CourseName,
                                Semester = z.x4.x3.cc.Semester,
                                TeacherName = z.x4.x3.x2.tt.TeacherName,
                                flag=z.x4.x3.x2.x.Flag
                            }).ToList();

            return Json(ListOfCourse2, JsonRequestBehavior.AllowGet);
        }

        
            //show assign course by dep and batchlist
        public JsonResult CourseBatchByDepartmentList(int DepartmentId, int BatchId)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");

           var ListOfCourse2 =
                db.CourseAssigns.Where(x => x.DepartmentId == DepartmentId && x.BatchId==BatchId)
                .Join(db.Teachers, x=>x.TeacherId, tt=>tt.Id, (x,tt)=>new{x,tt})
                .Join(db.Courses, x2=>x2.x.CourseId, cc=>cc.Id,(x2,cc)=>new{x2,cc})
                .Join(db.Batchs,x3=>x3.x2.x.BatchId,bb=>bb.Id,(x3,bb)=>new{x3,bb})
                .Join(db.Departments,x4=>x4.x3.x2.x.DepartmentId, dd=>dd.Id,(x4,dd)=>new {x4,dd})
                .Select(
                        z =>
                            new
                            {
                                Id = z.x4.x3.x2.x.Id,
                                DepartmentName = z.dd.DepartmentName,
                                BatchNumber = z.x4.bb.BatchNo,
                                CourseName = z.x4.x3.cc.CourseName,
                                Semester = z.x4.x3.cc.Semester,
                                TeacherName = z.x4.x3.x2.tt.TeacherName,
                                flag=z.x4.x3.x2.x.Flag
                            }).ToList();

            return Json(ListOfCourse2, JsonRequestBehavior.AllowGet);
        }
    }
}
