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
    public class AdminsController : Controller
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

        // GET: Admins
        public ActionResult Index()
        {

            if (AdminSessionControl())
            {
                return View(db.Admins.ToList());
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Admins/Create
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

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin _admin, HttpPostedFileBase PhotoPath2)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Password password = new Password();
                        int PasswordStatus = password.CheckStrength(_admin.Password);
                        var Exist = db.Admins.Where(w => w.Email == _admin.Email).Count();
                        string PathAddress = "";
                        string PathAddress2 = "";
                        if (PhotoPath2 == null && PhotoPath2.ContentLength < 0)
                        {
                            ViewBag.Message = "Photo Not Acceptable";
                            //teacher.Password = "";
                            //teacher.ConfirmPassword = "";
                            return View(PhotoPath2);
                        }
                        else if (PasswordStatus < 4)
                        {
                            ViewBag.Message = "Password Not Strong";
                            return View(PhotoPath2);
                        }
                        else if (Exist > 0)
                        {
                            ViewBag.Message = "Email Address Has Already Existed";
                            _admin.Password = "";
                            _admin.ConfirmPassword = "";
                            return View(_admin);
                        }
                        else
                        {
                            RandomNumber _ran=new RandomNumber();
                            string Random = _ran.Random(10);
                            //Photo Upload
                            string _FileName = System.IO.Path.GetFileName(_admin.LastName + Random + PhotoPath2.FileName);
                            PathAddress = Path.Combine(Server.MapPath("~/ShareFiles/Admins/"), _FileName);
                            PathAddress2 = "ShareFiles/Admins/" + _FileName;
                            PhotoPath2.SaveAs(PathAddress);

                            //encript password here
                            byte[] bytes = Encoding.Unicode.GetBytes(_admin.Password);
                            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                            _admin.Password = Convert.ToBase64String(inArray);
                            _admin.ConfirmPassword = Convert.ToBase64String(inArray);

                            _admin.PhotoPath = PathAddress2;

                            db.Admins.Add(_admin);
                            db.SaveChanges();

                            Mail _mail = new Mail();
                            // _mail.SendMail(teacher.Email, "Account Created From IIT System", "Dear Faculty Member greeting From IIT, This email is now registered  to access IIT Course management System");

                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception)
                    {

                        ViewBag.Message = "Something Wrong";
                        _admin.Password = "";
                        _admin.ConfirmPassword = "";
                        return View(_admin);
                    }
                }
                ViewBag.Message = "Input Data not valide";
                return View(_admin); 

            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(admin);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                Admin admin = db.Admins.Find(id);
                db.Admins.Remove(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return AdminLoginFaild();
            }

        }

        //login Action
        public ActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(Admin AdminUser)
        {
            if (AdminUser.Email != null && AdminUser.Password != null)
            { 
                //encript password here
                byte[] bytes = Encoding.Unicode.GetBytes(AdminUser.Password);
                byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                AdminUser.Password = Convert.ToBase64String(inArray);


                var AdminInfo = db.Admins.FirstOrDefault(x => x.Email == AdminUser.Email && x.Password == AdminUser.Password);
              
                if (AdminInfo != null)
                {
                    Session["Power"] = AdminInfo.Power;
                    Session["AdminId"] = AdminInfo.UserId;
                    Session["AdminName"] = AdminInfo.FirstName+" "+AdminInfo.LastName;
                    if (AdminInfo.Power == 1)
                        return RedirectToAction("AdminView", "ShareContentAdmin");

                    if (AdminInfo.Power == 2)
                        return RedirectToAction("AdminModerator", "Admins");

                    if (AdminInfo.Power == 3)
                        return RedirectToAction("AdminBatchCoordinator", "Admins");

                    if (AdminInfo.Power == 4)
                        return RedirectToAction("AdminStaff", "Admins");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password not matched");

                }
            }
            return View();

        }
        //Dashboard Admin
        public ActionResult AdminDashBoard()
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

        //Dashboard AdminModerator
        public ActionResult AdminModerator()
        {
            if (ModeratSessionControl())
            {
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        //Dashboard AdminModerator
        public ActionResult AdminBatchCoordinator()
        {
            if (BatchCoordinatorSessionControl())
            {
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }
        //Dashboard Staff
        public ActionResult AdminStaff()
        {
            if (StaffSessionControl())
            {
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }
        public ActionResult Logout()
        {
            Session["AdminId"] = null;
            Session["Power"] = null;
            return RedirectToAction("LoginAdmin", "Admins");
        }


        public ActionResult ChangePassword()
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

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword _change)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    Password password = new Password();
                    int PasswordStatus = password.CheckStrength(_change.NewPassword);

                    if (PasswordStatus >= 4)
                    {
                        var UserId = Convert.ToInt32(Session["AdminId"]);
                        var AdminInfo = db.Admins.Where(x => x.UserId == UserId).FirstOrDefault();

                        //encript password here
                        byte[] bytes = Encoding.Unicode.GetBytes(_change.OldPassword);
                        byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                        _change.OldPassword = Convert.ToBase64String(inArray);

                        if (AdminInfo.Password == _change.OldPassword)
                        {
                            //encript password here
                            byte[] bytes2 = Encoding.Unicode.GetBytes(_change.NewPassword);
                            byte[] inArray2 = HashAlgorithm.Create("SHA1").ComputeHash(bytes2);
                            _change.NewPassword = Convert.ToBase64String(inArray2);

                            byte[] bytes3 = Encoding.Unicode.GetBytes(_change.ConfirmNewPassword);
                            byte[] inArray3 = HashAlgorithm.Create("SHA1").ComputeHash(bytes3);
                            _change.ConfirmNewPassword = Convert.ToBase64String(inArray3);

                            AdminInfo.Password = _change.NewPassword;
                            AdminInfo.ConfirmPassword = _change.ConfirmNewPassword;
                            db.SaveChanges();
                            ViewBag.Message = "Password has been changed";
                            try
                            {
                                Mail _mail = new Mail();
                                _mail.SendMail(AdminInfo.Email, "Password Change", "Dear Admin Member, Your IIT System Login Password Has Been Changed");
                                return View();
                            }
                            catch (Exception)
                            {

                                ViewBag.Message = "Email Confirmation Not Send for Bad Internet Connection ";
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Old Password Not Match ";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = " Password Not Strong ";
                        return View();
                    }

                }
                ViewBag.Message = "Enter passwords Again";
                return View();
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
