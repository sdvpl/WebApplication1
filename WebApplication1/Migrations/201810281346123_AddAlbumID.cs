namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAlbumID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlbumTable", "AlbumID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AlbumTable", "AlbumID");
        }
    }
}
