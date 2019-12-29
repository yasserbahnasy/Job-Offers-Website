namespace Job_Offers_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplyJobs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplayForJobs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        message = c.String(),
                        ApplayDate = c.DateTime(nullable: false),
                        JobId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.JobId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplayForJobs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplayForJobs", "JobId", "dbo.Jobs");
            DropIndex("dbo.ApplayForJobs", new[] { "UserId" });
            DropIndex("dbo.ApplayForJobs", new[] { "JobId" });
            DropTable("dbo.ApplayForJobs");
        }
    }
}
