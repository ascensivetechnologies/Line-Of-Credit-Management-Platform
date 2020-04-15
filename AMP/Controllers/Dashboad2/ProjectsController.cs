using AMP.Attributes;
using AMP.Authentication;
using AMP.Helpers;
using AMP.Models;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AMP.Controllers.Dashboad2
{
    public class ProjectsController : Controller
    {
        // GET: Projects
        public ActionResult Index(string Search = "", int PageNo = 1, int PageSize = 10, bool MyRecords = true)
        {
            var records = new Dashboard2ServiceLayer().GetProjectsForGrid(Search,MyRecords);
            var model = new GridModel<ProjectGridModel>()
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
            ProjectModel model = new ProjectModel();

            return View(model);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult SaveProject(ProjectModel model, bool isEdit = false)
        {
            DbStatus status = new DbStatus();
            if (ModelState.IsValid)
            {
                model.ProjectStartDate = DateTimeHelper.ToDateObject(model.StartDateTxt);
                model.ProjectEndDate = DateTimeHelper.ToDateObject(model.EndDateTxt);
                var service = new Dashboard2ServiceLayer();
                if (model.Id == 0)
                    status = service.AddProject(model);
                else
                    status = service.UpdateProject(model);
                if (status.Status)
                {
                    status.Message = "Project Saved Successfuly!";
                    ViewBag.Message = status.Message;
                    ViewBag.TextColor = "Green";
                    model.Id = status.ProcessedId;
                }
                else
                {
                    status.Message = "Unable to save Project!";
                    ViewBag.Message = status.Message;
                    ViewBag.TextColor = "Red";
                }
            }
            else
            {
                ViewBag.Message = "Unable to save Project!";
                ViewBag.TextColor = "Red";
            }
            ViewBag.JavascriptFunction = string.Format("showNotification('{0}','{1}', '{2}');", status.Message, status.Status ? "success" : "error", status.Status ? "Success" : "Error");
            if (isEdit)
                return RedirectToAction("Edit", new { Id = model.Id });

            return RedirectToAction("Index");
        }
        

        public ActionResult SearchContracts(string pqNumber)
        {
            var service = new Dashboard2ServiceLayer();
            var list = service.GetPackagesByPqNumber(pqNumber);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult SaveContracts(List<PackageGridModel> Packages, int ProjectId)
        {
            Dashboard2ServiceLayer service = new Dashboard2ServiceLayer();
            var status = service.UpdateProjectContracts(Packages, ProjectId);
            var list = service.GetContractsList(ProjectId);
            return Json(new {Message = "Contract Saved!", Data = list }, JsonRequestBehavior.AllowGet);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult EditProject(int Id)
        {
            var model = new Dashboard2ServiceLayer().GetProjectById(Id);
            return View("Create", model);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult SaveProjectCode(string code, int projectId)
        {
            Dashboard2ServiceLayer serviceLayer = new Dashboard2ServiceLayer();
            var status = serviceLayer.UpdateProjectCode(code, projectId);
            return Json(new { status.Message, status.Status, Data = code }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int Id)
        {
            var service = new Dashboard2ServiceLayer();
            var model = service.GetProjectDetailModel(Id);
            model.Countries = service.GetAllCountries("");
            model.ContactTypes = service.GetAllContactTypes();
            model.ProjectContacts = service.GetAllContactsForProject(Id);
            model.ProjectStatuses = service.GetAllStatus();
            return View(model);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddProjectFile()
        {
            string projectId = System.Web.HttpContext.Current.Request.Params["ProjectId"];
            string _msg = "";
            string filePath = "";
            string filename = "";
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                SaveFiletoServerDirectory(projectId, out filePath, out filename);
                TBL_Files file = new TBL_Files();
                file.DisplayName = filename;
                file.FileFor = Utilities.Constants.Project;
                file.RecordId = Convert.ToInt32(projectId);
                file.Src = filePath;
                new Dashboard2ServiceLayer().AddFile(file);
                _msg = "File Uploaded!";
            }
            else
                _msg = "Unable to upload file";
            return Json(new { msg = _msg });
        }

        private void SaveFiletoServerDirectory(string folderName, out string filePath, out string filename)
        {
            var file = System.Web.HttpContext.Current.Request.Files[0];
            var root = HostingEnvironment.MapPath("/") + @"content\projectfiles\" + folderName;
            if (!System.IO.Directory.Exists(root))
            {
                System.IO.Directory.CreateDirectory(root);
            }
            filename = file.FileName.Trim('\"');
            filePath = System.IO.Path.Combine(root, filename);
            file.SaveAs(filePath);
            filePath = @"\content\projectfiles\" + folderName + @"\" + filename;
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult GetLOCsForLinking(int projectId)
        {
            var records = new Dashboard2ServiceLayer().GetUnlinkedLocForProjectJ(projectId);
            return Json(new { LOCs = records }, JsonRequestBehavior.AllowGet);
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
        public ActionResult UpdateLinkedLOCAmount(LOCProjectModel project)
        {
            new Dashboard2ServiceLayer().UpdateLinkedLOCAmount(project);
            return RedirectToAction("Edit", new {Id = project.ProjectId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult RemoveLinkedLOC(LOCProjectModel project)
        {
            new Dashboard2ServiceLayer().RemoveLinkedLocOfProject(project.Id);
            return RedirectToAction("Edit", new { Id = project.ProjectId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult GetContactsForLinking(int projectId, string searchTxt)
        {
            var records = new Dashboard2ServiceLayer().GetUnlinkedContactsForProject(projectId, searchTxt);
            return Json(new { Contacts = records }, JsonRequestBehavior.AllowGet);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult LinkContactsToProject(List<int> contactIds, int projectId)
        {
            var service = new Dashboard2ServiceLayer();
            service.LinkContactToProject(contactIds, projectId);
            var list = service.GetAllContactsForProject(projectId);
            return Json(new { Message = "Contacts Saved", Data = list });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult SaveProjectLocations(List<ProjectDetailModel.Markers> locations, int projectId, string mapType)
        {
            var service = new Dashboard2ServiceLayer();
            service.SaveProjectLocations(locations, projectId, mapType);
            return Json(new { Message = "Map Saved" });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult SaveandLinkContactToProject(ContactModel contact, int projectId)
        {
            var service = new Dashboard2ServiceLayer();
            var status = service.SaveandLinkContactToProject(contact, projectId);
            var list = service.GetAllContactsForProject(projectId);
            return Json(new { Status = status, Data = list });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpGet]
        public ActionResult GetContactDetail(int contactId)
        {
            var detail = new Dashboard2ServiceLayer().GetContactDetail(contactId);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdateProjectProgress(int projectId, int statusId)
        {
            var service = new Dashboard2ServiceLayer();
            var status = service.UpdateProjectProgress(projectId, statusId);
            return Json(status);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddNewProjectTimeline(ProjectTimelineModel projectTimeline)
        {
            var service = new Dashboard2ServiceLayer();
            var status = service.AddProjectTimeline(projectTimeline);
            return Json(status);
        }


        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdateProjectTimeline(List<ProjectTimelineModel> projectTimelines)
        {
            DbStatus status = new DbStatus();
            try
            {
                new Dashboard2ServiceLayer().UpdateProjectTimelines(projectTimelines);
                status.Status = true;
                status.Message = "Data Saved";
            }
            catch
            {
                status.Status = false;
                status.Message = "Something went wrong";
            }
            return Json(status);
        }

        [LmpAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddTeamMember(int Id, int ProjectId)
        {
            new Dashboard2ServiceLayer().AddTeamMember(Id, ProjectId, "TBL_Project");
            return RedirectToAction("Edit", new { Id = ProjectId });
        }

        [LmpAuthorize(Roles = "Admin")]
        public ActionResult RemoveTeamMember(int id, int ProjectId)
        {
            new Dashboard2ServiceLayer().RemoveTeamMember(id);
            return RedirectToAction("Edit", new { Id = ProjectId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddPqDetail(ProjectPqModel pqModel)
        {
            DbStatus status = new DbStatus();
            if(pqModel.Id == 0)
                status =new Dashboard2ServiceLayer().AddProjectPqDetail(pqModel);
            if (status.ProcessedId > 0)
            {
                var formFiles = System.Web.HttpContext.Current.Request.Files;
                if (formFiles != null && formFiles.Count > 0)
                {
                    var pqFileId = System.Web.HttpContext.Current.Request.Form["PqFileId"];
                    var dprFileId = System.Web.HttpContext.Current.Request.Form["DprFileId"];
                    SaveDocuments(status, "pqFile", pqFileId);
                    SaveDocuments(status, "dprFile", dprFileId);
                }
            }
            return RedirectToAction("Edit", new { Id = pqModel.ProjectId });
        }

        private void SaveDocuments(DbStatus status, string formFieldName, string fileId)
        {
            string filePath = "";
            string filename = "";
            int id = 0;
            try { id = Convert.ToInt32(fileId); }
            catch { id = 0; }
            var fileStatus = SaveFiletoServerDirectory(status.ProcessedId.ToString(), formFieldName, out filePath, out filename);
            if (filePath.Length > 0 && filename.Length > 0)
            {
                TBL_Files file = new TBL_Files();
                file.Id = id;
                file.DisplayName = filename;
                file.FileFor = formFieldName == "pqFile" ? Utilities.Constants.PQ_File : Utilities.Constants.PQ_DPR;
                file.RecordId = status.ProcessedId;
                file.Src = filePath;
                new Dashboard2ServiceLayer().AddFile(file);
            }
            if (!fileStatus.Status)
            {
                status.Status = false;
                status.Message = fileStatus.Message;
            }
        }

        private DbStatus SaveFiletoServerDirectory(string folderName, string formFileName, out string filePath, out string filename)
        {
            string[] fileExt = new string[] { "pdf", "docx", "doc", "xls", "xlsx", "jpg", "png" };
            var file = System.Web.HttpContext.Current.Request.Files[formFileName];
            DbStatus status = new DbStatus();
            if (file != null && file.ContentLength > 0)
            {
                filename = "";
                filePath = "";
                if (file.ContentLength > 5000000)
                {
                    status.Status = false;
                    status.Message = "File Upload Failed, File size should not exceed 5 Mb";
                    return status;
                }
                if (!(file.FileName.Split('.').Length > 1 && fileExt.Contains(file.FileName.Split('.')[1])))
                {
                    status.Status = false;
                    status.Message = "File Upload Failed, File type not supported";
                    return status;
                }
                var root = HostingEnvironment.MapPath("/") + @"content\projectfiles\" + folderName;
                if (!System.IO.Directory.Exists(root))
                {
                    System.IO.Directory.CreateDirectory(root);
                }
                filename = file.FileName.Trim('\"');
                filePath = System.IO.Path.Combine(root, filename);
                file.SaveAs(filePath);
                filePath = @"\content\projectfiles\" + folderName + @"\" + filename;
                status.Status = true;
            }
            else
            {
                filename = "";
                filePath = "";
                status.Status = true;
            }
            return status;
        }


    }
}