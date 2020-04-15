using AMP.Authentication;
using AMP.Models;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Linq;
using System;

namespace AMP.Controllers.Dashboad2
{
    public class PqController : Controller
    {
        // GET: Pqhhh
        public ActionResult Detail(int id)
        {
            if (TempData.ContainsKey("DbStatus"))
            {
                var status = TempData["DbStatus"] as DbStatus;
                ViewData["JavascriptFunction"] = string.Format("showNotification('{0}','{1}', '{2}');", status.Message, status.Status ? "success" : "error", status.Status ? "Success" : "Error");
            }
            PqDetailPageModel pqDetailPage = new PqDetailPageModel();
            pqDetailPage.PqDetail = new Dashboard2ServiceLayer().GetProjectPqPageDetail(id);
            pqDetailPage.Activity = new Dashboard2ServiceLayer().GetAllActivity("TBL_Projects_PQ", pqDetailPage.PqDetail.Id);
            return View(pqDetailPage); ;
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdatePqDetail(ProjectPqModel pqModel)
        {
            DbStatus status;
            if (pqModel.Id == 0)
                status = new Dashboard2ServiceLayer().AddProjectPqDetail(pqModel);
            else
                status = new Dashboard2ServiceLayer().UpdateProjectPqDetail(pqModel);

            if(status.ProcessedId > 0)
            {
                var formFiles = System.Web.HttpContext.Current.Request.Files;
                if (formFiles != null && formFiles.Count > 0)
                {
                    SaveDocuments(status, "pqFile");
                    SaveDocuments(status, "dprFile");
                }
            }

            TempData["DbStatus"] = status;
            return RedirectToAction("Detail", new { Id = pqModel.Id });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult DeletePqFile(int fileId, int pqId)
        {
            DbStatus status;
            status = new Dashboard2ServiceLayer().DeleteFile(fileId);

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        private void SaveDocuments(DbStatus status, string formFieldName)
        {
            string filePath = "";
            string filename = "";
            var fileStatus = SaveFiletoServerDirectory(status.ProcessedId.ToString(), formFieldName, out filePath, out filename);
            if (filePath.Length > 0 && filename.Length > 0)
            {
                TBL_Files file = new TBL_Files();
                file.DisplayName = filename;
                file.FileFor = formFieldName == "pqFile" ? Utilities.Constants.PQ_File : Utilities.Constants.PQ_DPR;
                file.RecordId = status.ProcessedId;
                file.Src = filePath;
                new Dashboard2ServiceLayer().AddFile(file);
                var pqFileId = System.Web.HttpContext.Current.Request.Form["PqFileId"];
                var dprFileId = System.Web.HttpContext.Current.Request.Form["DprFileId"];
                int id = 0;
                try { 
                    id = Convert.ToInt32(formFieldName == "pqFile" ? pqFileId : dprFileId);
                    new Dashboard2ServiceLayer().DeleteFile(id);
                }
                catch { id = 0; }
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
            if(file != null && file.ContentLength > 0)
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

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult SaveApplicantsDetails(ApplicantsModel model)
        {
            DbStatus status;
            if (model.Id == 0)
                status = new Dashboard2ServiceLayer().AddApplicant(model);
            else
                status = new Dashboard2ServiceLayer().UpdateApplicant(model);
            TempData["DbStatus"] = status;
            return RedirectToAction("Detail", "Pq", new { id = model.PqId });
        }
    }
}