using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.BBL;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class RoutineController : Controller
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
        
        RoutineBll _RoutineBll=new RoutineBll();

        // GET: Routine
        public ActionResult Index()
        {
            return View();
        }

        //course assigned view for student
        public ActionResult CourseAssignStudentView()
        {
            if (StudentSessionControl())
            {
                int UserId = Convert.ToInt32(Session["StudentId"]);
                var StudentInfo = db.Students.Where(s => s.Id == UserId).FirstOrDefault();
                var ListOfAssignCourse = _RoutineBll.CourseDepartments(StudentInfo);
                return View(ListOfAssignCourse);
            }
            else
            {
                return StudentLoginFaild();
            }
        }

        //course assigned view for teacher
        public ActionResult CourseAssignTeacherView()
        {
            if (TeacherSessionControl())
            {
                int TeacherId = Convert.ToInt32(Session["TeacherId"]);
                var ListOfAssignCourse = _RoutineBll.CourseDepartments(TeacherId);

                return View(ListOfAssignCourse);
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        //Routine assigned view for teacher
        public ActionResult RoomAssigneViewForTeacher()
        {
            if (TeacherSessionControl())
            {
                int TeacherId = Convert.ToInt32(Session["TeacherId"]);

                var RoomAssingList = _RoutineBll.RoomAssignViewModels(TeacherId);

                return View(RoomAssingList);
            }
            else
            {
                return TeacherLoginFaild();
            }

        }

        //Routine assigned view for student
        public ActionResult RoomAssigneViewForStudent()
        {
            if (StudentSessionControl())
            {
                int StudentId = Convert.ToInt32(Session["StudentId"]);
                var StudentInfo = db.Students.Where(x => x.Id == StudentId).FirstOrDefault();

                var RoomAssingList = _RoutineBll.RoomAssignViewModels(StudentInfo);

                return View(RoomAssingList);
            }
            else
            {
                return StudentLoginFaild();
            }

        }

      
    }
}