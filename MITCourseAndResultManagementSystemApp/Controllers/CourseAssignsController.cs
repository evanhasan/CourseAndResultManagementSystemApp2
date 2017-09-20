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
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class CourseAssignsController : Controller
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
        // GET: CourseAssigns
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                var courseAssigns = db.CourseAssigns.Include(c => c.Department);

                 List<CourseDepartment> ListOfAssignCourse =db.CourseAssigns.Where(ww=>ww.Flag==1)
                        .Join(db.Teachers, x => x.TeacherId, tt => tt.Id, (x, tt) => new { x, tt })
                        .Join(db.Courses, x2 => x2.x.CourseId, cc => cc.Id, (x2, cc) => new { x2, cc })
                        .Join(db.Batchs, x3 => x3.x2.x.BatchId, bb => bb.Id, (x3, bb) => new { x3, bb })
                        .Join(db.Departments, x4 => x4.x3.x2.x.DepartmentId, dd => dd.Id, (x4, dd) => new { x4, dd })
                        .Select(
                            z =>
                                new CourseDepartment
                                {
                                    Id = z.x4.x3.x2.x.Id,
                                    DepartmentName = z.dd.DepartmentName,
                                    BatchNumber = z.x4.bb.BatchNo,
                                    CourseName = z.x4.x3.cc.CourseName,
                                    Semester = z.x4.x3.cc.Semester,
                                    TeacherName = z.x4.x3.x2.tt.TeacherName,
                                    Flag = z.x4.x3.x2.x.Flag,

                                }).OrderBy(x => x.BatchNumber).ToList();
                ViewBag.ListOfAssignCourse = ListOfAssignCourse;

                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: CourseAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CourseAssign courseAssign = db.CourseAssigns.Find(id);
                if (courseAssign == null)
                {
                    return HttpNotFound();
                }
                return View(courseAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: CourseAssigns/Create
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

        // POST: CourseAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BatchId,CourseId,TeacherId,Credit,DepartmentId")] CourseAssign courseAssign)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    int count = db.CourseAssigns.Count(
                          x =>x.DepartmentId == courseAssign.DepartmentId && x.BatchId == courseAssign.BatchId &&
                              x.TeacherId == courseAssign.TeacherId && x.CourseId == courseAssign.CourseId);

                    var flag = db.CourseAssigns.Where(x =>
                        x.DepartmentId == courseAssign.DepartmentId && x.BatchId == courseAssign.BatchId &&
                        x.TeacherId == courseAssign.TeacherId && x.CourseId == courseAssign.CourseId)
                        .Select(x => x.Flag).FirstOrDefault();

                    var Exist = db.CourseAssigns.Where(x =>
                        x.DepartmentId == courseAssign.DepartmentId && x.BatchId == courseAssign.BatchId &&
                        x.TeacherId == courseAssign.TeacherId && x.CourseId == courseAssign.CourseId)
                        .Select(x => x).FirstOrDefault();

                    //Count Same Id Teacher Course er koyta ase
                    if (count > 0)
                    {
                        if (flag == 0)
                        {
                            Exist.Flag = 1;
                            Exist.Credit = courseAssign.Credit;

                            db.SaveChanges();

                            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");
                            ViewBag.Message = "Assigned Again";
                            return View(courseAssign);
                        }
                        else
                        {
                            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");
                            ViewBag.Message = "Already Assigned";
                            courseAssign = null;
                            return View(courseAssign);
                        }

                    }
                    else
                    {
                        courseAssign.Flag = 1;
                        db.CourseAssigns.Add(courseAssign);

                        db.SaveChanges();
                        ViewBag.Message = " Assigned Successfully";
                        ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");
                        return View();
                    }
                }
                else
                {
                    ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode");
                    return View();
                }
            }
            else
            {
                return AdminLoginFaild();
            }
            
        }

        // GET: CourseAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CourseAssign courseAssign = db.CourseAssigns.Find(id);
                if (courseAssign == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", courseAssign.DepartmentId);
                return View(courseAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: CourseAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BatchId,CourseId,TeacherId,Credit,DepartmentId")] CourseAssign courseAssign)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(courseAssign).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", courseAssign.DepartmentId);
                return View(courseAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: CourseAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CourseAssign courseAssign = db.CourseAssigns.Find(id);
                if (courseAssign == null)
                {
                    return HttpNotFound();
                }
                return View(courseAssign);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: CourseAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                CourseAssign courseAssign = db.CourseAssigns.Find(id);

                var _roomAssign =
                                   db.RoomAssigns.Where(
                                       x =>
                                           x.DepartmentId == courseAssign.DepartmentId && x.BatchId == courseAssign.BatchId &&
                                           x.TeacherId == courseAssign.TeacherId && x.CourseId==courseAssign.CourseId).ToList();

                foreach (var item in _roomAssign)
                {
                    db.RoomAssigns.Remove(item);
                }
               


               db.CourseAssigns.Remove(courseAssign);
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

        //get Batch
        public JsonResult GetBatchList(int DepartmentId)
        {
            var BatchResult = db.Batchs.Where(x => x.DepartmentId == DepartmentId).ToList().Select(c => new { c.Id, c.BatchNo }).ToList();

            return Json(BatchResult, JsonRequestBehavior.AllowGet);
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

        //get Teacher
        public JsonResult GetTeacherList(int DepartmentId)
        {
            //var BatchResult = db.Teachers.Where(x => x.DepartmentId == DepartmentId).ToList().Select(c => new { c.Id, c.TeacherName }).ToList();
            var BatchResult = db.Teachers.ToList().Select(c => new { c.Id, c.TeacherName }).ToList();


            return Json(BatchResult, JsonRequestBehavior.AllowGet);
        }
        //get Course Credit 
        public JsonResult GetCourseCredit(int CourseId)
        {
            var BatchResult = db.Courses.Where(x => x.Id == CourseId).Select(c => c.Credit);

            return Json(BatchResult, JsonRequestBehavior.AllowGet);
        }

        //get Teacher Credit 
        public JsonResult GetTeacherCredit(int TeacherId)
        {
            var BatchResult = db.Teachers.Where(x => x.Id == TeacherId).Select(c => c.CreditTaken);

            return Json(BatchResult, JsonRequestBehavior.AllowGet);
        }

        //get Teacher Credit 
        public JsonResult GetTeacherCreditRamain(int TeacherId)
        {
            var Max = db.Teachers.Where(x => x.Id == TeacherId).Sum(c => c.CreditTaken);
            var Count = db.CourseAssigns.Where(x => x.TeacherId == TeacherId).Count();

            double remain = 0;
            if (Count > 0)
            {
                var Use = db.CourseAssigns.Where(x => x.TeacherId == TeacherId).Sum(c => c.Credit);
                remain = Convert.ToDouble(Max) - Convert.ToDouble(Use);
            }
            else
            {
                remain = Convert.ToDouble(Max);
            }

            return Json(remain, JsonRequestBehavior.AllowGet);
        }
    }
}
