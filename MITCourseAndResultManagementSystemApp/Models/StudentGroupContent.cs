using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class StudentGroupContent
    {
        [Key]
        public int Id { get; set; }
          [Required]
        public string Content { get; set; }
        public int StudentId { get; set; }
        public int DepartmentId { get; set; }
        public int BatchId { get; set; }
        public DateTime DateTime { get; set; }
        public string FilePath { get; set; }
       
    }
}