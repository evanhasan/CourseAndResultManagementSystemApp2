using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models.ViewModel
{
    public class TeacherShareContentViewModel
    {
        
        public int Id { get; set; }
       public string Title { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public string TeacherName { get; set; }
        public DateTime DateTime { get; set; }
        public int BatchNumber { get; set; }
        public string Department { get; set; }
        
        public string Designation { get; set; }
        public string PhotoPath { get; set; }
      
    }
}