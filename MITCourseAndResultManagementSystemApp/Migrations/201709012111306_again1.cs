namespace MITCourseAndResultManagementSystemApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class again1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResetPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RandomNumber = c.String(nullable: false),
                        RandomNumber2 = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        UserIP = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        Credit = c.Double(nullable: false),
                        LabTest = c.Double(nullable: false),
                        MidTerm = c.Double(nullable: false),
                        Assignment = c.Double(nullable: false),
                        Attendance = c.Double(nullable: false),
                        Final = c.Double(nullable: false),
                        Other = c.Double(nullable: false),
                        TotalMarks = c.Double(nullable: false),
                        GPA = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShareContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        FilePath = c.String(),
                        PosterId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        BatchId = c.Int(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            AddColumn("dbo.Admins", "PhotoPath", c => c.String());
            AddColumn("dbo.ShareComments", "Flag", c => c.Int(nullable: false));
            AddColumn("dbo.ThanksButtons", "Flag", c => c.Int(nullable: false));
            AlterColumn("dbo.Admins", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShareContents", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.ShareContents", new[] { "DepartmentID" });
            AlterColumn("dbo.Admins", "Email", c => c.String());
            DropColumn("dbo.ThanksButtons", "Flag");
            DropColumn("dbo.ShareComments", "Flag");
            DropColumn("dbo.Admins", "PhotoPath");
            DropTable("dbo.ShareContents");
            DropTable("dbo.Results");
            DropTable("dbo.ResetPasswords");
        }
    }
}
