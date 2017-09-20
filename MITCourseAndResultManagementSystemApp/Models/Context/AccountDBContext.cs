using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models.Context
{
    public class AccountDBContext:DbContext
    {
      public DbSet<MasterUserAccount> MasterUserAccounts { get; set; }
      public DbSet<Admin> Admins { get; set; }
      public DbSet<Department> Departments { get; set; }
      public DbSet<Course> Courses { get; set; }
      public DbSet<Teacher> Teachers { get; set; }
      public DbSet<Batch> Batchs { get; set; }
      public DbSet<CourseAssign> CourseAssigns { get; set; }
      public DbSet<Student> Students { get; set; }
      public DbSet<TeacherShareContent> TeacherShareContents { get; set; }
      public DbSet<Room> Rooms { get; set; }
      public DbSet<RoomAssign> RoomAssigns { get; set; }
      public DbSet<Day> Day { get; set; }
      public DbSet<ShareComment> ShareComments { get; set; }
      public DbSet<StudentGroupContent> StudentGroupContent { get; set; }
      public DbSet<ThanksButton> ThanksButtons { get; set; }
      public DbSet<ShareContent> ShareContents { get; set; }
      public DbSet<ResetPassword> ResetPassword { get; set; }
      public DbSet<Result> Results { get; set; }
        
    }
}