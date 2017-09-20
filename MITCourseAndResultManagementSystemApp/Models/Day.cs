using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        [Required, Display(Name = "Day")]
        public string DayName { get; set; }
    }
}