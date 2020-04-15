using AMP.ViewModels.DashboardModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.Controllers.Dashboad2
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var model = new DashboardModel();
            return View(model);
        }
    }
}