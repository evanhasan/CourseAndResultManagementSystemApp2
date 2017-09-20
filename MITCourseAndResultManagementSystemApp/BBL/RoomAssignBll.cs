using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class RoomAssignBll
    {
        private AccountDBContext db = new AccountDBContext();
        public List<RoomAssignViewModel> RoomAssignViewModels()
        {
            List<RoomAssignViewModel> RoomAssingList =
                db.RoomAssigns.Join(db.Batchs, ra => ra.BatchId, b => b.Id, (ra, b) => new { ra, b })
                    .Join(db.Courses, ra2 => ra2.ra.CourseId, c => c.Id, (ra2, c) => new { ra2, c })
                    .Join(db.Rooms, ra3 => ra3.ra2.ra.RoomId, r => r.Id, (ra3, r) => new { ra3, r })
                    .Join(db.Day, ra4 => ra4.ra3.ra2.ra.Day, d => d.Id, (ra4, d) => new { ra4, d })
                    .Join(db.Departments, ra5 => ra5.ra4.ra3.ra2.ra.DepartmentId, dp => dp.Id, (ra5, dp) => new { ra5, dp })
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
                    }).OrderBy(x => new { x.Department, x.BatchNumber, x.Semester, x.StartTime, x.Day, x.RoomNumber }).ToList();
            return RoomAssingList;
        }


        public bool CountRoomAssign(RoomAssign roomAssign)
        {
            int TeacherId = db.CourseAssigns.Where(
                x =>
                    x.DepartmentId == roomAssign.DepartmentId && x.BatchId == roomAssign.BatchId &&
                    x.CourseId == roomAssign.CourseId && x.Flag != 0).Select(x => x.TeacherId).FirstOrDefault();

            roomAssign.TeacherId = TeacherId;

            string StartTime = "2017-01-01 " + roomAssign.StartTime.ToString("H:mm");
            string EndTime = "2017-01-01 " + roomAssign.EndTime.ToString("H:mm");

            roomAssign.StartTime = Convert.ToDateTime(StartTime);
            roomAssign.EndTime = Convert.ToDateTime(EndTime);


            bool CheckExist = false;
            var count = db.RoomAssigns.Count(x => (roomAssign.Day == x.Day && roomAssign.RoomId == x.RoomId)
                                                  &&
                                                  ((roomAssign.StartTime > x.StartTime && roomAssign.EndTime < x.EndTime)
                                                   ||
                                                   (roomAssign.StartTime < x.StartTime && roomAssign.EndTime > x.EndTime)
                                                   ||
                                                   (roomAssign.StartTime < x.StartTime && roomAssign.EndTime > x.StartTime)
                                                     ||
                                                     (roomAssign.StartTime > x.StartTime && roomAssign.EndTime > x.EndTime)
                                                   ));
            if (count > 0)
            {
                CheckExist = true;
            }

            return CheckExist;
        }

    }

}