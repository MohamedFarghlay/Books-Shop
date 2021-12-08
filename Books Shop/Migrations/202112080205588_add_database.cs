namespace Books_Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(),
                        TotalEditions = c.Int(nullable: false),
                        CurrentEditions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfEditions = c.Int(nullable: false),
                        books_Id = c.Int(),
                        users_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.books_Id)
                .ForeignKey("dbo.Users", t => t.users_Id)
                .Index(t => t.books_Id)
                .Index(t => t.users_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBooks", "users_Id", "dbo.Users");
            DropForeignKey("dbo.UserBooks", "books_Id", "dbo.Books");
            DropIndex("dbo.UserBooks", new[] { "users_Id" });
            DropIndex("dbo.UserBooks", new[] { "books_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.UserBooks");
            DropTable("dbo.Books");
        }
    }
}
