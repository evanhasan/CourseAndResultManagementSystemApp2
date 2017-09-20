using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.BBL;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class StudentsController : Controller
    {
        StudentBll _StudentBll = new StudentBll();
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
        // GET: Students
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                //Department and Batch wise Group List
                ViewBag.ListStudents = db.Students.GroupBy(x => new { x.DepartmentId, x.BatchId })
                        .Select(x => new DepartmentWithBatch { DepartmentId = x.Key.DepartmentId, BatchId = x.Key.BatchId, Count = x.Count() }).ToList()
                        .Join(db.Departments, x1 => x1.DepartmentId, dd => dd.Id, (x1, dd) => new { x1, dd })
                        .Join(db.Batchs, x2 => x2.x1.BatchId, bb => bb.Id, (x2, bb) => new { x2, bb })
                        .Select(z => new DepartmentWithBatch
                        {
                            Department = z.x2.dd.DepartmentName,
                            DepartmentId = z.x2.x1.DepartmentId,
                            Batch = z.bb.BatchNo,
                            BatchId = z.x2.x1.BatchId,
                            Count = z.x2.x1.Count

                        }).ToList();
                var StudentsList = _StudentBll.StudentListViewModels();
                return View(StudentsList);
            }
            else
            {
                return AdminLoginFaild();
            }
        }



        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                int BatchNo = db.Batchs.Where(w => w.Id == student.BatchId).Select(s => s.BatchNo).FirstOrDefault();
                ViewBag.BatchNo = BatchNo;
                return View(student);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Students/Create
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

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student, HttpPostedFileBase PhotoPath2)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        string PathAddress = "";
                        string PathAddress2 = "";

                        Password password = new Password();
                        int PasswordStatus = password.CheckStrength(student.Password);
                        var ExistEmail = db.Students.Where(w => w.Email == student.Email);
                        var ExistReg = db.Students.Where(w => w.RegNo == student.RegNo).Count();

                        if (PasswordStatus < 4)
                        {
                            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                            ViewBag.Message = "Password Not strong: one capital letter, one small Letter, one digit and one special charecture needed ";
                            student.Password = "";
                            student.ConfirmPassword = "";
                            return View(student);
                        }
                        else if (PhotoPath2 == null && PhotoPath2.ContentLength < 0)
                        {
                            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                            ViewBag.Message = "Photo Not Acceptable";
                            student.Password = "";
                            student.ConfirmPassword = "";
                            return View(student);
                        }
                        else if (ExistEmail.Count() > 0)
                        {
                            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                            ViewBag.Message = "Email Address Already Existed";
                            student.Password = "";
                            student.ConfirmPassword = "";
                            return View(student);
                        }
                        else if (ExistReg > 0)
                        {
                            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                            ViewBag.Message = "Reg Number Already Existed";
                            student.Password = "";
                            student.ConfirmPassword = "";
                            return View(student);
                        }
                        else
                        {
                            //Photo Upload
                            RandomNumber _ran = new RandomNumber();
                            string Random = _ran.Random(10);
                            string Random2 = _ran.Random(10);

                            string _FileName = System.IO.Path.GetFileName(student.MobileNo+Random + Random2+PhotoPath2.FileName);
                            PathAddress = Path.Combine(Server.MapPath("~/ShareFiles/Students/"), _FileName);
                            PathAddress2 = "ShareFiles/Students/" + _FileName;
                            PhotoPath2.SaveAs(PathAddress);

                            //encript password here
                            byte[] bytes = Encoding.Unicode.GetBytes(student.Password);
                            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                            student.Password = Convert.ToBase64String(inArray);

                            student.ConfirmPassword = Convert.ToBase64String(inArray);
                            student.PhotoPath = PathAddress2;

                            db.Students.Add(student);
                            db.SaveChanges();
                            Mail _mail = new Mail();
                         //   _mail.SendMail(student.Email, "Account Created From IIT System", "Dear Student greeting From IIT, This email is now registered  to access IIT Course management System");


                            //Result Table will be created
                            int StudentId=ExistEmail.FirstOrDefault().Id;
                            var ListofCourse =
                            db.Courses.Where(x => x.DepartmentId == ExistEmail.FirstOrDefault().DepartmentId).ToList();

                            foreach (var course in ListofCourse)
                            {
                                Result _result=new Result();
                                _result.StudentId = StudentId;
                                _result.CourseId = course.Id;
                                _result.Credit = course.Credit;
                                db.Results.Add(_result);
                                db.SaveChanges();
                            }

                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                        ViewBag.Message = "Something Wrong";
                        student.Password = "";
                        student.ConfirmPassword = "";
                        return View(student);
                    }
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                return View(student);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                int BatchNo = db.Batchs.Where(w => w.Id == student.BatchId).Select(s => s.BatchNo).FirstOrDefault();
                ViewBag.BatchNo = BatchNo;
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                return View(student);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentCode", student.DepartmentId);
                return View(student);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                int BatchNo = db.Batchs.Where(w => w.Id == student.BatchId).Select(s => s.BatchNo).FirstOrDefault();
                ViewBag.BatchNo = BatchNo;
                return View(student);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        //Teacher login Action
        public ActionResult LoginStudent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginStudent(Student _student)
        {
            AccountDBContext DB = new AccountDBContext();

            if (_student.Email != null && _student.Password != null)
            {
                //encript password here
                byte[] bytes = Encoding.Unicode.GetBytes(_student.Password);
                byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                _student.Password = Convert.ToBase64String(inArray);
                var Student = DB.Students.Where(x => x.Email == _student.Email && x.Password == _student.Password).FirstOrDefault();
                if (Student != null)
                {
                    Session["Email"] = Student.Email.ToString();
                    Session["StudentId"] = Student.Id.ToString();
                    Session["StudentName"] = Student.Name.ToString();
                    return RedirectToAction("StudentView", "ShareContents");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password not matched");

                }
            }
            return View();
        }


        //Dashboard Teacher
        public ActionResult StudentDashboard()
        {
            if (StudentSessionControl())
            {
                return View();
            }
            else
            {
                return StudentLoginFaild();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("LoginStudent", "Students");
        }


        public ActionResult ChangePassword()
        {
            if (StudentSessionControl())
            {
                return View();
            }
            else
            {
                return StudentLoginFaild();
            }
        }
       
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword _change)
        {
            if (StudentSessionControl())
            {
                if (ModelState.IsValid)
                {
                    var UserId = Convert.ToInt32(Session["StudentId"]);
                    var StudentInfo = db.Students.Where(x => x.Id == UserId).FirstOrDefault();

                    //encript password here
                    byte[] bytes = Encoding.Unicode.GetBytes(_change.OldPassword);
                    byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                    _change.OldPassword = Convert.ToBase64String(inArray);

                    if (StudentInfo.Password == _change.OldPassword)
                    {
                        //encript password here
                        byte[] bytes2 = Encoding.Unicode.GetBytes(_change.NewPassword);
                        byte[] inArray2 = HashAlgorithm.Create("SHA1").ComputeHash(bytes2);
                        _change.NewPassword = Convert.ToBase64String(inArray2);

                        byte[] bytes3 = Encoding.Unicode.GetBytes(_change.ConfirmNewPassword);
                        byte[] inArray3 = HashAlgorithm.Create("SHA1").ComputeHash(bytes3);
                        _change.ConfirmNewPassword = Convert.ToBase64String(inArray3);

                        StudentInfo.Password = _change.NewPassword;
                        StudentInfo.ConfirmPassword = _change.ConfirmNewPassword;
                        db.SaveChanges();
                        ViewBag.Message = "Password has been changed";
                        Mail _mail = new Mail();
                        _mail.SendMail(StudentInfo.Email, "Password Change", "Dear Student, Your IIT System Login Password Has Been Changed");
                        return View();
                    }
                }
                ViewBag.Message = "Something Wrong";
                return View();
            }
            else
            {
                return StudentLoginFaild();
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

        public ActionResult CourseViewForStudent()
        {
            if (StudentSessionControl())
            {
                int UserId = Convert.ToInt32(Session["StudentId"]);
                var StudentInfo = db.Students.Where(s => s.Id == UserId).FirstOrDefault();
                var CourseList = db.Courses.Where(w => w.DepartmentId == StudentInfo.DepartmentId).ToList().OrderBy(x => x.Semester);

                return View(CourseList);
            }
            else
            {
                return StudentLoginFaild();
            }
        }
        //this list will see students
        public ActionResult StudentListViewForStudent()
        {
            if (StudentSessionControl())
            {
                int UserId = Convert.ToInt32(Session["StudentId"]);
                var StudentInfo = db.Students.Where(s => s.Id == UserId).FirstOrDefault();
                var StudentsList = _StudentBll.StudentsListForStudentShow(StudentInfo);
                return View(StudentsList);
            }
            else
            {
                return StudentLoginFaild();
            }
        }

        public ActionResult StudentSelected(string Dep, int Batch)
        {
            if (AdminSessionControl())
            {
                var StudentsList = _StudentBll.StudentListViewModels().Where(x=>x.Department==Dep && x.BatchNumber==Batch);
               
                //Department and Batch wise Group List
                ViewBag.ListStudents = db.Students.GroupBy(x => new { x.DepartmentId, x.BatchId })
                        .Select(x => new DepartmentWithBatch { DepartmentId = x.Key.DepartmentId, BatchId = x.Key.BatchId, Count = x.Count() }).ToList()
                        .Join(db.Departments, x1 => x1.DepartmentId, dd => dd.Id, (x1, dd) => new { x1, dd })
                        .Join(db.Batchs, x2 => x2.x1.BatchId, bb => bb.Id, (x2, bb) => new { x2, bb })
                        .Select(z => new DepartmentWithBatch
                        {
                            Department = z.x2.dd.DepartmentName,
                            DepartmentId = z.x2.x1.DepartmentId,
                            Batch = z.bb.BatchNo,
                            BatchId = z.x2.x1.BatchId,
                            Count = z.x2.x1.Count

                        }).ToList();

                

                return View(StudentsList);
            }
            else
            {
                return AdminLoginFaild();
            }
        }
    }
}
