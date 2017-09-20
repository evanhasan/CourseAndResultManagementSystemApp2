using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class ShareComment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime CommentTime { get; set; }
        [Required]
        public int ContentId { get; set; }
        [Required]
        public int UserId { get; set; }

        public int  Flag { get; set; }

    }
}