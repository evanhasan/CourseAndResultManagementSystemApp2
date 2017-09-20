using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public double Credit { get; set; }

    
        public double LabTest { get; set; }
        public double MidTerm { get; set; }
        public double Assignment { get; set; }
        public double Attendance { get; set; }
        public double Final { get; set; }
        public double Other { get; set; }
        public double TotalMarks { get; set; }
        public double GPA { get; set; }
       

    }
}