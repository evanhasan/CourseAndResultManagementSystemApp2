using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITCourseAndResultManagementSystemApp.Models;
using MITCourseAndResultManagementSystemApp.Models.Context;
using MITCourseAndResultManagementSystemApp.Models.ViewModel;

namespace MITCourseAndResultManagementSystemApp.BBL
{
    public class StudentBll
    {
        private AccountDBContext db = new AccountDBContext();

        //list of student for student
        public List<StudentListViewModel> StudentsListForStudentShow(Student StudentInfo)
        {
            List<StudentListViewModel> StudentsList =
                db.Students.Where(x => x.DepartmentId == StudentInfo.DepartmentId && x.BatchId == StudentInfo.BatchId)
                    .Join(db.Batchs, s => s.BatchId, b => b.Id, (s, b) => new { s, b })
                    .Join(db.Departments, s2 => s2.s.DepartmentId, d => d.Id, (s2, d) => new { s2, d })
                    .Select(z => new StudentListViewModel
                    {
                        Id = z.s2.s.Id,
                        Name = z.s2.s.Name,
                        BloodGroup = z.s2.s.BloodGroup,
                        MobileNo = z.s2.s.MobileNo,
                        Email = z.s2.s.Email,
                        Address = z.s2.s.Address,
                        Gender = z.s2.s.Gender,
                        RegNo = z.s2.s.RegNo,
                        RollNumber = z.s2.s.RollNumber,
                        Password = z.s2.s.Password,
                        ConfirmPassword = z.s2.s.ConfirmPassword,
                        BatchNumber = z.s2.b.BatchNo,
                        Department = z.d.DepartmentName,
                        PhotoPath =  z.s2.s.PhotoPath,
                    }).OrderByDescending(o => o.BatchNumber).ToList();
            return StudentsList;
        }

        //list of student for Admin
        public List<StudentListViewModel> StudentListViewModels()
        {
            List<StudentListViewModel> StudentsList = db.Students
                .Join(db.Batchs, s => s.BatchId, b => b.Id, (s, b) => new { s, b })
                .Join(db.Departments, s2 => s2.s.DepartmentId, d => d.Id, (s2, d) => new { s2, d })
                .Select(z => new StudentListViewModel
                {
                    Id = z.s2.s.Id,
                    Name = z.s2.s.Name,
                    MobileNo = z.s2.s.MobileNo,
                    Email = z.s2.s.Email,
                    Address = z.s2.s.Address,
                    Gender = z.s2.s.Gender,
                    RegNo = z.s2.s.RegNo,
                    RollNumber = z.s2.s.RollNumber,
                    Password = z.s2.s.Password,
                    ConfirmPassword = z.s2.s.ConfirmPassword,
                    BatchNumber = z.s2.b.BatchNo,
                    Department = z.d.DepartmentName,
                    PhotoPath = z.s2.s.PhotoPath
                    
                }).OrderByDescending(o => o.BatchNumber).ToList();
            return StudentsList;
        }
    }
}