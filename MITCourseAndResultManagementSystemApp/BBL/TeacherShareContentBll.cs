//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using MITCourseAndResultManagementSystemApp.Models;
//using MITCourseAndResultManagementSystemApp.Models.Context;
//using MITCourseAndResultManagementSystemApp.Models.ViewModel;

//namespace MITCourseAndResultManagementSystemApp.BBL
//{
//    public class TeacherShareContentBll
//    {
//        private AccountDBContext db = new AccountDBContext();
//        //get comment list
//        public List<ShareCommentViewModel> ShareCommentViewModels()
//        {
//           //get comments list
//            List<ShareCommentViewModel> CommentsList =
//                db.ShareComments.Join(db.Students, cm => cm.UserId, st => st.Id, (cm, st) => new { cm, st })
//                    .Select(z => new ShareCommentViewModel
//                    {
//                        CommentId = z.cm.Id,
//                        Comment = z.cm.Comment,
//                        UserId = z.cm.UserId,
//                        CommentTime = z.cm.CommentTime.ToString(),
//                        ContentId = z.cm.ContentId,
//                        UserName = z.st.Name,
//                        Flag = z.cm.Flag
                        
//                    }).ToList();
//            return CommentsList;
            
//        }
//        //student see the content/post in wall
//        public IOrderedEnumerable<StudentContentShowViewModel> StudentContentShowViewModels(Student _Student, Batch _Batch)
//        {
//            var _Content =
//                db.TeacherShareContents.Where(r => r.DepartmentID == _Student.DepartmentId && r.BatchId == _Batch.Id)
//                    .Join(db.Teachers, r2 => r2.TeacherId, cc => cc.Id, (r2, cc) => new { r2, cc })
//                    .Select(
//                        x => new StudentContentShowViewModel
//                        {
//                            Title = x.r2.Title,
//                            Message = x.r2.Message,
//                            DateTime = x.r2.DateTime,
//                            TeacherName = x.cc.TeacherName,
//                            Designation = x.cc.Designation,
//                            Id = x.r2.Id,
//                            FilePath = x.r2.FilePath,
//                            PhotoPath = x.cc.PhotoPath
//                        }).ToList().OrderByDescending(x => x.DateTime);
//            return _Content;
//        }

//        //Teacher see the content/post in wall
//        public IOrderedEnumerable<TeacherShareContentViewModel> WallShareContentViewModels(int Id)
//        {
//            var _Content =
//                db.TeacherShareContents.Where(r => r.TeacherId == Id)
//                    .Join(db.Teachers, r2 => r2.TeacherId, cc => cc.Id, (r2, cc) => new { r2, cc })
//                    .Select(
//                        x => new TeacherShareContentViewModel
//                        {
//                            Title = x.r2.Title,
//                            Message = x.r2.Message,
//                            DateTime = x.r2.DateTime,
//                            TeacherName = x.cc.TeacherName,
//                            Designation = x.cc.Designation,
//                            Id = x.r2.Id,
//                            FilePath = x.r2.FilePath,
//                            PhotoPath = x.cc.PhotoPath
//                        }).ToList().OrderByDescending(x => x.DateTime);
//            return _Content;
//        }
      
//        //for Faculty or Admin who are posted list of posts
//        public List<TeacherShareContentViewModel> ShareContentViewModels(int TeacherSessionId)
//        {
//            //Flag 1 for Admins Flag 0 for Teachers
//            List<TeacherShareContentViewModel> ShareContents =
//                db.ShareContents.Where(x => x.PosterId == TeacherSessionId)
//                    .Join(db.Batchs, ts => ts.BatchId, b => b.Id, (ts, b) => new { ts, b })
//                    .Join(db.Teachers, ts2 => ts2.ts.PosterId, t => t.Id, (ts2, t) => new { ts2, t })
//                    .Join(db.Departments, ts3 => ts3.ts2.ts.DepartmentID, d => d.Id, (ts3, d) => new { ts3, d })
//                    .Select(z => new TeacherShareContentViewModel()
//                    {
//                        Title = z.ts3.ts2.ts.Title,
//                        Message = z.ts3.ts2.ts.Message,
//                        FilePath = z.ts3.ts2.ts.FilePath,
//                        DateTime = z.ts3.ts2.ts.DateTime,
//                        TeacherName = z.ts3.t.TeacherName,
//                        Designation = z.ts3.t.Designation,
//                        Department = z.d.DepartmentName,
//                        BatchNumber = z.ts3.ts2.b.BatchNo,
//                        PhotoPath = z.ts3.t.PhotoPath,
//                        Id = z.ts3.ts2.ts.Id
//                    }).ToList();
//            return ShareContents;
//        }
//    }
//}