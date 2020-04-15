using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMP.ViewModels.Dashboad2;

namespace AMP.Controllers.Dashboad2
{
    public class MapReportController : Controller
    {
        // GET: MapReport
        public ActionResult Index()
        {
            return View(new MapReportModel());
        }
    }
}