using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMP.BobEProcure;
using AMP.Finacle;
using AMP.Services;

namespace AMP.Controllers.Dashboad2
{
    public class SyncController : Controller
    {
        // GET: Sync
        public ActionResult Index(string alert = "", string type = "success")
        {
            if (!string.IsNullOrWhiteSpace(alert))
                ViewBag.JavaScriptFunction = string.Format("showNotification('{0}','{1}');", alert, type);
            return View(new AMP.ViewModels.Dashboad2.SyncDetailModel());
            //return View();
        }

        [HttpGet]
        public ActionResult SyncFinacle()
        {
            string exp = "", error = "";
            bool status = true;
            FinacleConnector conn = new FinacleConnector();

            status = conn.LocDetails(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "LocDetails",
            });

            status = conn.AmpContracts(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "AmpContracts",
            });

            status = conn.AmpDemands(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "AmpDemands",
            });

            status = conn.CgsView(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "CgsView",
            });

            status = conn.Disbursment(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "Disbursment",
            });

            status = conn.RepaymentSchdule(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "RepaymentSchdule",
            });

            status = conn.PrincipalDue(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "PrincipalDue",
            });

            status = conn.InterestDue(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "InterestDue",
            });

            status = conn.ContractTransactions(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "ContractTransactions",
            });

            status = conn.LocTransactions(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp.ToString(),
                ServiceName = "LocTransactions",
            });
            //obj.AmpContracts();
            //obj.AmpDemands();
            //obj.CgsView();
            //obj.Disbursment();
            //obj.RepaymentSchdule();
            //obj.PrincipalDue();
            //obj.InterestDue();
            //obj.ContractTransactions();
            //obj.LocTransactions();

            if (status)
            {
                new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
                {
                    Status = "Success",
                    System = "Finacle",
                    FullMessage = "",
                    ServiceName = "FinacleSyncSuccess",
                });
                return RedirectToAction("Index", "Sync", new { alert = "Successfully Synced with Finacle", type = "success" });
            }
            else
                return RedirectToAction("Index", "Sync", new { alert = "Failed to Sync with Finacle", type = "error" });
        }

        [HttpGet]
        public ActionResult SyncBobeProcure()
        {
            string exp = "", error = "";
            bool status = true;
            BobEProcureConnector conn = new BobEProcureConnector();
            status = conn.PackageSync(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Bob eProcure",
                FullMessage = exp.ToString(),
                ServiceName = "PackageSync",
            });
            status = conn.StatusSync(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Bob eProcure",
                FullMessage = exp.ToString(),
                ServiceName = "StatusSync",
            });
            status = conn.VendorSync(out exp) ? status : false;
            error += "::>" + exp;
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Bob eProcure",
                FullMessage = exp.ToString(),
                ServiceName = "VendorSync",
            });

            if (status)
            {
                new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
                {
                    Status = "Success",
                    System = "Bob eProcure",
                    FullMessage = "",
                    ServiceName = "BobSyncSuccess",
                });
                return RedirectToAction("Index", "Sync", new { alert = "Successfully Synced BoB e-Procure", type = "success" });
            }
            else
                return RedirectToAction("Index", "Sync", new { alert = "Failed to Sync BoB e-Procure", type = "error" });
        }
    }
}