namespace Books_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resolve_relationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBooks1",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Book_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Book_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Book_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBooks1", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.UserBooks1", "User_Id", "dbo.Users");
            DropIndex("dbo.UserBooks1", new[] { "Book_Id" });
            DropIndex("dbo.UserBooks1", new[] { "User_Id" });
            DropTable("dbo.UserBooks1");
        }
    }
}
