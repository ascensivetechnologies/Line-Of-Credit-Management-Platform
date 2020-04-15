using AMP.Attributes;
using AMP.Authentication;
using AMP.Models;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AMP.EmailAlerts;

namespace AMP.Controllers.Dashboad2
{
    public class LocController : Controller
    {
        // GET: Loc
        public ActionResult Index(string Search="", int PageNo = 1, int PageSize = 10, bool MyRecords = true)
        {
            var service = new Dashboard2ServiceLayer();
            ////////EmailRuleService ruleService = new EmailRuleService();
            //////////ruleService.GenerateEmailSchedule(DateTime.Now.Date);
            ////////ruleService.GenerateMailBodyFromEmailSchedule(DateTime.Now.Date);

            List<LOCModel> records;
            if (MyRecords)
                records = service.GetMyLocs(Search).ToList();
            else
                records = service.GetAllLocs(Search).ToList();
            records.ForEach(item => {
                item.SignedDate = item._SignedDate.HasValue
                                    ? item._SignedDate.Value.ToString("dd/MM/yyyy") : "";
            });
            var model = new GridModel<LOCModel>()
            {
                SearchText = Search,
                PageNo = PageNo,
                PageSize = PageSize,
                Records = records,
                MyRecords = MyRecords
            };
            model.SetNavigation(PageNo);
            return View(model);
        }

        

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult Create()
        {
            LOCModel model = new LOCModel();
            LoadModelHelperData(model);
            return View(model);
        }

        private static void LoadModelHelperData(LOCModel model)
        {
            var countries = new Dashboard2ServiceLayer().GetAllCountries().Select(data => new SelectListItem
            {
                Text = data.CountryName,
                Value = data.Id.ToString()
            }).ToList();
            model.Countries = countries;
           
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult SaveLoc(LOCModel model, bool isEdit = false)
        {
            if(ModelState.IsValid)
            {
                var service = new Dashboard2ServiceLayer();
                var status = service.CreateOrUpdateLoc(model);
            }
            if (isEdit)
                return RedirectToAction("Edit", new { Id = model.Id });

            return RedirectToAction("Index");
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult EditLoc(int Id)
        {
            var model = new Dashboard2ServiceLayer().GetLocById(Id);
            LoadModelHelperData(model);
            return View("Create", model);
        }

        public ActionResult GetProjectsForLinking(int locId)
        {
            var records = new Dashboard2ServiceLayer().GetUnlinkedProjectForLoc(locId);
            return Json(new { Projects = records }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int Id)
        {
            var model = new Dashboard2ServiceLayer().GetLocById(Id);
            return View(model);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult Edit(LOCModel model)
        {
            new Dashboard2ServiceLayer().CreateOrUpdateLoc(model);
            return RedirectToAction("Index");
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult Archive(int Id)
        {
            new Dashboard2ServiceLayer().DeleteLoc(Id);
            return RedirectToAction("Index");
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult LinkProjectToLOC(LOCProjectModel project)
        {
            new Dashboard2ServiceLayer().LinkProjectToLOC(project);
            return RedirectToAction("Index");
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddBalanceConfirmation(LOCConfirmationModel model)
        {
            new Dashboard2ServiceLayer().AddBalanceConfirmationToLOC(model);
            return RedirectToAction("Index");
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddLocFile()
        {
            string locId = System.Web.HttpContext.Current.Request.Params["LocId"];
            string _msg = "";
            string filePath = "";
            string filename = "";
            if (!string.IsNullOrWhiteSpace(locId))
            {
                SaveFiletoServerDirectory(locId, out filePath, out filename);
                TBL_Files file = new TBL_Files();
                file.DisplayName = filename;
                file.FileFor = Utilities.Constants.LOC;
                file.RecordId = Convert.ToInt32(locId);
                file.Src = filePath;
                new Dashboard2ServiceLayer().AddFile(file);
                _msg = "File Uploaded!";
            }
            else
                _msg = "Unable to upload file";
            return Json(new { msg = _msg });
        }

        [HttpGet]
        public ActionResult GetCountryById(int id)
        {
            return Json(new Dashboard2ServiceLayer().GetCountryById(id), JsonRequestBehavior.AllowGet);
        }

        [LmpAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddTeamMember(int Id, int LocId)
        {
            new Dashboard2ServiceLayer().AddTeamMember(Id, LocId, "TBL_Loc");
            return RedirectToAction("Edit",new { Id = LocId });
        }

        [LmpAuthorize(Roles = "Admin")]
        public ActionResult RemoveTeamMember(int id, int LocId)
        {
            new Dashboard2ServiceLayer().RemoveTeamMember(id);
            return RedirectToAction("Edit", new { Id = LocId });
        }

        private void SaveFiletoServerDirectory(string folderName, out string filePath, out string filename)
        {
            var file = System.Web.HttpContext.Current.Request.Files[0];
            var root = HostingEnvironment.MapPath("/") + @"content\locfiles\" + folderName;
            if (!System.IO.Directory.Exists(root))
            {
                System.IO.Directory.CreateDirectory(root);
            }
            filename = file.FileName.Trim('\"');
            filePath = System.IO.Path.Combine(root, filename);
            file.SaveAs(filePath);
            filePath = @"\content\locfiles\" + folderName + @"\" + filename;
        }
    }
}