using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models.ViewModel
{
    public class ShareCommentViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public string CommentTime { get; set; }
        [Required]
        public int ContentId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public int  Flag { get; set; }
        public string PhotoPath { get; set; }
    }
}