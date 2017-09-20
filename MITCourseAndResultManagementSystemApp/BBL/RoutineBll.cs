using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class RoutineBll
    {
        private AccountDBContext db = new AccountDBContext();

        public List<CourseDepartment> CourseDepartments(Student StudentInfo)
        {
            List<CourseDepartment> ListOfAssignCourse =
                db.CourseAssigns.Where(w => w.Flag == 1)
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
                                DepartmentId = z.x4.x3.x2.x.DepartmentId,
                                BatchNumber = z.x4.bb.BatchNo,
                                BatchId = z.x4.x3.x2.x.BatchId,
                                CourseName = z.x4.x3.cc.CourseName,
                                Semester = z.x4.x3.cc.Semester,
                                TeacherName = z.x4.x3.x2.tt.TeacherName,
                                Credit = z.x4.x3.x2.x.Credit,
                                Flag = z.x4.x3.x2.x.Flag,
                            })
                    .Where(w => w.DepartmentId == StudentInfo.DepartmentId && w.BatchId == StudentInfo.BatchId)
                    .OrderByDescending(x => x.Semester).ToList();
            return ListOfAssignCourse;
        }

        public List<CourseDepartment> CourseDepartments(int TeacherId)
        {
            List<CourseDepartment> ListOfAssignCourse =
                db.CourseAssigns.Where(xx => xx.TeacherId == TeacherId && xx.Flag == 1)
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
                                Credit = z.x4.x3.x2.x.Credit
                            }).OrderBy(x => x.BatchNumber).ToList();
            return ListOfAssignCourse;
        }


        public List<RoomAssignViewModel> RoomAssignViewModels(int TeacherId)
        {
            List<RoomAssignViewModel> RoomAssingList = db.RoomAssigns.Where(w => w.TeacherId == TeacherId)
                .Join(db.Batchs, ra => ra.BatchId, b => b.Id, (ra, b) => new { ra, b })
                .Join(db.Courses, ra2 => ra2.ra.CourseId, c => c.Id, (ra2, c) => new { ra2, c })
                .Join(db.Rooms, ra3 => ra3.ra2.ra.RoomId, r => r.Id, (ra3, r) => new { ra3, r })
                .Join(db.Day, ra4 => ra4.ra3.ra2.ra.Day, d => d.Id, (ra4, d) => new { ra4, d })
                .Join(db.Departments, ra5 => ra5.ra4.ra3.ra2.ra.DepartmentId, dp => dp.Id,
                    (ra5, dp) => new { ra5, dp })
                .Select(z => new RoomAssignViewModel
                {
                    BatchNumber = z.ra5.ra4.ra3.ra2.b.BatchNo,
                    CourseName = z.ra5.ra4.ra3.c.CourseName,
                    RoomNumber = z.ra5.ra4.r.RoomNo,
                    Day = z.ra5.d.DayName,
                    StartTime = z.ra5.ra4.ra3.ra2.ra.StartTime,
                    EndTime = z.ra5.ra4.ra3.ra2.ra.EndTime,
                    Semester = z.ra5.ra4.ra3.ra2.ra.SemesterId,
                    Id = z.ra5.ra4.ra3.ra2.ra.Id,
                    Department = z.dp.DepartmentName,
                })
                .OrderBy(x => new { x.Department, x.BatchNumber, x.Semester, x.StartTime, x.Day, x.RoomNumber })
                .ToList();
            return RoomAssingList;
        }

        public List<RoomAssignViewModel> RoomAssignViewModels(Student StudentInfo)
        {
            List<RoomAssignViewModel> RoomAssingList =
                db.RoomAssigns.Where(w => w.DepartmentId == StudentInfo.DepartmentId && w.BatchId == StudentInfo.BatchId)
                    .Join(db.Batchs, ra => ra.BatchId, b => b.Id, (ra, b) => new { ra, b })
                    .Join(db.Courses, ra2 => ra2.ra.CourseId, c => c.Id, (ra2, c) => new { ra2, c })
                    .Join(db.Rooms, ra3 => ra3.ra2.ra.RoomId, r => r.Id, (ra3, r) => new { ra3, r })
                    .Join(db.Day, ra4 => ra4.ra3.ra2.ra.Day, d => d.Id, (ra4, d) => new { ra4, d })
                    .Join(db.Departments, ra5 => ra5.ra4.ra3.ra2.ra.DepartmentId, dp => dp.Id,
                        (ra5, dp) => new { ra5, dp })
                    .Select(z => new RoomAssignViewModel
                    {
                        BatchNumber = z.ra5.ra4.ra3.ra2.b.BatchNo,
                        CourseName = z.ra5.ra4.ra3.c.CourseName,
                        RoomNumber = z.ra5.ra4.r.RoomNo,
                        Day = z.ra5.d.DayName,
                        StartTime = z.ra5.ra4.ra3.ra2.ra.StartTime,
                        EndTime = z.ra5.ra4.ra3.ra2.ra.EndTime,
                        Semester = z.ra5.ra4.ra3.ra2.ra.SemesterId,
                        Id = z.ra5.ra4.ra3.ra2.ra.Id,
                        Department = z.dp.DepartmentName,
                    })
                    .OrderBy(x => new { x.Day, x.Semester, x.StartTime })
                    .ToList();
            return RoomAssingList;
        }

        public List<RoomAssignViewModel> RoomAssignViewModelsForAdmin()
        {
            List<RoomAssignViewModel> RoomAssingList =
                db.RoomAssigns
                    .Join(db.Batchs, ra => ra.BatchId, b => b.Id, (ra, b) => new { ra, b })
                    .Join(db.Courses, ra2 => ra2.ra.CourseId, c => c.Id, (ra2, c) => new { ra2, c })
                    .Join(db.Rooms, ra3 => ra3.ra2.ra.RoomId, r => r.Id, (ra3, r) => new { ra3, r })
                    .Join(db.Day, ra4 => ra4.ra3.ra2.ra.Day, d => d.Id, (ra4, d) => new { ra4, d })
                    .Join(db.Departments, ra5 => ra5.ra4.ra3.ra2.ra.DepartmentId, dp => dp.Id,
                        (ra5, dp) => new { ra5, dp })
                    .Select(z => new RoomAssignViewModel
                    {
                        BatchNumber = z.ra5.ra4.ra3.ra2.b.BatchNo,
                        CourseName = z.ra5.ra4.ra3.c.CourseName,
                        RoomNumber = z.ra5.ra4.r.RoomNo,
                        Day = z.ra5.d.DayName,
                        StartTime = z.ra5.ra4.ra3.ra2.ra.StartTime,
                        EndTime = z.ra5.ra4.ra3.ra2.ra.EndTime,
                        Semester = z.ra5.ra4.ra3.ra2.ra.SemesterId,
                        Id = z.ra5.ra4.ra3.ra2.ra.Id,
                        Department = z.dp.DepartmentName,
                    })
                    .OrderBy(x => new { x.Day, x.Semester, x.StartTime })
                    .ToList();
            return RoomAssingList;
        }
    }
}