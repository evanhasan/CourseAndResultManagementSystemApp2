using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class ShareContentBLL
    {
        private AccountDBContext db = new AccountDBContext();

        public List<ShareCommentViewModel> AllShareCommentViewModels()
        {
            //get comments list
            //"1" means Teacher comment
            List<ShareCommentViewModel> CommentsList1 = db.ShareComments.Where(w => w.Flag == 1)
                .Join(db.Teachers, cm => cm.UserId, st => st.Id, (cm, st) => new { cm, st })
                .Select(z => new ShareCommentViewModel
                {
                    CommentId = z.cm.Id,
                    Comment = z.cm.Comment,
                    UserId = z.cm.UserId,
                    CommentTime = z.cm.CommentTime.ToString(),
                    ContentId = z.cm.ContentId,
                    UserName = z.st.TeacherName,
                    Flag = z.cm.Flag,
                    PhotoPath = z.st.PhotoPath
                }).ToList();

            //"0" means Student comment
            List<ShareCommentViewModel> CommentsList2 = db.ShareComments.Where(w => w.Flag == 0)
                  .Join(db.Students, cm => cm.UserId, st => st.Id, (cm, st) => new { cm, st })
                      .Select(z => new ShareCommentViewModel
                      {
                          CommentId = z.cm.Id,
                          Comment = z.cm.Comment,
                          UserId = z.cm.UserId,
                          CommentTime = z.cm.CommentTime.ToString(),
                          ContentId = z.cm.ContentId,
                          UserName = z.st.Name,
                          Flag = z.cm.Flag,
                          PhotoPath = z.st.PhotoPath
                      }).ToList();
           
            //"2" means Admin comment
            List<ShareCommentViewModel> CommentsList3 = db.ShareComments.Where(w => w.Flag == 2)
                  .Join(db.Admins, cm => cm.UserId, st => st.UserId, (cm, st) => new { cm, st })
                      .Select(z => new ShareCommentViewModel
                      {
                          CommentId = z.cm.Id,
                          Comment = z.cm.Comment,
                          UserId = z.cm.UserId,
                          CommentTime = z.cm.CommentTime.ToString(),
                          ContentId = z.cm.ContentId,
                          UserName = z.st.FirstName+z.st.LastName,
                          Flag = z.cm.Flag,
                          PhotoPath = z.st.PhotoPath
                      }).ToList();

            List<ShareCommentViewModel> CommentsList = new List<ShareCommentViewModel>();

            CommentsList = CommentsList2.Union(CommentsList1).ToList();
            CommentsList = CommentsList.Union(CommentsList3).ToList().OrderBy(x => x.CommentId).ToList();
            return CommentsList;
        }


        public List<ShareCommentViewModel> JsonShareCommentViewModels(int ContentId)
        {
            List<ShareCommentViewModel> CommentsList = null;

            List<ShareCommentViewModel> CommentsList1 = db.ShareComments.Where(w => w.Flag == 1 && w.ContentId == ContentId)
                  .Join(db.Teachers, cm => cm.UserId, st => st.Id, (cm, st) => new { cm, st })
                  .Select(z => new ShareCommentViewModel
                  {
                      CommentId = z.cm.Id,
                      Comment = z.cm.Comment,
                      UserId = z.cm.UserId,
                      CommentTime = z.cm.CommentTime.ToString(),
                      ContentId = z.cm.ContentId,
                      UserName = z.st.TeacherName,
                      Flag = z.cm.Flag,
                      PhotoPath = z.st.PhotoPath
                  }).ToList();

            //"0" means Student comment
            List<ShareCommentViewModel> CommentsList2 = db.ShareComments.Where(w => w.Flag == 0 && w.ContentId == ContentId)
                  .Join(db.Students, cm => cm.UserId, st => st.Id, (cm, st) => new { cm, st })
                      .Select(z => new ShareCommentViewModel
                      {
                          CommentId = z.cm.Id,
                          Comment = z.cm.Comment,
                          UserId = z.cm.UserId,
                          CommentTime = z.cm.CommentTime.ToString(),
                          ContentId = z.cm.ContentId,
                          UserName = z.st.Name,
                          Flag = z.cm.Flag,
                          PhotoPath = z.st.PhotoPath
                      }).ToList();

            //"2" means Admin comment
            List<ShareCommentViewModel> CommentsList3 = db.ShareComments.Where(w => w.Flag == 2 && w.ContentId == ContentId)
                  .Join(db.Admins, cm => cm.UserId, st => st.UserId, (cm, st) => new { cm, st })
                      .Select(z => new ShareCommentViewModel
                      {
                          CommentId = z.cm.Id,
                          Comment = z.cm.Comment,
                          UserId = z.cm.UserId,
                          CommentTime = z.cm.CommentTime.ToString(),
                          ContentId = z.cm.ContentId,
                          UserName = z.st.FirstName+z.st.LastName,
                          Flag = z.cm.Flag,
                          PhotoPath = z.st.PhotoPath
                      }).ToList();
            CommentsList = CommentsList2.Union(CommentsList1).ToList();
            CommentsList = CommentsList.Union(CommentsList3).ToList().OrderBy(x => x.CommentId).ToList();
            return CommentsList;
        }


        //student will see in the wall
        public List<StudentContentShowViewModel> StudentContentShowViewModels(Student _Student, Batch _Batch)
        {
            //teacher post
            var _Content1 =
                db.ShareContents.Where(r => r.DepartmentID == _Student.DepartmentId && r.BatchId == _Batch.Id && r.Flag==0)
                    .Join(db.Teachers, r2 => r2.PosterId, cc => cc.Id, (r2, cc) => new { r2, cc })
                    .Select(
                        x => new StudentContentShowViewModel
                        {
                            Title = x.r2.Title,
                            Message = x.r2.Message,
                            DateTime = x.r2.DateTime,
                            TeacherName = x.cc.TeacherName,
                            Designation = x.cc.Designation,
                            Id = x.r2.Id,
                            FilePath = x.r2.FilePath,
                            PhotoPath = x.cc.PhotoPath
                        }).ToList().OrderByDescending(x => x.DateTime);

            //Adminpost
            var _Content2 =
               db.ShareContents.Where(r => r.DepartmentID == _Student.DepartmentId && r.BatchId == _Batch.Id && r.Flag==1)
                   .Join(db.Admins, r2 => r2.PosterId, cc => cc.UserId, (r2, cc) => new { r2, cc })
                   .Select(
                       x => new StudentContentShowViewModel
                       {
                           Title = x.r2.Title,
                           Message = x.r2.Message,
                           DateTime = x.r2.DateTime,
                           TeacherName = x.cc.FirstName + x.cc.LastName,
                           Designation = x.cc.Designation,
                           Id = x.r2.Id,
                           FilePath = x.r2.FilePath,
                           PhotoPath = x.cc.PhotoPath
                       }).ToList().OrderByDescending(x => x.DateTime);

            //StudentContentShowViewModel _Content = null;
            var _Content = _Content1.Union(_Content2).ToList().OrderByDescending(x => x.DateTime).ToList();
            return _Content;
        }
//facult wall
        public IOrderedEnumerable<TeacherShareContentViewModel> FacultyShareContentViewModelsWall(int Id)
        {
            var _Content =
                db.ShareContents.Where(r => r.PosterId == Id)
                    .Join(db.Teachers, r2 => r2.PosterId, cc => cc.Id, (r2, cc) => new { r2, cc })
                    .Join(db.Departments, r3 => r3.r2.DepartmentID, d => d.Id, (r3, d) => new { r3, d })
                    .Join(db.Batchs, r4 => r4.r3.r2.BatchId, b => b.Id, (r4, b) => new { r4, b })
                    .Select(
                        x => new TeacherShareContentViewModel
                        {
                            Title = x.r4.r3.r2.Title,
                            Message = x.r4.r3.r2.Message,
                            DateTime = x.r4.r3.r2.DateTime,
                            TeacherName = x.r4.r3.cc.TeacherName,
                            Designation = x.r4.r3.cc.Designation,
                            Id = x.r4.r3.r2.Id,
                            FilePath = x.r4.r3.r2.FilePath,
                            PhotoPath = x.r4.r3.cc.PhotoPath,
                            Department = x.r4.d.DepartmentCode,
                            BatchNumber = x.b.BatchNo

                        }).ToList().OrderByDescending(x => x.DateTime);
            return _Content;
        }

//faculty member Post list view
        public List<TeacherShareContentViewModel> FacultyShareContentViewModels(int TeacherSessionId)
        {
            List<TeacherShareContentViewModel> ShareContents =
                db.ShareContents.Where(x => x.PosterId == TeacherSessionId && x.Flag==0)
                    .Join(db.Batchs, ts => ts.BatchId, b => b.Id, (ts, b) => new { ts, b })
                    .Join(db.Teachers, ts2 => ts2.ts.PosterId, t => t.Id, (ts2, t) => new { ts2, t })
                    .Join(db.Departments, ts3 => ts3.ts2.ts.DepartmentID, d => d.Id, (ts3, d) => new { ts3, d })
                    .Select(z => new TeacherShareContentViewModel()
                    {
                        Title = z.ts3.ts2.ts.Title,
                        Message = z.ts3.ts2.ts.Message,
                        FilePath = z.ts3.ts2.ts.FilePath,
                        DateTime = z.ts3.ts2.ts.DateTime,
                        TeacherName = z.ts3.t.TeacherName,
                        Designation = z.ts3.t.Designation,
                        Department = z.d.DepartmentName,
                        BatchNumber = z.ts3.ts2.b.BatchNo,
                        PhotoPath = z.ts3.t.PhotoPath,
                        Id = z.ts3.ts2.ts.Id
                    }).ToList();
            return ShareContents;
        }

        //Admin member Post list view
        public List<TeacherShareContentViewModel> AdminShareContentViewModels(int TeacherSessionId)
        {
            List<TeacherShareContentViewModel> ShareContents =
                db.ShareContents.Where(x => x.PosterId == TeacherSessionId && x.Flag==1)
                    .Join(db.Batchs, ts => ts.BatchId, b => b.Id, (ts, b) => new { ts, b })
                    .Join(db.Admins, ts2 => ts2.ts.PosterId, t => t.UserId, (ts2, t) => new { ts2, t })
                    .Join(db.Departments, ts3 => ts3.ts2.ts.DepartmentID, d => d.Id, (ts3, d) => new { ts3, d })
                    .Select(z => new TeacherShareContentViewModel()
                    {
                        Title = z.ts3.ts2.ts.Title,
                        Message = z.ts3.ts2.ts.Message,
                        FilePath = z.ts3.ts2.ts.FilePath,
                        DateTime = z.ts3.ts2.ts.DateTime,
                        TeacherName = z.ts3.t.FirstName+z.ts3.t.LastName,
                        Designation = z.ts3.t.Designation,
                        Department = z.d.DepartmentName,
                        BatchNumber = z.ts3.ts2.b.BatchNo,
                        PhotoPath = z.ts3.t.PhotoPath,
                        Id = z.ts3.ts2.ts.Id
                    }).ToList().OrderByDescending(x=>x.DateTime).ToList();
            return ShareContents;
        }

        //Admin wall
        public IOrderedEnumerable<TeacherShareContentViewModel> AdminShareContentViewModelsWall(int Id)
        {
            var _Content =
                db.ShareContents.Where(r => r.PosterId == Id && r.Flag==1)
                    .Join(db.Admins, r2 => r2.PosterId, cc => cc.UserId, (r2, cc) => new { r2, cc })
                    .Join(db.Departments, r3 => r3.r2.DepartmentID, d => d.Id, (r3, d) => new { r3, d })
                    .Join(db.Batchs, r4 => r4.r3.r2.BatchId, b => b.Id, (r4, b) => new { r4, b })
                    .Select(
                        x => new TeacherShareContentViewModel
                        {
                            Title = x.r4.r3.r2.Title,
                            Message = x.r4.r3.r2.Message,
                            DateTime = x.r4.r3.r2.DateTime,
                            TeacherName = x.r4.r3.cc.FirstName + x.r4.r3.cc.LastName,
                            Designation = x.r4.r3.cc.Designation,
                            Id = x.r4.r3.r2.Id,
                            FilePath = x.r4.r3.r2.FilePath,
                           PhotoPath = x.r4.r3.cc.PhotoPath,
                            Department = x.r4.d.DepartmentCode,
                            BatchNumber = x.b.BatchNo

                        }).ToList().OrderByDescending(x => x.DateTime);
            return _Content;
        }

    }
}