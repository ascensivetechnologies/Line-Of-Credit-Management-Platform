using AMP.Authentication;
using AMP.Models;
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
    public class CountryController : Controller
    {
        // GET: Country
        public ActionResult Index(string Search = "", int PageNo = 1, int PageSize = 10)
        {
            var records = new Dashboard2ServiceLayer().GetAllCountries(Search);
            var model = new GridModel<CountryModel>()
            {
                SearchText = Search,
                PageNo = PageNo,
                PageSize = PageSize,
                Records = records,
            };
            model.AlphabetPagers = AlphabetPagerModel.GetAlphabetPager(Search);
            model.SetNavigation(PageNo);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new CountryModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CountryModel model)
        {
            DbStatus status = new DbStatus();
            if (ModelState.IsValid)
            {
                status = new Dashboard2ServiceLayer().CreateOrUpdateCountry(model);
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
            if (status.Status)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.JavascriptFunction = string.Format("showNotification('{0}','{1}');", status.Message, "error");
                return View(model);
            }
            
        }

        public ActionResult Edit(int id)
        {
            var model = new Dashboard2ServiceLayer().GetCountryById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CountryModel model)
        {
            DbStatus status = new DbStatus();
            if (ModelState.IsValid)
            {
                status = new Dashboard2ServiceLayer().CreateOrUpdateCountry(model);
                if (status.Status)
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.JavascriptFunction = string.Format("showNotification('{0}','{1}');", status.Message, "error");
                    return View(model);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(error => error.Errors).ToList();
                if (errors.Any())
                {
                    ViewBag.JavascriptFunction = string.Format("showNotification('{0}','{1}');", errors[0].ErrorMessage, "error");
                }
                else
                {
                    ViewBag.JavascriptFunction = string.Format("showNotification('{0}','{1}');", "Something went wrong", "error");
                }
            }
            return View(model);
        }

        public ActionResult Delete(int Id)
        {
            DbStatus status = new DbStatus();
            status = new Dashboard2ServiceLayer().DeleteCountry(Id);
            ViewBag.JavascriptFunction = string.Format("showNotification('{0}','{1}', '{2}');", status.Message, status.Status ? "success" : "error", status.Status ? "Success" : "Error");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetTermsBySelectedId(string Loc)
        {
            return Json(new Dashboard2ServiceLayer().GetAllTerms().Where(e => e.LOCClassification.Equals(Loc)).FirstOrDefault() ,JsonRequestBehavior.AllowGet);
        }
    }
}