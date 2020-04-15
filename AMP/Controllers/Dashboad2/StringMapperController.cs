using AMP.Authentication;
using AMP.Models;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.Controllers.Dashboad2
{
    [LmpAuthorize(Roles = "Admin")]
    public class StringMapperController : Controller
    {

        // GET: Region
        public ActionResult Index(string Search = "", int PageNo = 1, int PageSize = 10)
        {
            
            var records = new Dashboard2ServiceLayer().GetAllStringMappers(Search);
            
            var model = new GridModel<StringMapperModel>()
            {
                SearchText = Search,
                PageNo = PageNo,
                PageSize = PageSize,
                Records = records,
            };
            model.SetNavigation(PageNo);
            if (TempData.ContainsKey("JavascriptFunction"))
                ViewBag.JavascriptFunction = TempData["JavascriptFunction"] as string;
          
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(StringMapperModel model)
        {
            DbStatus status = new DbStatus();
            if (ModelState.IsValid)
                {
                status=new Dashboard2ServiceLayer().CreateOrUpdateStringMapper(model);
                }
            else
            {
                status.Status = false;
                var fError = ModelState.Values.SelectMany(e => e.Errors).FirstOrDefault();
                if (fError != null)
                    status.Message = fError.ErrorMessage;
                else
                    status.Message = "Something went wrong";
            }

            TempData["JavascriptFunction"] = string.Format("showNotification('{0}','{1}', '{2}');", status.Message, status.Status ? "success" : "error", status.Status ? "Success" : "Error");
            return RedirectToAction("Index");
           
        }



        //[HttpPost]
        //public ActionResult Delete(int Id)
        //{
        //    if (Id != 0)
        //    {
        //        new Dashboard2ServiceLayer().DeleteRegion(Id);
        //    }
        //    return RedirectToAction("Index");
        //}

    }
}