using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models.ViewModel
{
    public class StudentListViewModel
    {
        public int Id { get; set; }
        public int BatchNumber { get; set; }
        
        public string Department { get; set; }
        public string Name { get; set; }
        public string BloodGroup { get; set; }
        public string Email { get; set; }
       
        public string MobileNo { get; set; }
       
        public string Address { get; set; }
       
        public string Gender { get; set; }
        
        public string RegNo { get; set; }
       
        public string RollNumber { get; set; }
        public string PhotoPath { get; set; } 
        public string Password { get; set; }
       
        public string ConfirmPassword { get; set; }

    }
}