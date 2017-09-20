using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models.ViewModel
{
    public class DepartmentWithBatch
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int BatchId { get; set; }
        public string Department { get; set; }
        public int Batch { get; set; }
        public int Count { get; set; }
    }
}