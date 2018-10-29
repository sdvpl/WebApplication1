using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SongList
    {
        public int ID { get; set; }
        public string ArtistName { get; set; }
        public string ReleaseName { get; set; }
        public string TrackName { get; set; }
        public string TrackID { get; set; }
    }
}