namespace WebBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Wishlist_Id", "dbo.Wishlists");
            DropIndex("dbo.Books", new[] { "Wishlist_Id" });
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 128),
                        ShippingAddress = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "OriginalPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Orders", "User_Id", c => c.Int());
            AddColumn("dbo.Wishlists", "BookId", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Books", "Author", c => c.String(maxLength: 100));
            AlterColumn("dbo.Books", "ISBN", c => c.String(maxLength: 50));
            AlterColumn("dbo.Books", "Publisher", c => c.String(maxLength: 100));
            AlterColumn("dbo.Books", "Genre", c => c.String(maxLength: 50));
            AlterColumn("dbo.Books", "CoverImageUrl", c => c.String(maxLength: 500));
            AlterColumn("dbo.Books", "UpdatedAt", c => c.DateTime());
            AlterColumn("dbo.Reviews", "Comment", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Reviews", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Wishlists", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "User_Id");
            CreateIndex("dbo.Reviews", "UserId");
            CreateIndex("dbo.Wishlists", "UserId");
            CreateIndex("dbo.Wishlists", "BookId");
            AddForeignKey("dbo.Orders", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Wishlists", "BookId", "dbo.Books", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Wishlists", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.Books", "ImageUrls");
            DropColumn("dbo.Books", "AverageRating");
            DropColumn("dbo.Books", "TotalRatings");
            DropColumn("dbo.Books", "Wishlist_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Wishlist_Id", c => c.Int());
            AddColumn("dbo.Books", "TotalRatings", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "AverageRating", c => c.Double(nullable: false));
            AddColumn("dbo.Books", "ImageUrls", c => c.String());
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Wishlists", "UserId", "dbo.Users");
            DropForeignKey("dbo.Wishlists", "BookId", "dbo.Books");
            DropForeignKey("dbo.Orders", "User_Id", "dbo.Users");
            DropIndex("dbo.Wishlists", new[] { "BookId" });
            DropIndex("dbo.Wishlists", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            AlterColumn("dbo.Wishlists", "UserId", c => c.String());
            AlterColumn("dbo.Reviews", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Reviews", "Comment", c => c.String(maxLength: 1024));
            AlterColumn("dbo.Books", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Books", "CoverImageUrl", c => c.String(maxLength: 512));
            AlterColumn("dbo.Books", "Genre", c => c.String(maxLength: 128));
            AlterColumn("dbo.Books", "Publisher", c => c.String(maxLength: 256));
            AlterColumn("dbo.Books", "ISBN", c => c.String(maxLength: 32));
            AlterColumn("dbo.Books", "Author", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.Wishlists", "BookId");
            DropColumn("dbo.Orders", "User_Id");
            DropColumn("dbo.Books", "OriginalPrice");
            DropTable("dbo.Users");
            CreateIndex("dbo.Books", "Wishlist_Id");
            AddForeignKey("dbo.Books", "Wishlist_Id", "dbo.Wishlists", "Id");
        }
    }
}
