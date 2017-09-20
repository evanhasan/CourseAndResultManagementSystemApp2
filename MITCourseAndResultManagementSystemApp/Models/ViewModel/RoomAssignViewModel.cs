using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models.ViewModel
{
    public class RoomAssignViewModel
    {
        public int Id { get; set; }
        public int BatchNumber { get; set; }
        public string Day { get; set; }
        public string CourseName { get; set; }
        public string RoomNumber { get; set; }
        public int TeacherId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Semester { get; set; }
        public string Department { get; set; }
        public TimeSpan Duration { get; set; }
    }
}