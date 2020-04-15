using AMP.Authentication;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AMP.Controllers.Dashboad2
{
    [LmpAuthorize(Roles = "Admin")]
    public class OptionsController : Controller
    {
        // GET: Options
        public ActionResult Index(OptionTypes SearchType = OptionTypes.Risk, string Search = "", int PageNo = 1, int PageSize = 10)
        {
            var records = new Dashboard2ServiceLayer().GetAllOptions(Search, SearchType);
            var model = new GridModel<Options>()
            {
                SearchText = Search,
                PageNo = PageNo,
                PageSize = PageSize,
                Records = records,
                Record = new Options()
                {
                   Parents = new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.Sector).Select(x =>
                   {
                       return new SelectListItem()
                       {
                           Text = x.Value,
                           Value = x.Id.ToString()
                       };
                   }).ToList()
                }
                
            };
            model.SetNavigation(PageNo);
            OptionPageModel pageModel = new OptionPageModel();
            pageModel.Model = model;
            pageModel.SelectedOption = ((int)SearchType).ToString();
            pageModel.GetOptionList = EnumHelper.GetSelectList(typeof(OptionTypes), (OptionTypes)SearchType).ToList();
            //ViewBag.SelectedSearchType = ((int)SearchType).ToString();
            if(TempData.ContainsKey("JavascriptFunction"))
                ViewBag.JavascriptFunction = TempData["JavascriptFunction"] as string;
            return View(pageModel);
        }

        [HttpPost]
        public ActionResult Create(Options model)
        {
            if (string.IsNullOrWhiteSpace(model.Value))
            {
                ModelState.AddModelError("", "Value and Type are required fields");
                return Redirect("Index"); 
            }
            var status = new Dashboard2ServiceLayer().CreateOrUpdateOption(model);
            TempData["JavascriptFunction"] = string.Format("showNotification('{0}','{1}', '{2}');", status.Message, status.Status ? "success" : "error", status.Status ? "Success" : "Error");

            return RedirectToAction("Index", new {SearchType = (int)model.Type });
        }

        public ActionResult Delete(int Id)
        {
            var service = new Dashboard2ServiceLayer();
            var option = service.GetOptionById(Id);
            var status = service.DeleteOption(Id);
            TempData["JavascriptFunction"] = string.Format("showNotification('{0}','{1}', '{2}');", status.Message, status.Status ? "success" : "error", status.Status ? "Success" : "Error");
            return RedirectToAction("Index", new { SearchType = (int)option.Type });
        }
        
    }
}