namespace IntegrationTestFun.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 60),
                        LastName = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.PersonId)
                .Index(t => new { t.FirstName, t.LastName }, unique: true, name: "IX_FirstNameLastName");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.People", "IX_FirstNameLastName");
            DropTable("dbo.People");
        }
    }
}
