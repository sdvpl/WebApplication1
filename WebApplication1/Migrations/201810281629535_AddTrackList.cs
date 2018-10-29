namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrackList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackList",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArtistName = c.String(),
                        ReleaseName = c.String(),
                        TrackName = c.String(),
                        TrackID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TrackList");
        }
    }
}
