using System;
using System.Collections.Generic;
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
    public class ShareContentsController : Controller
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
        public ActionResult Faculty()
        {
            if (TeacherSessionControl())
            {
                int TeacherSessionId = Convert.ToInt32(Session["TeacherId"]);
                var teacherShareContents = _ShareContentBll.FacultyShareContentViewModels(TeacherSessionId);
                ViewBag.teacherShareContents = teacherShareContents;
                return View();
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        // GET: ManagementShareContents
        public ActionResult Management()
        {
            if (TeacherSessionControl())
            {
                int TeacherSessionId = Convert.ToInt32(Session["TeacherId"]);
                var teacherShareContents = _ShareContentBll.FacultyShareContentViewModels(TeacherSessionId);
                ViewBag.teacherShareContents = teacherShareContents;
                return View();
            }
            else
            {
                return TeacherLoginFaild();
            }
        }
        // GET: TeacherShareContents/Details/5
        public ActionResult DetailsForFaculty(int? id)
        {
            if (TeacherSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ShareContent ShareContent = db.ShareContents.Find(id);
                if (ShareContent == null)
                {
                    return HttpNotFound();
                }
                return View(ShareContent);
            }
            else
            {
                return TeacherLoginFaild();
            }

        }

        // GET: TeacherShareContents/Create
        public ActionResult CreateByFaculty()
        {
            if (TeacherSessionControl())
            {
                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode");
                return View();
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        // POST: TeacherShareContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateByFaculty(ShareContent ShareContent, HttpPostedFileBase Myfile)
        {
            if (TeacherSessionControl())
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
                        string _FileName = System.IO.Path.GetFileName(ShareContent.DepartmentID+ShareContent.BatchId+Random + Random2+Myfile.FileName);
                        PathAddress = Path.Combine(Server.MapPath("~/ShareFiles"), _FileName);
                        PathAddress2 = "ShareFiles/" + _FileName;

                        Myfile.SaveAs(PathAddress);
                    }
                    ShareContent.PosterId = Convert.ToInt16(Session["TeacherId"]);
                    ShareContent.DateTime = DateTime.Now;
                    ShareContent.FilePath = PathAddress2;
                    db.ShareContents.Add(ShareContent);
                    db.SaveChanges();
                    // Mail _mail = new Mail();
                    // _mail.SendMail("evanhasan0810@gmail.com", "Test", "Test Mail From IIT");


                    return RedirectToAction("FacultyView", "ShareContents");
                }

                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode",ShareContent.DepartmentID);
                return View(ShareContent);
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        // GET: TeacherShareContents/Edit/5
        public ActionResult EditByFaculty(int? id)
        {
            if (TeacherSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ShareContent ShareContent = db.ShareContents.Find(id);
                if (ShareContent == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode",
                    ShareContent.DepartmentID);
                return View(ShareContent);
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        // POST: TeacherShareContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditByFaculty(ShareContent ShareContent)
        {
            if (TeacherSessionControl())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(ShareContent).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Faculty");
                }
                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode",
                    ShareContent.DepartmentID);
                return View(ShareContent);
            }
            else
            {
                return TeacherLoginFaild();
            }

        }

        // GET: TeacherShareContents/Delete/5
        public ActionResult DeleteByFaculty(int? id)
        {
            if (TeacherSessionControl())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ShareContent ShareContent = db.ShareContents.Find(id);
                if (ShareContent == null)
                {
                    return HttpNotFound();
                }
                return View(ShareContent);
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        // POST: TeacherShareContents/Delete/5
        [HttpPost, ActionName("DeleteByFaculty")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedByFaculty(int id)
        {
            if (TeacherSessionControl())
            {
                ShareContent ShareContent = db.ShareContents.Find(id);
                db.ShareContents.Remove(ShareContent);
                db.SaveChanges();
                return RedirectToAction("Faculty");
            }
            else
            {
                return TeacherLoginFaild();
            }
        }

        [HttpPost]
        public ActionResult DeleteFromWallFaculty(int id)
        {
            if (TeacherSessionControl())
            {
                ShareContent ShareContent = db.ShareContents.Find(id);
                db.ShareContents.Remove(ShareContent);
                db.SaveChanges();
                return RedirectToAction("FacultyView");
            }
            else
            {
                return AdminLoginFaild();
            }
        }

        //Teacher see the teacher content as in Wall
        public ActionResult FacultyView()
        {
            if (TeacherSessionControl())
            {
                int Id = Convert.ToInt32(Session["TeacherId"]);

                var _Content = _ShareContentBll.FacultyShareContentViewModelsWall(Id);

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

                RoutineBll _RoutineBll = new RoutineBll();
                ViewBag.RoomAssingList = _RoutineBll.RoomAssignViewModels(Id);
                ViewBag.ListOfAssignCourse = _RoutineBll.CourseDepartments(Id);
                ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentCode");
                return View();
            }
            return TeacherLoginFaild();
        }


        //student see the teacher content
        public ActionResult StudentView()
        {

            if (StudentSessionControl())
            {
                try
                {
                    int Id = Convert.ToInt32(Session["StudentId"]);

                    var _Student = db.Students.FirstOrDefault(x => x.Id == Id);
                    var _Batch = db.Batchs.FirstOrDefault(x => x.Id == _Student.BatchId);

                    var _Content = _ShareContentBll.StudentContentShowViewModels(_Student, _Batch);

                    if (_Content != null)
                    {
                        StudentBll _StudentBll = new StudentBll();
                        RoutineBll _RoutineBll = new RoutineBll();
                       //list of course
                        int UserId = Convert.ToInt32(Session["StudentId"]);
                        var StudentInfo = db.Students.Where(s => s.Id == UserId).FirstOrDefault();
                        var ListOfAssignCourse = _RoutineBll.CourseDepartments(StudentInfo);
                        //room assign list
                        var RoomAssingList = _RoutineBll.RoomAssignViewModels(StudentInfo);
                        ViewBag.RoomAssignList = RoomAssingList;
                        ViewBag.CourseAssignForStudent = ListOfAssignCourse;
                        //classmates
                       var StudentId = db.Students.Where(s => s.Id == UserId).FirstOrDefault();
                        ViewBag.StudentsList = _StudentBll.StudentsListForStudentShow(StudentId);
                        //Course list all
                        ViewBag.CourseListAll = db.Courses.Where(w => w.DepartmentId == StudentInfo.DepartmentId).ToList().OrderBy(x => x.Semester);
                        //post content
                        ViewBag.Content = _Content;
                    }
                    else
                    {
                        ViewBag.Content = null;
                    }
                    //get comments list
                    var CommentsList = _ShareContentBll.AllShareCommentViewModels();
                    ViewBag.Comments = CommentsList;
                    //get Thanks list
                    var ThanksList = db.ThanksButtons.ToList();
                    ViewBag.Thanks = ThanksList;
                    return View();
                }
                catch (Exception)
                {

                    RedirectToAction("Login", "Students");
                }
            }
            return StudentLoginFaild();

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
        //comment by json
        public JsonResult CommentAutoRefresh(int PostId)
        {        
       //get comments list
            var CommentsList = _ShareContentBll.JsonShareCommentViewModels(PostId);
            return Json(CommentsList, JsonRequestBehavior.AllowGet);

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