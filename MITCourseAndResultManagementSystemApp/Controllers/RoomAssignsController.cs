using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using MITCourseAndResultManagementSystemApp.BBL;
using MITCourseAndResultManagementSystemApp.Migrations;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class RoomAssignsController : Controller
    {
        private AccountDBContext db = new AccountDBContext();
        RoomAssignBll _roomAssign = new RoomAssignBll();
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

        // GET: RoomAssigns
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                var RoomAssingList = _roomAssign.RoomAssignViewModels();
                return View(RoomAssingList);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: RoomAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoomAssign roomAssign = db.RoomAssigns.Find(id);
                if (roomAssign == null)
                {
                    return HttpNotFound();
                }
                return View(roomAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: RoomAssigns/Create
        public ActionResult Create()
        {
            if (AdminSessionControl())
            {
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");
                ViewBag.Room = new SelectList(db.Rooms, "Id", "RoomNo");
                ViewBag.Day = new SelectList(db.Day, "Id", "DayName");

                // ViewBag.Day=new SelectList()
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: RoomAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomAssign roomAssign)
        {
            if (AdminSessionControl())
            {
                var check = _roomAssign.CountRoomAssign(roomAssign);
                if (ModelState.IsValid)
                {
                    if (check)
                    {
                        ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", roomAssign.DepartmentId);
                        ViewBag.Room = new SelectList(db.Rooms, "Id", "RoomNo");
                        ViewBag.Day = new SelectList(db.Day, "Id", "DayName");
                        ViewBag.Message = "This Room is Assign for this time ";
                        roomAssign = null;
                        return View(roomAssign);
                    }
                    else
                    {
                        db.RoomAssigns.Add(roomAssign);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", roomAssign.DepartmentId);
                ViewBag.Room = new SelectList(db.Rooms, "Id", "RoomNo");
                ViewBag.Day = new SelectList(db.Day, "Id", "DayName");
                return View(roomAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: RoomAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoomAssign roomAssign = db.RoomAssigns.Find(id);
                if (roomAssign == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", roomAssign.DepartmentId);
                ViewBag.Room = new SelectList(db.Rooms, "Id", "RoomNo", roomAssign.RoomId);
                ViewBag.Day = new SelectList(db.Day, "Id", "DayName", roomAssign.Day);
              
                return View(roomAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: RoomAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomAssign roomAssign)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    var check = _roomAssign.CountRoomAssign(roomAssign);
                    if (check)
                    {
                        ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", roomAssign.DepartmentId);
                        ViewBag.Room = new SelectList(db.Rooms, "Id", "RoomNo", roomAssign.RoomId);
                        ViewBag.Day = new SelectList(db.Day, "Id", "DayName", roomAssign.Day);
                        ViewBag.Message = "This Room is Assign for this time ";
                        return View(roomAssign);
                    }
                    else
                    {
                        db.Entry(roomAssign).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", roomAssign.DepartmentId);
                    ViewBag.Room = new SelectList(db.Rooms, "Id", "RoomNo", roomAssign.RoomId);
                    ViewBag.Day = new SelectList(db.Day, "Id", "DayName", roomAssign.Day);
                    return View(roomAssign);
                }

            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: RoomAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoomAssign roomAssign = db.RoomAssigns.Find(id);
                if (roomAssign == null)
                {
                    return HttpNotFound();
                }
                return View(roomAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: RoomAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                RoomAssign roomAssign = db.RoomAssigns.Find(id);
                db.RoomAssigns.Remove(roomAssign);
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


        //get Course
        public JsonResult GetCourseList(int DepartmentId)
        {
            var BatchResult = db.Courses.Where(x => x.DepartmentId == DepartmentId).ToList().Select(c => new { c.Id, c.CourseName }).ToList();

            return Json(BatchResult, JsonRequestBehavior.AllowGet);
        }
        //get Course Semester
        public JsonResult GetCourseSemesterList(int CourseId)
        {
            var Semester = db.Courses.Where(x => x.Id == CourseId).Select(x => x.Semester);

            return Json(Semester, JsonRequestBehavior.AllowGet);
        }
    }
}
