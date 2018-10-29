using WebApplication1.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebApplication1.DAL
{
    public class SongListContext : DbContext
    {
        public SongListContext() : base("SongListContext") =>
            //Database.SetInitializer<SongListContext>(new DropCreateDatabaseIfModelChanges<SongListContext>());
            Database.SetInitializer(strategy: new DropCreateDatabaseAlways<SongListContext>());

        public DbSet<AlbumTable> AlbumTableEntry { get; set; }
        public DbSet<TrackList> TrackListEntry { get; set; }
        public DbSet<SongList> SongListEntry { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}