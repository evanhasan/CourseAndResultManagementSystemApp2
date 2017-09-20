using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class ShareContent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        public string FilePath { get; set; }
        public int PosterId { get; set; }
        public DateTime DateTime { get; set; }

      [Display(Name = "Batch No")]
        public int BatchId { get; set; }
        [Display(Name = "Course Name")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        public int  Flag { get; set; }
    }
}