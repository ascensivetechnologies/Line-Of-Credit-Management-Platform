using AMP.Authentication;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AMP.Controllers.Dashboad2
{
    [LmpAuthorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(string Search = "", int PageNo = 1, int PageSize = 10)
        {
            var records = new Dashboard2ServiceLayer().GetAllUsers(Search);
            var model = new GridModel<UsersModel>()
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
            var model = new UsersModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UsersModel model)
        {
            try
            {
                string base64 = Request.Form["imgCropped"];
                byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
                model.ContactImage = bytes;
            }
            catch(Exception ex)
            {

            }
            new Dashboard2ServiceLayer().CreateOrUpdateUser(model);
            return RedirectToAction("Index", "User");
        }

        public ActionResult Edit(int Id)
        {
            var model = new Dashboard2ServiceLayer().GetUserId(Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UsersModel model)
        {
            try
            {
                string base64 = Request.Form["imgCropped"];
                byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
                model.ContactImage = bytes;
            }
            catch (Exception ex)
            {

            }
            
            new Dashboard2ServiceLayer().CreateOrUpdateUser(model);
            return RedirectToAction("Index", "User");
        }

        public ActionResult Delete(int id)
        {
            new Dashboard2ServiceLayer().DeleteUser(id);
            return RedirectToAction("Index", "User");
        }
    }
}