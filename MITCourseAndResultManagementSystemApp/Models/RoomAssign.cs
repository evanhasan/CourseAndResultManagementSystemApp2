using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class RoomAssign
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Batch "), Display(Name = "Batch No")]
        public int BatchId { get; set; }
        [Required(ErrorMessage = "Please enter Course "), Display(Name = "SubJect")]
        public int CourseId { get; set; }
         [Required(ErrorMessage = "Please enter Semester "), Display(Name = "Semester")]
        public int SemesterId { get; set; }
        [Required(ErrorMessage = "Please enter Room "), Display(Name = "Room Number")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Please enter Day ")]
        public int Day { get; set; }
        [Display(Name = "Teacher Name")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Please enter Start time"), Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "Please enter End Time "), Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
     

        public int flag { get; set; }
        [Display(Name = "Course Name")]
        public int DepartmentId { get; set; }
        public virtual Department Departments { get; set; }
    }
}