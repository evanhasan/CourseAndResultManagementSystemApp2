using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class UnassignController : Controller
    {
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

        public bool StudentSessionControl()
        {
            if (Session["StudentId"] != null)
            {
                return true;
            }
            return false;
        }
        public bool TeacherSessionControl()
        {
            if (Session["TeacherId"] != null)
            {
                return true;
            }
            return false;
        }

        public ActionResult AdminLoginFaild()
        {
            return RedirectToAction("LoginAdmin", "Admins");
        }
        public ActionResult TeacherLoginFaild()
        {
            return RedirectToAction("LoginTeacher", "Teachers");
        }
        public ActionResult StudentLoginFaild()
        {
            return RedirectToAction("LoginStudent", "Students");
        }
        // GET: Unassign
        AccountDBContext db = new AccountDBContext();
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                return View("UnassignCourse");
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        public ActionResult UnassignCourse()
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

        [HttpPost]
        public ActionResult UnassignCourse(UnassignCourse _unassignCourse)
        {
            //room remove
            if (AdminSessionControl())
            {
                var _roomAssign =
                   db.RoomAssigns.Where(
                       x =>
                           x.DepartmentId == _unassignCourse.DepartmentId && x.BatchId == _unassignCourse.BatchId &&
                           x.SemesterId == _unassignCourse.Semester).ToList();
                foreach (var item in _roomAssign)
                {
                    db.RoomAssigns.Remove(item);
                }
                //course unassigned
                var query = db.CourseAssigns.Where(
                     x => x.DepartmentId == _unassignCourse.DepartmentId && x.BatchId == _unassignCourse.BatchId)
                     .Join(db.Courses, c => c.CourseId, s => s.Id, (c, s) => new { c, s })
                     .Where(x => x.s.Semester == _unassignCourse.Semester).Select(z => z.c);

                foreach (CourseAssign item in query)
                {
                    item.Flag = 0;
                    item.Credit = 0;
                }
                db.SaveChanges();
                ViewBag.Message = "Course Unassign Successfully";
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }
    }
}