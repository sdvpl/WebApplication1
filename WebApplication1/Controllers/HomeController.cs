using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return RedirectToAction("Index","AlbumTables");

        }

    }
}