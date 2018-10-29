using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebApplication1.DAL;
using WebApplication1.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace WebApplication1.Controllers
{
    public class AlbumTablesController : Controller
    {
        public ActionResult Index()
        {
            return View(db.AlbumTableEntry.OrderBy(x => x.ReleaseDate).ToList());
        }

        private SongListContext db = new SongListContext();

        public ActionResult ClearAlbumSearch()
        {
            db.AlbumTableEntry.RemoveRange(db.AlbumTableEntry);
            db.SaveChanges();

            return View("Index", db.AlbumTableEntry.ToList());
        }

        public ActionResult ClearTracklist()
        {
            db.TrackListEntry.RemoveRange(db.TrackListEntry);
            db.SaveChanges();

            return View("TrackList", db.TrackListEntry.ToList());
        }

        public ActionResult ClearSongList()
        {
            db.SongListEntry.RemoveRange(db.SongListEntry);
            db.SaveChanges();

            return View("SongList", db.SongListEntry.ToList());
        }

        [HttpPost]
        public ActionResult Find(string Artistname)
        {
            
            ViewBag.Message = "try";
            //Search s1 = new Search();
            String ArtistID ="";
            System.Diagnostics.Debug.WriteLine("ArtistName = " + Artistname);

            if (String.IsNullOrEmpty(Artistname))
            {
                System.Diagnostics.Debug.WriteLine("ArtistName is empty");
            }
            else
            {
                ArtistID = SearchArtist(Artistname);

                System.Diagnostics.Debug.WriteLine("ArtistID = " + ArtistID);
            }
            if (String.IsNullOrEmpty(ArtistID) | ArtistID.ToString() == "empty")
            {
                ViewBag.Message = "Could not find";
                    //return View("Index");
            }
            else
            {
                db.AlbumTableEntry.RemoveRange(db.AlbumTableEntry);
                db.SaveChanges();
                AlbumSearch(ArtistID, Artistname);
            }

            return View("Index", db.AlbumTableEntry.OrderBy(x => x.ReleaseDate).ToList());
            //return View("Index");
        }

        public String SearchArtist(String ArtistToFind)
        {
            if (String.IsNullOrEmpty(ArtistToFind))
            {
                System.Diagnostics.Debug.WriteLine("empty ArtistToFind");
                return "empty";
            }
            else
            {
                TextInfo TInfo = new CultureInfo("en-US", false).TextInfo;
                String text = TInfo.ToTitleCase(ArtistToFind);
                text = text.Trim();
                System.Diagnostics.Debug.WriteLine(text);
                string ArtistID = ArtistSearch(text);
                return ArtistID;
                //ShowAlbumSearch();
            }
        }

        public String ArtistSearch(String ArtistName)
        {
            var Artistrequest = (HttpWebRequest)WebRequest.Create("https://musicbrainz.org/ws/2/artist/?query=" + ArtistName);
            Artistrequest.Method = "GET";
            Artistrequest.UserAgent = "DesktopApp1/1.1.1 ( sdvpl2011@gmail.com )";
            Artistrequest.Credentials = new NetworkCredential("sdvpl2011", "musicbrainz");
            var Artistresponse = (HttpWebResponse)Artistrequest.GetResponse();
            string ArtistID = "";
            string name = "";

            XmlReader reader = XmlReader.Create(Artistresponse.GetResponseStream());
            while (reader.Read())
            {


                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "artist"))
                {
                    ArtistID = reader.GetAttribute("id");
                    reader.ReadToDescendant("name");
                    name = reader.ReadElementContentAsString();
                    Console.WriteLine("Check Name = " + name);
                    if (name == ArtistName)
                    {
                        break;
                    }
                    //reader.Skip();
                    //reader.ReadToFollowing("name");
                    //break;
                    //.ReadElementContentAsString();
                    //XmlReader inner = reader.ReadSubtree();

                    //inner.ReadToDescendant("name");
                    //Console.WriteLine("Check Name = " + reader.ReadElementContentAsString());
                }

                //reader.ReadToFollowing("artist");




            }
            Artistresponse.Close();


            if (String.IsNullOrEmpty(ArtistID))
            {
                string row1 = "empty";
                System.Diagnostics.Debug.WriteLine(row1);
                return row1;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ArtisID is not empty: " + ArtistID);
                return ArtistID;
            }
            //}
        }

        public void AlbumSearch(String ArtistID, String ArtistName)
        {
            //https://musicbrainz.org/ws/2/release?release-group=3d00fb45-f8ab-3436-a8e1-b4bfc4d66913
            //http://musicbrainz.org/ws/1/release/03e4ebe1-0a44-411c-8e19-78e0768603f8?type=xml&inc=tracks

            var Albumrequest = (HttpWebRequest)WebRequest.Create("https://musicbrainz.org/ws/2/release-group?artist=" + ArtistID);
            //var Albumrequest = (HttpWebRequest)WebRequest.Create("http://musicbrainz.org/ws/1/artist/" + ArtistID + "?type=xml&inc=sa-Official+release-events");

            Albumrequest.Method = "GET";
            Albumrequest.UserAgent = "DesktopApp1/1.1.1 ( sdvpl2011@gmail.com )";
            Albumrequest.Credentials = new NetworkCredential("sdvpl2011", "musicbrainz");
            var Albumresponse = (HttpWebResponse)Albumrequest.GetResponse();
            String Aname = ArtistName;
            String Albumname = "";
            String ReleaseDate = "";
            String AlbumID1 = "";

            XmlReader reader = XmlReader.Create(Albumresponse.GetResponseStream());
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "release-group"))
                {
                    AlbumID1 = reader.GetAttribute("id");
                }
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "title"))
                {

                    Albumname = reader.ReadElementContentAsString();
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "first-release-date"))
                {
                    ReleaseDate = reader.ReadElementContentAsString();
                    String[] row1 = { Aname, Albumname, ReleaseDate };

                    System.Diagnostics.Debug.WriteLine("ArtistName = " + Aname);
                    System.Diagnostics.Debug.WriteLine("ReleaseName = " + Albumname);
                    System.Diagnostics.Debug.WriteLine("ReleaseDate = " + ReleaseDate);

                    AlbumTable at = new AlbumTable
                    {
                        ArtistName = Aname,
                        ReleaseName = Albumname,
                        ReleaseDate = ReleaseDate,
                        AlbumID = AlbumID1
                    };
                    db.AlbumTableEntry.Add(at);
                    db.SaveChanges();
                }
            }
            Albumresponse.Close();
        }

        public ActionResult ViewTrackList()
        {
            return View("TrackList", db.TrackListEntry.ToList());
        }

        public ActionResult TrackList(string artistname, string albumname, string releasedate, string albumid)
        {
            System.Diagnostics.Debug.WriteLine("artistname = " + artistname.ToString());
            System.Diagnostics.Debug.WriteLine("albumname = " + albumname.ToString());
            System.Diagnostics.Debug.WriteLine("releasedate = " + releasedate.ToString());
            System.Diagnostics.Debug.WriteLine("albumid = " + albumid.ToString());

            db.TrackListEntry.RemoveRange(db.TrackListEntry);
            db.SaveChanges();

            RecordSearch(artistname, albumname, releasedate, albumid);
            
            return View(db.TrackListEntry.ToList());
        }

        private void RecordSearch(string artistname, string aname, string rdate, string aid)
        {
            //https://musicbrainz.org/ws/2/release?release-group=3d00fb45-f8ab-3436-a8e1-b4bfc4d66913

            var Trackrequest = (HttpWebRequest)WebRequest.Create("https://musicbrainz.org/ws/2/release?release-group=" + aid);

            Trackrequest.Method = "GET";
            Trackrequest.UserAgent = "DesktopApp1/1.1.1 ( sdvpl2011@gmail.com )";
            Trackrequest.Credentials = new NetworkCredential("sdvpl2011", "musicbrainz");
            var Trackresponse = (HttpWebResponse)Trackrequest.GetResponse();
            string artname = artistname;
            string Albumname = aname;
            string ReleaseDate = rdate;
            string AlbumID = aid;
            string releaseid = "";
            string title = "";

            XmlReader reader = XmlReader.Create(Trackresponse.GetResponseStream());
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "release"))
                {
                    releaseid = reader.GetAttribute("id");
                    //Console.WriteLine("releaseid = " + releaseid);
                }
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "title"))
                {
                    title = reader.ReadElementContentAsString();
                    //Console.WriteLine("title = " + title);
                    //Console.WriteLine("Albumname = " + Albumname);
                }
                if (releaseid != null && title == Albumname)
                {
                    //Console.WriteLine("Realease Id = " + releaseid);
                    TrackSearch(artname, Albumname, ReleaseDate, releaseid);
                    break;

                }
            }
            Trackresponse.Close();
        }

        private void TrackSearch(string artname, string aname, string rdate, string aid)
        {
            var Trackrequest = (HttpWebRequest)WebRequest.Create("http://musicbrainz.org/ws/1/release/" + aid + "?type=xml&inc=tracks");

            Trackrequest.Method = "GET";
            Trackrequest.UserAgent = "DesktopApp1/1.1.1 ( sdvpl2011@gmail.com )";
            Trackrequest.Credentials = new NetworkCredential("sdvpl2011", "musicbrainz");
            var Trackresponse = (HttpWebResponse)Trackrequest.GetResponse();
            string artistname = artname;
            string Albumname = aname;
            string ReleaseDate = rdate;
            string AlbumID = aid;
            string trackname;

            XmlReader reader = XmlReader.Create(Trackresponse.GetResponseStream());

            while (reader.Read())
            {
                var elementName = string.Empty;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    elementName = reader.Name;
                    switch (elementName)
                    {
                        case "track-list":
                            {
                                var subReader = reader.ReadSubtree();
                                while (subReader.ReadToFollowing("track"))
                                {
                                    if (subReader.ReadToFollowing("title"))
                                    {
                                        trackname = reader.ReadElementContentAsString();
                                        string[] row = { artistname, Albumname, trackname };
                                        TrackList tl = new TrackList
                                        {
                                            ArtistName = artistname,
                                            ReleaseName = Albumname,
                                            TrackName = trackname,
                                            TrackID = aid
                                        };
                                        db.TrackListEntry.Add(tl);
                                        db.SaveChanges();
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            Trackresponse.Close();
        }

        public ActionResult BackToAlbumSearch()
        {
            return View("Index", db.AlbumTableEntry.OrderBy(x => x.ReleaseDate).ToList());
        }

        public ActionResult AddToSongList(string artistname, string albumname, string trackname, string TrackID )
        {
            SongList sl = new SongList
            {
                ArtistName = artistname,
                ReleaseName = albumname,
                TrackName = trackname,
                TrackID = TrackID
            };
            db.SongListEntry.Add(sl);
            db.SaveChanges();

            return View("TrackList", db.TrackListEntry.ToList());
        }

        public ActionResult ViewSongList()
        {
            return View("SongList", db.SongListEntry.ToList());
        }

        public ActionResult RemoveFromSongList(int id)
        {
            SongList sl2 = db.SongListEntry.Find(id);
            db.SongListEntry.Remove(sl2);
            db.SaveChanges();
            return View("SongList", db.SongListEntry.ToList());
        }

        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            gv.DataSource = db.SongListEntry.ToList();
            //var slexc = db.SongListEntry.SqlQuery("SELECT ArtistName, ReleaseName, TrackName FROM SongList").ToList();
            //gv.DataSource = slexc;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=SongList.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            db.SongListEntry.RemoveRange(db.SongListEntry);
            db.SaveChanges();

            return View("SongList", db.SongListEntry.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
