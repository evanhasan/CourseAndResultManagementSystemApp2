using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class ThanksButton
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Thanks { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PostId { get; set; }

        public int  Flag { get; set; }
    }
}