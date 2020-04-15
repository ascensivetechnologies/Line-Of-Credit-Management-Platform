using AMP.Authentication;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.Controllers.Dashboad2
{
    [LmpAuthorize(Roles = "Admin")]
    public class RegionController : Controller
    {
        
        // GET: Region
        public ActionResult Index(string Search = "", int PageNo = 1, int PageSize = 10)
        {
            var records = new Dashboard2ServiceLayer().GetAllRegions(Search);
            var model = new GridModel<RegionModel>()
            {
                SearchText = Search,
                PageNo = PageNo,
                PageSize = PageSize,
                Records = records,
            };
            model.SetNavigation(PageNo);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(RegionModel model)
        {
            if(ModelState.IsValid)
            {
                new Dashboard2ServiceLayer().CreateOrUpdateRegion(model);
            }
            return RedirectToAction("Index"); 
        }

        

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            if (Id != 0)
            {
                new Dashboard2ServiceLayer().DeleteRegion(Id);
            }
            return RedirectToAction("Index");
        }
        
    }
}