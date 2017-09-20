using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.BBL;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class ResetPasswordController : Controller
    {
        private AccountDBContext db = new AccountDBContext();
        // GET: ResetPassword
        public ActionResult FacultyResetRequest()
        {
            return View();
        }

        // GET: ResetPassword
        [HttpPost]
        public ActionResult FacultyResetRequest(string Email)
        {
            if (Email!=null)
            {
                ResetPassword _reset=new ResetPassword();
                int CheckExistance = db.Teachers.Where(x => x.Email == Email).Select(y => y.Id).FirstOrDefault();

                if (CheckExistance >0)
                {
                    //make 2 random Number
                  
                         RandomNumber _ran=new RandomNumber();
                         string RandomNumber1 = _ran.Random(10);
                         string RandomNumber2 = _ran.Random(8);

                   UserIPAddress _ipAddress=new UserIPAddress();
                    _reset.UserIP=_ipAddress.GetUserIp();
                    _reset.Email = Email;
                    _reset.RandomNumber = RandomNumber1;
                    _reset.RandomNumber2 = RandomNumber2;
                    _reset.DateTime = DateTime.Now;
                    _reset.AccountId = CheckExistance;
                    db.ResetPassword.Add(_reset);
                    db.SaveChanges();

                    string Sub = "Reset Password";
                    string Link = "http://localhost:56969/ResetPassword/FacultyReset?Id1=" + RandomNumber1 + "&Id2=" +
                                     RandomNumber2;
                    string Message = "<a href='"+Link+"'><b>localhost:56969/ResetPassword/FacultyReset?Id1=" + RandomNumber1 + "&Id2=" +
                                     RandomNumber2 + "</b></a> : Ip Address : "+_reset.UserIP;

                    try
                    {
                        Mail _mail = new Mail();
                        _mail.SendMail(_reset.Email, Sub, Message);
                    }
                    catch (Exception)
                    {
                        ViewBag.Message = "Internet Not Supporting at this time try again later";
                        return View();
                    }
                  
                    ViewBag.Message = "Check Your Email You have received a Link to reset your password";
                    return View();
                }
            }
            ViewBag.Message = "Enter Valid Email";
            return View();
        }

        [HttpGet]
        public ActionResult FacultyReset(string Id1, string Id2)
        {
            var Check = db.ResetPassword.Where(x => x.RandomNumber == Id1 && x.RandomNumber2 == Id2).FirstOrDefault();
            if (Check != null)
            {
                ViewBag.AccountId = Check.AccountId;
                return View();
            }
            else
            {
                ViewBag.Message = "This Action Removed Reset Again";
                return RedirectToAction("FacultyResetRequist", "ResetPassword");
            }

        }

        [HttpPost]
        public ActionResult FacultyReset(ChangePassword _change)
        {
            if (_change.NewPassword!=null && _change.ConfirmNewPassword!=null &&_change.UserId!=null)
            {
                Password password = new Password();
                int PasswordStatus = password.CheckStrength(_change.NewPassword);

                if (PasswordStatus >= 4)
                {
                   var TeacherInfo = db.Teachers.Where(x => x.Id == _change.UserId).FirstOrDefault();

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
                    try
                    {
                        Mail _mail = new Mail();
                        _mail.SendMail(TeacherInfo.Email, "Password Change",
                            "Dear Faculty Member, Your IIT System Login Password Has Been Changed");
                    }
                    catch (Exception)
                    {
                        ViewBag.Message = "Password has been changed but will not get email confirmation because of bad Internet connection ";
                        return View();
                    }

                 var ResetList=   db.ResetPassword.Where(x => x.AccountId == _change.UserId).ToList();
                    foreach (var item in ResetList)
                    {
                        item.RandomNumber = "xxx";
                    }
                    db.SaveChanges();
                        ViewBag.Message = "Password Reseted ";
                        return View();
                  }
                else
                {
                    ViewBag.Message = "Password Not Strong ";
                    return View();
                }
            }
            ViewBag.Message = "Password Wrong ";
            return View();
        }

//Admin Area
        // GET: ResetPassword
        public ActionResult AdminResetRequest()
        {
            return View();
        }

        //ResetPassword
        [HttpPost]
        public ActionResult AdminResetRequest(string Email)
        {
            if (Email != null)
            {
                ResetPassword _reset = new ResetPassword();
                int CheckExistance = db.Admins.Where(x => x.Email == Email).Select(y => y.UserId).FirstOrDefault();

                if (CheckExistance > 0)
                {
                    //make 2 random Number
                    var random = new Random();
                    string RandomNumber1 = string.Empty;
                    for (int i = 0; i < 10; i++)
                        RandomNumber1 = String.Concat(RandomNumber1, random.Next(10).ToString());

                    string RandomNumber2 = string.Empty;
                    for (int i = 0; i < 8; i++)
                        RandomNumber2 = String.Concat(RandomNumber2, random.Next(8).ToString());


                    UserIPAddress _ipAddress = new UserIPAddress();
                    _reset.UserIP = _ipAddress.GetUserIp();
                    _reset.Email = Email;
                    _reset.RandomNumber = RandomNumber1;
                    _reset.RandomNumber2 = RandomNumber2;
                    _reset.DateTime = DateTime.Now;
                    _reset.AccountId = CheckExistance;
                    db.ResetPassword.Add(_reset);
                    db.SaveChanges();

                    string Sub = "Reset Password";
                    string Link = "http://localhost:56969/ResetPassword/AdminReset?Id1=" + RandomNumber1 + "&Id2=" +
                                     RandomNumber2;
                    string Message = "<a href='" + Link + "'><b>localhost:56969/ResetPassword/FacultyReset?Id1=" + RandomNumber1 + "&Id2=" +
                                     RandomNumber2 + "</b></a> : Ip Address : " + _reset.UserIP;

                    try
                    {
                        Mail _mail = new Mail();
                        _mail.SendMail(_reset.Email, Sub, Message);
                    }
                    catch (Exception)
                    {
                        ViewBag.Message = "Internet Not Supporting at this time try again later";
                        return View();
                    }

                    ViewBag.Message = "Check Your Email You have received a Link to reset your password";
                    return View();
                }
            }
            ViewBag.Message = "Enter Valid Email";
            return View();
        }

        [HttpGet]
        public ActionResult AdminReset(string Id1, string Id2)
        {
            var Check = db.ResetPassword.Where(x => x.RandomNumber == Id1 && x.RandomNumber2 == Id2).FirstOrDefault();
            if (Check != null)
            {
                ViewBag.AccountId = Check.AccountId;
                return View();
            }
            else
            {
                ViewBag.Message = "This Action Removed Reset Again";
                return RedirectToAction("AdminResetRequist", "ResetPassword");
            }
        }

        [HttpPost]
        public ActionResult AdminReset(ChangePassword _change)
        {
            if (_change.NewPassword != null && _change.ConfirmNewPassword != null && _change.UserId != null)
            {
                Password password = new Password();
                int PasswordStatus = password.CheckStrength(_change.NewPassword);

                if (PasswordStatus >= 4)
                {
                    var AdninInfo = db.Admins.Where(x => x.UserId == _change.UserId).FirstOrDefault();

                    //encript password here
                    byte[] bytes2 = Encoding.Unicode.GetBytes(_change.NewPassword);
                    byte[] inArray2 = HashAlgorithm.Create("SHA1").ComputeHash(bytes2);
                    _change.NewPassword = Convert.ToBase64String(inArray2);

                    byte[] bytes3 = Encoding.Unicode.GetBytes(_change.ConfirmNewPassword);
                    byte[] inArray3 = HashAlgorithm.Create("SHA1").ComputeHash(bytes3);
                    _change.ConfirmNewPassword = Convert.ToBase64String(inArray3);

                    AdninInfo.Password = _change.NewPassword;
                    AdninInfo.ConfirmPassword = _change.ConfirmNewPassword;
                    db.SaveChanges();
                    ViewBag.Message = "Password has been changed";
                    try
                    {
                        Mail _mail = new Mail();
                        _mail.SendMail(AdninInfo.Email, "Password Change",
                            "Dear Admin, Your IIT System Login Password Has Been Changed");
                    }
                    catch (Exception)
                    {
                        ViewBag.Message = "Password has been changed but will not get email confirmation because of bad Internet connection ";
                        return View();
                    }

                    var ResetList = db.ResetPassword.Where(x => x.AccountId == _change.UserId).ToList();
                    foreach (var item in ResetList)
                    {
                        item.RandomNumber = "xxx";
                    }
                    db.SaveChanges();
                    ViewBag.Message = "Password Reseted ";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Password Not Strong ";
                    return View();
                }
            }
            ViewBag.Message = "Password Wrong ";
            return View();
        }

 ////Student Area
  
        // GET: ResetPassword
        public ActionResult StudentResetRequest()
        {
            return View();
        }

        // GET: ResetPassword
        [HttpPost]
        public ActionResult StudentResetRequest(string Email)
        {
            if (Email != null)
            {
                ResetPassword _reset = new ResetPassword();
                int CheckExistance = db.Students.Where(x => x.Email == Email).Select(y => y.Id).FirstOrDefault();

                if (CheckExistance > 0)
                {
                    //make 2 random Number
                    var random = new Random();
                    string RandomNumber1 = string.Empty;
                    for (int i = 0; i < 10; i++)
                        RandomNumber1 = String.Concat(RandomNumber1, random.Next(10).ToString());

                    string RandomNumber2 = string.Empty;
                    for (int i = 0; i < 8; i++)
                        RandomNumber2 = String.Concat(RandomNumber2, random.Next(8).ToString());

                    UserIPAddress _ipAddress = new UserIPAddress();
                    _reset.UserIP = _ipAddress.GetUserIp();
                    _reset.Email = Email;
                    _reset.RandomNumber = RandomNumber1;
                    _reset.RandomNumber2 = RandomNumber2;
                    _reset.DateTime = DateTime.Now;
                    _reset.AccountId = CheckExistance;
                    db.ResetPassword.Add(_reset);
                    db.SaveChanges();

                    string Sub = "Reset Password";
                    string Link = "http://localhost:56969/ResetPassword/StudentReset?Id1=" + RandomNumber1 + "&Id2=" +
                                     RandomNumber2;
                    string Message = "<a href='" + Link + "'><b>localhost:56969/ResetPassword/FacultyReset?Id1=" + RandomNumber1 + "&Id2=" +
                                     RandomNumber2 + "</b></a> : Ip Address : " + _reset.UserIP;
                    try
                    {
                        Mail _mail = new Mail();
                        _mail.SendMail(_reset.Email, Sub, Message);
                    }
                    catch (Exception)
                    {
                        ViewBag.Message = "Internet Not Supporting at this time try again later";
                        return View();
                    }
                   

                    ViewBag.Message = "Check Your Email You have received a Link to reset your password";
                    return View();
                }
            }
            ViewBag.Message = "Enter Valid Email";
            return View();
        }

        [HttpGet]
        public ActionResult StudentReset(string Id1, string Id2)
        {
            var Check = db.ResetPassword.Where(x => x.RandomNumber == Id1 && x.RandomNumber2 == Id2).FirstOrDefault();
            if (Check != null)
            {
                ViewBag.AccountId = Check.AccountId;
                return View();
            }
            else
            {
                ViewBag.Message = "This Action Removed Reset Again";
                return RedirectToAction("FacultyResetRequist", "ResetPassword");
            }

        }

        [HttpPost]
        public ActionResult StudentReset(ChangePassword _change)
        {
            if (_change.NewPassword != null && _change.ConfirmNewPassword != null && _change.UserId != null)
            {
                Password password = new Password();
                int PasswordStatus = password.CheckStrength(_change.NewPassword);

                if (PasswordStatus >= 4)
                {
                    var StudentInfo = db.Students.Where(x => x.Id == _change.UserId).FirstOrDefault();

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
                    try
                    {
                        Mail _mail = new Mail();
                        _mail.SendMail(StudentInfo.Email, "Password Change",
                            "Dear Student, Your IIT System Login Password Has Been Changed");
                    }
                    catch (Exception)
                    {
                        ViewBag.Message = "Password has been changed but will not get email confirmation because of bad Internet connection ";
                        return View();
                    }

                    var ResetList = db.ResetPassword.Where(x => x.AccountId == _change.UserId).ToList();
                    foreach (var item in ResetList)
                    {
                        item.RandomNumber = "xxx";
                    }
                    db.SaveChanges();
                    ViewBag.Message = "Password Reseted ";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Password Not Strong ";
                    return View();
                }
            }
            ViewBag.Message = "Password Wrong ";
            return View();
        }
    }
}