using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class SongListInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SongListContext>
    {
        protected override void Seed(SongListContext context)
        {
            var albums = new List<AlbumTable>
            {
            new AlbumTable{ArtistName="Metallica",ReleaseName="Metallica",ReleaseDate="1990",AlbumID="000"},
            };

            var TrackLists = new List<TrackList>
            {
            new TrackList{ArtistName="Metallica",ReleaseName="Metallica",TrackName="Enter Sandman",TrackID="000"}
            };

            var SongLists = new List<SongList>
            {
            new SongList{ArtistName="Metallica",ReleaseName="Metallica",TrackName="Enter Sandman",TrackID="000"}
            };
        }
    }
}