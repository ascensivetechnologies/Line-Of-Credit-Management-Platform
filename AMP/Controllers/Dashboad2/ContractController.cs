using System;
using System.Web.Hosting;
using System.Web.Mvc;
using AMP.Models;
using AMP.Services;
using AMP.Authentication;
using AMP.ViewModels.Dashboad2;

namespace AMP.Controllers.Dashboad2
{
    public class ContractController : Controller
    {
        public ActionResult ContractDetail(int id)
        {
            if (TempData.ContainsKey("DbStatus"))
            {
                var status = TempData["DbStatus"] as DbStatus;
                ViewData["JavascriptFunction"] = string.Format("showNotification('{0}','{1}', '{2}');", status.Message, status.Status ? "success" : "error", status.Status ? "Success" : "Error");
            }
            var model = new Dashboard2ServiceLayer().GetContractDetail(id);
            return View(model);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddContractFile()
        {
            string contractId = System.Web.HttpContext.Current.Request.Params["ContractId"];
            string _msg = "";
            string filePath = "";
            string filename = "";
            if (!string.IsNullOrWhiteSpace(contractId))
            {
                SaveFiletoServerDirectory(contractId, out filePath, out filename);
                TBL_Files file = new TBL_Files();
                file.DisplayName = filename;
                file.FileFor = Utilities.Constants.Contract;
                file.RecordId = Convert.ToInt32(contractId);
                file.Src = filePath;
                new Dashboard2ServiceLayer().AddFile(file);
                _msg = "File Uploaded!";
            }
            else
                _msg = "Unable to upload file";
            return Json(new { msg = _msg });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult Create(int pqId)
        {
            EditContractModel editContractModel = new EditContractModel();
            editContractModel.Contract.PQId = pqId;
            var service = new Dashboard2ServiceLayer();
            return View(editContractModel);
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        public ActionResult SaveContract(EditContractModel editContractModel)
        {
            var service = new Dashboard2ServiceLayer();
            var status = service.SaveContract(editContractModel.Contract);
            TempData["DbStatus"] = status;
            return RedirectToAction("Detail", "Pq", new { Id = editContractModel.Contract.PQId });
        }

        public ActionResult EditContract(ContractModel model)
        {
            var status = new Dashboard2ServiceLayer().SaveContract(model);
            TempData["DbStatus"] = status;
            return RedirectToAction("ContractDetail", "Contract", new { Id = model.Id });
        }
        private void SaveFiletoServerDirectory(string folderName, out string filePath, out string filename)
        {
            var file = System.Web.HttpContext.Current.Request.Files[0];
            var root = HostingEnvironment.MapPath("/") + @"content\contractfiles\" + folderName;
            if (!System.IO.Directory.Exists(root))
            {
                System.IO.Directory.CreateDirectory(root);
            }
            filename = file.FileName.Trim('\"');
            filePath = System.IO.Path.Combine(root, filename);
            file.SaveAs(filePath);
            filePath = @"\content\contractfiles\" + folderName + @"\" + filename;
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdateTender(ContractModel model)
        {
            new Dashboard2ServiceLayer().UpdateContractTenure(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.Id });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdatePqDetails(ProjectPqModel pqModel)
        {
            var status = new Dashboard2ServiceLayer().UpdateProjectPqDetail(pqModel);
            TempData["DbStatus"] = status;
            return RedirectToAction("ContractDetail", "Contract", new { id = pqModel.ContractId });
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
            return RedirectToAction("ContractDetail", "Contract", new { id = model.ContractId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdateFunding(ContractModel model)
        {
            new Dashboard2ServiceLayer().UpdateContractFunding(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.Id });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddResponsibilty(ResponsibilityModel model)
        {
            new Dashboard2ServiceLayer().AddResponsibility(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.ContractId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdateGurantee(ContractModel model)
        {
            new Dashboard2ServiceLayer().UpdateContractGuarantee(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.Id });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddTerms(PaymentTermsModel model)
        {
            new Dashboard2ServiceLayer().AddContractPaymentTerms(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.ContractId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddContent(ContentRequirementModel model)
        {
            new Dashboard2ServiceLayer().AddContractContent(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.ContractId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddLC(CreditLetterModel model)
        {
            new Dashboard2ServiceLayer().AddContractLC(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.ContractId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdateClosure(ContractModel model)
        {
            new Dashboard2ServiceLayer().UpdateContractClosure(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.Id });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult AddLoc(LocMapModel model)
        {
            new Dashboard2ServiceLayer().AddContractLoc(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.ContractId });
        }

        [LmpAuthorize(Roles = "DeskOfficer")]
        [HttpPost]
        public ActionResult UpdateContractNumber(string code, int contractId)
        {
            var status = new Dashboard2ServiceLayer().UpdateContractNumber(code, contractId);
            return Json(new { status.Message, status.Status, data = code }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePayment(PaymentTermsModel model)
        {
            new Dashboard2ServiceLayer().AddContractPaymentTerms(model);
            return RedirectToAction("ContractDetail", "Contract", new { id = model.ContractId });
        }
    }
}