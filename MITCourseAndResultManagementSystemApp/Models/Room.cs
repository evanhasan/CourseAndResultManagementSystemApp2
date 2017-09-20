using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Display(Name = "Room Number")]
        public string RoomNo { get; set; }
        public string Description { get; set; }
    }
}