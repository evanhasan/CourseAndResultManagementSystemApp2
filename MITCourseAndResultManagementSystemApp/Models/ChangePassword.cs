using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MITCourseAndResultManagementSystemApp.BBL;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class ChangePassword
    {
        public int Id { get; set; }
         [Required, DataType(DataType.Password), MinLength(8, ErrorMessage = "Min 8 Char Needed"), Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password), MinLength(8, ErrorMessage = "Min 8 Char Needed"), Display(Name = "New Password")]
        public string NewPassword { get; set; }

         [Required, Compare("NewPassword"), DataType(DataType.Password), MinLength(8, ErrorMessage = "Min 8 Char Needed"), Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }

        public int UserId { get; set; }
    }
}