using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Batch
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Enter Batch number"), Display(Name = "Batch No")]
        public int BatchNo { get; set; }
        [Display(Name = "Course Name")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}