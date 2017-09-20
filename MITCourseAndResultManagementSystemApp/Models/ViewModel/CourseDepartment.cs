using System;
using System.Collections.Generic;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models.ViewModel
{
    public class CourseDepartment
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int BatchId { get; set; }
        public int BatchNumber { get; set; }
        public string CourseName { get; set; }
        public int Semester { get; set; }
        public string TeacherName { get; set; }
        public double Credit { get; set; }
        public int Flag { get; set; }
    
        
    }
}