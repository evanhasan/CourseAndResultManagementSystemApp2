using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.BBL;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class MasterUserController : Controller
    {
        // GET: MasterUserRegistration
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                AccountDBContext DB = new AccountDBContext();
                return View(DB.MasterUserAccounts.ToList());
            }
            else
            {
                return RedirectToAction("LoginMaster", "MasterUser");
            }
        }

        public ActionResult Register()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginMaster", "MasterUser");
            }
            
        }
        [HttpPost]
        public ActionResult Register(MasterUserAccount account)
        {
            AccountDBContext DB = new AccountDBContext();
            var isExist = DB.MasterUserAccounts.Count(x => x.Email == account.Email);
            Password password = new Password();
            int PasswordStatus = password.CheckStrength(account.Password);
            if (ModelState.IsValid)
            {
                if (PasswordStatus >= 4)
                {
                    if (isExist == 0)
                    {
                        //encript password here
                        byte[] bytes = Encoding.Unicode.GetBytes(account.Password);
                        byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                        account.Password = Convert.ToBase64String(inArray);
                        account.ConfirmPassword = Convert.ToBase64String(inArray);
                        DB.MasterUserAccounts.Add(account);
                        DB.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Message = "User" + account.FirstName + " " + account.LastName + " is Successfully Registered";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "User Already Exist";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Password Not Strong";
                    return View();
                }
               

            }
            return View();
        }
        //login Action
        public ActionResult LoginMaster()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginMaster(MasterUserAccount user)
        {

            AccountDBContext DB = new AccountDBContext();

            if (user.Email!=null && user.Password!=null)
            {
                
                var Master = DB.MasterUserAccounts.SingleOrDefault(x => x.Email == user.Email && x.Password == user.Password);
                if (Master != null)
                {
                    Session["Email"] = Master.Email.ToString();
                    Session["UserId"] = Master.UserId.ToString();
                    return RedirectToAction("MasterPage");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password not matched");
                   
                }
            }
            return View();
        }
        //Dashboard master
        public ActionResult MasterPage()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginMaster");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("LoginMaster", "MasterUser");
        }
    }
}