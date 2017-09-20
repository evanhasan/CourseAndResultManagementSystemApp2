using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class TeachersController : Controller
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
        // GET: Teachers
        public ActionResult Index()
        {
            if (AdminSessionControl())
            {
                var teachers = db.Teachers;
                return View(teachers.ToList());
            }
            else
            {
                return AdminLoginFaild();
            }

        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Teacher teacher = db.Teachers.Find(id);
                if (teacher == null)
                {
                    return HttpNotFound();
                }
                return View(teacher);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Teachers/Create
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

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher, HttpPostedFileBase PhotoPath2)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Password password = new Password();
                        int PasswordStatus = password.CheckStrength(teacher.Password);
                        var Exist = db.Teachers.Where(w => w.Email == teacher.Email).Count();
                        string PathAddress = "";
                        string PathAddress2 = "";
                        if (PhotoPath2 == null && PhotoPath2.ContentLength < 0)
                        {
                            ViewBag.Message = "Photo Not Acceptable";
                            //teacher.Password = "";
                            //teacher.ConfirmPassword = "";
                            return View(teacher);
                        }
                        else if (PasswordStatus < 4)
                        {
                            ViewBag.Message = "Password Not Strong";
                            return View(teacher);
                        }
                        else if (Exist > 0)
                        {
                            ViewBag.Message = "Email Address Has Already Existed";
                            teacher.Password = "";
                            teacher.ConfirmPassword = "";
                            return View(teacher);
                        }
                        else
                        {
                            //Photo Upload
                            RandomNumber _ran = new RandomNumber();
                            string Random = _ran.Random(10);
                            string Random2 = _ran.Random(10);
                            string _FileName = System.IO.Path.GetFileName(teacher.ContactNo+Random + Random2+PhotoPath2.FileName);
                            PathAddress = Path.Combine(Server.MapPath("~/ShareFiles/Teachers/"), _FileName);
                            PathAddress2 = "ShareFiles/Teachers/" + _FileName;
                            PhotoPath2.SaveAs(PathAddress);

                            //encript password here
                            byte[] bytes = Encoding.Unicode.GetBytes(teacher.Password);
                            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                            teacher.Password = Convert.ToBase64String(inArray);
                            teacher.ConfirmPassword = Convert.ToBase64String(inArray);

                            teacher.TeacherName = teacher.suffix + " " + teacher.TeacherFName + " " + teacher.TeacherLName;
                            teacher.PhotoPath = PathAddress2;

                            db.Teachers.Add(teacher);
                            db.SaveChanges();

                            Mail _mail = new Mail();
                            // _mail.SendMail(teacher.Email, "Account Created From IIT System", "Dear Faculty Member greeting From IIT, This email is now registered  to access IIT Course management System");

                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception)
                    {

                        ViewBag.Message = "Something Wrong";
                        teacher.Password = "";
                        teacher.ConfirmPassword = "";
                        return View(teacher);
                    }
                }
                return View(teacher);
            }
            else
            {
                return AdminLoginFaild();
            }
        }
        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Teacher teacher = db.Teachers.Find(id);
                if (teacher == null)
                {
                    return HttpNotFound();
                }
                return View(teacher);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teacher teacher)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    teacher.TeacherName = teacher.suffix + " " + teacher.TeacherFName + " " +
                                                    teacher.TeacherLName;
                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(teacher);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Teacher teacher = db.Teachers.Find(id);
                if (teacher == null)
                {
                    return HttpNotFound();
                }
                return View(teacher);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                Teacher teacher = db.Teachers.Find(id);
                db.Teachers.Remove(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        //Teacher login Action
        public ActionResult LoginTeacher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginTeacher(Teacher _teacher)
        {

            AccountDBContext DB = new AccountDBContext();

            if (_teacher.Email != null && _teacher.Password != null)
            {
                //encript password here
                byte[] bytes = Encoding.Unicode.GetBytes(_teacher.Password);
                byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                _teacher.Password = Convert.ToBase64String(inArray);
                var Teacher = DB.Teachers.Where(x => x.Email == _teacher.Email && x.Password == _teacher.Password).FirstOrDefault();
                if (Teacher != null)
                {
                    Session["TeacherName"] = Teacher.suffix+ " "+ Teacher.TeacherFName;
                    Session["TeacherId"] = Teacher.Id.ToString();
                    return RedirectToAction("FacultyView", "ShareContents");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password not matched");

                }
            }
            return View();
        }

        //Dashboard Teacher
        public ActionResult TeacherDashboard()
        {
            if (TeacherSessionControl())
            {
                return View();
            }
            else
            {
                return TeacherLoginFaild();
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return TeacherLoginFaild();
        }

//change Password

        public ActionResult ChangePassword()
        {
            if (TeacherSessionControl())
            {
                return View();
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword _change)
        {
            if (TeacherSessionControl())
            {
                if (ModelState.IsValid)
                {
                    Password password = new Password();
                    int PasswordStatus = password.CheckStrength(_change.NewPassword);

                    if (PasswordStatus >= 4)
                    {
                        var UserId = Convert.ToInt32(Session["TeacherId"]);
                        var TeacherInfo = db.Teachers.Where(x => x.Id == UserId).FirstOrDefault();

                        //encript password here
                        byte[] bytes = Encoding.Unicode.GetBytes(_change.OldPassword);
                        byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                        _change.OldPassword = Convert.ToBase64String(inArray);

                        if (TeacherInfo.Password == _change.OldPassword)
                        {
                            //encript password here
                            byte[] bytes2 = Encoding.Unicode.GetBytes(_change.NewPassword);
                            byte[] inArray2 = HashAlgorithm.Create("SHA1").ComputeHash(bytes2);
                            _change.NewPassword = Convert.ToBase64String(inArray2);

                            byte[] bytes3 = Encoding.Unicode.GetBytes(_change.ConfirmNewPassword);
                            byte[] inArray3 = HashAlgorithm.Create("SHA1").ComputeHash(bytes3);
                            _change.ConfirmNewPassword = Convert.ToBase64String(inArray3);

                            TeacherInfo.Password = _change.NewPassword;
                            TeacherInfo.ConfirmPassword = _change.ConfirmNewPassword;
                            db.SaveChanges();
                            ViewBag.Message = "Password has been changed";
                            Mail _mail = new Mail();
                            _mail.SendMail(TeacherInfo.Email, "Password Change", "Dear Faculty Member, Your IIT System Login Password Has Been Changed");
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = "Password Not Match ";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Password Not Strong ";
                        return View();
                    }

                }
                ViewBag.Message = "Enter passwords Again";
                return View();
            }
            else
            {
                return TeacherLoginFaild();
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
