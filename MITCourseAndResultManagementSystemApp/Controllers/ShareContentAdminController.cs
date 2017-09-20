using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MITCourseAndResultManagementSystemApp.BBL;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.Controllers
{
    public class ShareContentAdminController : Controller
    {
        ShareContentBLL _ShareContentBll = new ShareContentBLL();
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

        // GET: TeacherShareContents
        public ActionResult Admin()
        {
            if (AdminSessionControl())
            {
                int AdminSessionId = Convert.ToInt32(Session["AdminId"]);
                var ShareContents = _ShareContentBll.AdminShareContentViewModels(AdminSessionId);
                ViewBag.teacherShareContents = ShareContents;
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }


        // GET:ShareContents/Details/5
        public ActionResult Details(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ShareContent ShareContent = db.ShareContents.Find(id);
                if (ShareContent != null && ShareContent.Flag == 1)
                {
                    return View(ShareContent);
                }
                return HttpNotFound();

            }
            else
            {
                return AdminLoginFaild();
            }

        }

        // GET: TeacherShareContents/Create
        public ActionResult Create()
        {
            if (AdminSessionControl())
            {
                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode");
                return View();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: TeacherShareContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShareContent ShareContent, HttpPostedFileBase Myfile)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    string PathAddress = "";
                    string PathAddress2 = "";
                    if (Myfile != null)
                    {
                        RandomNumber _ran = new RandomNumber();
                        string Random = _ran.Random(10);
                        string Random2 = _ran.Random(8);
                        string _FileName = System.IO.Path.GetFileName(ShareContent.DepartmentID + ShareContent.BatchId + Random + Random2 + Myfile.FileName);
                        PathAddress = Path.Combine(Server.MapPath("~/ShareFiles"), _FileName);
                        PathAddress2 = "ShareFiles/" + _FileName;

                        Myfile.SaveAs(PathAddress);
                    }
                    //  ShareContent.Message = ShareContent.Message.Replace("[\r\n]", " <br /> ");
                    ShareContent.PosterId = Convert.ToInt16(Session["AdminId"]);
                    ShareContent.DateTime = DateTime.Now;
                    ShareContent.FilePath = PathAddress2;
                    ShareContent.Flag = 1;
                    db.ShareContents.Add(ShareContent);
                    db.SaveChanges();
                    Mail _mail = new Mail();
                    // _mail.SendMail("evanhasan0810@gmail.com", "Test", "Test Mail From IIT");

                    return RedirectToAction("AdminView", "ShareContentAdmin");
                }

                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode", ShareContent.DepartmentID);
                return View(ShareContent);
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // GET: TeacherShareContents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ShareContent ShareContent = db.ShareContents.Find(id);


                if (ShareContent != null && ShareContent.Flag == 1)
                {
                    ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode",
                   ShareContent.DepartmentID);
                    return View(ShareContent);
                }
                return HttpNotFound();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: TeacherShareContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShareContent ShareContent)
        {
            if (AdminSessionControl())
            {
                if (ModelState.IsValid)
                {
                    ShareContent.Flag = 1;
                    db.Entry(ShareContent).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }
                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode",
                    ShareContent.DepartmentID);
                return View(ShareContent);
            }
            else
            {
                return AdminLoginFaild();
            }

        }

        // GET: TeacherShareContents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AdminSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ShareContent ShareContent = db.ShareContents.Find(id);
                if (ShareContent != null && ShareContent.Flag == 1)
                {
                    return View(ShareContent);
                }
                return HttpNotFound();
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        // POST: TeacherShareContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (AdminSessionControl())
            {
                ShareContent ShareContent = db.ShareContents.Find(id);
                db.ShareContents.Remove(ShareContent);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            else
            {
                return AdminLoginFaild();
            }
        }
        [HttpPost]
        public ActionResult DeleteFromWall(int id)
        {
            if (AdminSessionControl())
            {

                ShareContent ShareContent = db.ShareContents.Find(id);
                db.ShareContents.Remove(ShareContent);
                db.SaveChanges();
                return RedirectToAction("AdminView");

            }
            else
            {
                return AdminLoginFaild();
            }
        }


        //Admin see the teacher content as in Wall
        public ActionResult AdminView()
        {
            if (AdminSessionControl())
            {
                int Id = Convert.ToInt32(Session["AdminId"]);

                var _Content = _ShareContentBll.AdminShareContentViewModelsWall(Id);
                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode");
                if (_Content != null)
                {
                    ViewBag.Content = _Content;
                }
                else
                {
                    ViewBag.Content = null;
                }
                //get comments list
                var CommentsList = _ShareContentBll.AllShareCommentViewModels();
                if (CommentsList != null)
                {
                    ViewBag.Comments = CommentsList;
                }
                else
                {
                    ViewBag.Comments = null;
                }

                //get Thanks list
                var ThanksList = db.ThanksButtons.ToList();
                ViewBag.Thanks = ThanksList;


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

                //Room Assign
                RoutineBll _RoutineBll = new RoutineBll();
                var RoomAssingList = _RoutineBll.RoomAssignViewModelsForAdmin();
                ViewBag.RoomAssignList = RoomAssingList;
               


                return View();
            }
            return AdminLoginFaild();
        }



        //comment by json
        public JsonResult CommentSubmitByClick(int PostId, int x, string Comment, int Flag)
        {
            ShareComment aShareComment = new ShareComment();
            aShareComment.Comment = Comment;
            aShareComment.ContentId = PostId;
            aShareComment.UserId = x;
            aShareComment.CommentTime = DateTime.Now;
            aShareComment.Flag = Flag;
            try
            {
                //save comments
                db.ShareComments.Add(aShareComment);
                db.SaveChanges();
            }
            catch (Exception)
            {
                RedirectToAction("Index", "Index");
            }
            //get comments list
            var CommentsList = _ShareContentBll.JsonShareCommentViewModels(aShareComment.ContentId);
            return Json(CommentsList, JsonRequestBehavior.AllowGet);

        }

        //comment Delete by json
        public JsonResult CommentDeletebyClick(int Commentid, int ContentId)
        {
            var list = db.ShareComments.Where(w => w.Id == Commentid).FirstOrDefault();
            if (list != null)
            {
                db.ShareComments.Remove(list);
                db.SaveChanges();
            }
            //get comments list
            var CommentsList = _ShareContentBll.JsonShareCommentViewModels(ContentId);
            return Json(CommentsList, JsonRequestBehavior.AllowGet);
        }


        //Thanks by json
        public JsonResult ThanksButtonClick(int PostId, int x, int Flag)
        {
            int Existance = db.ThanksButtons.Where(w => w.PostId == PostId && w.UserId == x && w.Flag == Flag).Count();

            if (Existance > 0)
            {
                var FindThanker = db.ThanksButtons.Where(w => w.PostId == PostId && w.UserId == x && w.Flag == Flag).FirstOrDefault();
                db.ThanksButtons.Remove(FindThanker);

                db.SaveChanges();
                int CountAll = db.ThanksButtons.Where(w => w.PostId == PostId).Count();
                //save thanks and count here 0==show Save and Button show thanks
                //make unThanks
                ThanksReturnValues _aValues = new ThanksReturnValues();
                _aValues.ThanksCount = CountAll;
                _aValues.Status = 0;
                return Json(_aValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //save thanks and count here 1==show Save and Button show Unthanks
                ThanksButton _thanks = new ThanksButton { Thanks = 1, UserId = x, PostId = PostId, Flag = Flag };
                db.ThanksButtons.Add(_thanks);
                db.SaveChanges();
                int CountAll = db.ThanksButtons.Where(w => w.PostId == PostId).Count();
                ThanksReturnValues _aValues = new ThanksReturnValues();
                _aValues.ThanksCount = CountAll;
                _aValues.Status = 1;

                return Json(_aValues, JsonRequestBehavior.AllowGet);
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