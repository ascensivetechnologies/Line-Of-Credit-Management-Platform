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
    public class TermsController : Controller
    {
        // GET: Terms
        public ActionResult Index(string Search = "",int PageNo = 1, int PageSize = 10)
        {
            var records = new Dashboard2ServiceLayer().GetAllTerms(Search);
            var model = new GridModel<TermsModel>()
            {
                SearchText = Search,
                PageNo = PageNo,
                PageSize = PageSize,
                Records = records,
            };
            model.SetNavigation(PageNo);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new TermsModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TermsModel model)
        {
            if (ModelState.IsValid)
            {
                new Dashboard2ServiceLayer().CreateOrUpdateTerms(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var model = new Dashboard2ServiceLayer().GetTermsById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(TermsModel model)
        {
            if (ModelState.IsValid)
            {
                new Dashboard2ServiceLayer().CreateOrUpdateTerms(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int Id)
        {
            new Dashboard2ServiceLayer().DeleteTerms(Id);
            return RedirectToAction("Index");
        }
    }
}