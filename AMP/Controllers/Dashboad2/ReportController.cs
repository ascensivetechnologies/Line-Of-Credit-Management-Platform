﻿using AMP.Attributes;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using AMP.ViewModels.ReportBuilder;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;
using System.Drawing;
using OfficeOpenXml.Style;
using System;

namespace AMP.Controllers.Dashboad2
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index(string Search = "", int PageNo = 1, int PageSize = 10)
        {
            var records = new Dashboard2ServiceLayer().GetAllReports(Search);
            var model = new GridModel<ReportingManager>()
            {
                SearchText = Search,
                PageNo = PageNo,
                PageSize = PageSize,
                Records = records,
            };
            model.SetNavigation(PageNo);
            return View(model);
        }

        public ActionResult StandardReport(string Search = "", int PageNo = 1, int PageSize = 10)
        {
            var records = new Dashboard2ServiceLayer().GetAllStandardReports(Search);
            var model = new GridModel<ReportingManager>()
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
            var model = new ReportModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReportModel model)
        {
            model.Filters = (model.Filters ?? new List<ViewModels.ReportBuilder.Filter>()).Where(e => e.Removed != true).ToList();
            model.GroupBys = (model.GroupBys ?? new List<ViewModels.ReportBuilder.GroupBy>()).Where(e => e.Removed != true).ToList();
            if (model.CreateReport)
            {
                ReportingService.CreateReport(model);
               
                if(!(model.Errors ?? new List<string>()).Any())
                {
                    return RedirectToAction("Index", "Report");
                }
                    
            }
            else if(model.ExcelExport)
            {
                DataSet dataset = ReportingService.RunExcelReport(model);
                if (dataset.Tables.Count > 0)
                {
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        foreach (DataTable set in dataset.Tables)
                        {
                            var workSheet = package.Workbook.Worksheets.Add("Sheet 1");
                            workSheet.Cells["A1"].LoadFromDataTable(set, true);
                        }
                        var bytes = package.GetAsByteArray();
                        model.CreateReport = false;
                        model.ExcelExport = false;
                        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0}.xlsx", model.ReportName));
                    }
                }
            }

            else
            {
                ViewData["clearFields"] = "true";
            }
            
            model.CreateReport = false;
            model.ExcelExport = false;
            return View(model);
        }

        public ActionResult Report(string proc, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model;
            if (string.IsNullOrWhiteSpace(proc))
                return RedirectToAction("Index", "Report");
            if (TempData.ContainsKey("ReportViewer"))
            {
                model = TempData["ReportViewer"] as ReportViewer;
            }
            else
            {
                model = new ReportViewer()
                {
                    DisplayName = proc.Replace("report_", "").Replace("_", " "),
                    ProcedureName = proc,
                    Reports = new List<ViewModels.Dashboad2.Report>(),
                };
            }
            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            return View(model);
        }

        public ActionResult StandardRegionWiseLocReport(string region,int PageNo=1,int PageSize=10)
        {
            ReportViewer model,ddmodel;
            
            model = new ReportViewer()
            {
                DisplayName = "standard_report_Region1_wise_LOC_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Region_wise_LOC_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName="LOC_Region",FieldValue=region,FieldType="varchar"} },
                };
            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Region2_wise_LOC_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Region_wise_LOC_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                 };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel= new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        
        /// <param name="month">This parameter will contain both month and year info in MM/yyyy format</param>
        public ActionResult standardreportMonthlyStatus(string month, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;
            DateTime? monthDate = GetMonthDate(month);
            model = new ReportViewer()
            {
                DisplayName = "standard_report_Monthly_Status".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Monthly_Status",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "@month", 
                    FieldValue = monthDate.HasValue ? monthDate.Value.ToString("dd/MM/yyyy", 
                    System.Globalization.CultureInfo.InvariantCulture) : "", 
                    FieldType = "datetime" } },
            };
            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Monthly_Status".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Monthly_Status",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        /// <param name="month">This parameter will contain both month and year info in MM/yyyy format</param>
        private DateTime? GetMonthDate(string month)
        {
            if (month != null && month.Length > 0)
            {
                var dateParts = month.Split('/').ToList();
                if (dateParts.Any() && dateParts.Count() == 2)
                {
                    dateParts.Insert(0, "01");
                    string dateString = string.Join("/", dateParts);
                    try
                    {
                        DateTime date = DateTime.ParseExact(dateString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        return date;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
            else
                return null;
        }

        public ActionResult StandardLOCDetails(string region,string country,int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_LOC_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_LOC_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "region", FieldValue = region, FieldType = "varchar" }, new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_LOC_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_LOC_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult StandardGOIGuarantees(string country, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_GOI_Guarantees_Issued".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_GOI_Guarantees_Issued",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };
            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_GOI_Guarantees_Issued".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_GOI_Guarantees_Issued",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult LOCCommitments(string country, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_LOC_Commitments".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_LOC_Commitments",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };
            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_LOC_Commitments".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_LOC_Commitments",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult UnutilizedSanctions(string borrower, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Unutilized_Sanctions".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Unutilized_Sanctions",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "borrower", FieldValue = borrower, FieldType = "varchar" } },
            };
            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Unutilized_Sanctions".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Unutilized_Sanctions",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult UnutilizedContracts(string borrower, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Unutilized_Contracts".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Unutilized_Contracts",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "borrower", FieldValue = borrower, FieldType = "varchar" } },
            };
            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Unutilized_Contracts".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Unutilized_Contracts",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult YearWiseDisbursements(string region, string country, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Year_Wise_Disbursements".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Year_Wise_Disbursements",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "region", FieldValue = region, FieldType = "varchar" }, new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Year_Wise_Disbursements".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Year_Wise_Disbursements",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult ProjectDetails(string region, string country, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Project_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Project_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "region", FieldValue = region, FieldType = "varchar" }, new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Project_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Project_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult ContractDetails(string region, string country, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Contract_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Contract_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "region", FieldValue = region, FieldType = "varchar" }, new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Contract_Details".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Contract_Details",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult CompletedProjects(string region, string country, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Completed_Projects".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Completed_Projects",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "region", FieldValue = region, FieldType = "varchar" }, new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Completed_Projects".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Completed_Projects",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult PQProcessStatus(string LOC_Country="", string Project_Sector="", string PQ_Category="", string PQ_Status="",string PQ_ApplicationStart_From="", string PQ_ApplicationStart_To="", string PQ_LastSubmissionOn_From="", string PQ_LastSubmissionOn_To="", int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_PQ_Process_Status".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_PQ_Process_Status",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> {
                    new ViewModels.Dashboad2.Filter() { FilterName = "LOC_Country", FieldValue = LOC_Country, FieldType = "varchar" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "Project_Sector", FieldValue = Project_Sector, FieldType = "varchar" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "PQ_Category", FieldValue = PQ_Category, FieldType = "varchar" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "PQ_Status", FieldValue = PQ_Status, FieldType = "varchar" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "PQ_ApplicationStart_From", FieldValue = PQ_ApplicationStart_From, FieldType = "datetime" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "PQ_ApplicationStart_To", FieldValue = PQ_ApplicationStart_To, FieldType = "datetime" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "PQ_LastSubmissionOn_From", FieldValue = PQ_LastSubmissionOn_From, FieldType = "datetime" },
                     new ViewModels.Dashboad2.Filter() { FilterName = "PQ_LastSubmissionOn_To", FieldValue = PQ_LastSubmissionOn_To, FieldType = "datetime" }

                },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_PQ_Process_Status".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_PQ_Process_Status",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult OperativeLOCs(string region, string country, int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Operative_LOCs".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Operative_LOCs",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> { new ViewModels.Dashboad2.Filter() { FilterName = "region", FieldValue = region, FieldType = "varchar" }, new ViewModels.Dashboad2.Filter() { FilterName = "country", FieldValue = country, FieldType = "varchar" } },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Operative_LOCs".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Operative_LOCs",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult RepaidLOCs(string LOC_Region = "", string LOC_Country = "", string LOC_AllocatedAmount="", string LOC_DeaDate_From = "", string LOC_DeaDate_To = "", string LOC_SigningDate_From = "", string LOC_SigningDate_To = "", int PageNo = 1, int PageSize = 10)
        {
            ReportViewer model, ddmodel;

            model = new ReportViewer()
            {
                DisplayName = "standard_report_Repaid_LOCs".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Repaid_LOCs",
                Reports = new List<ViewModels.Dashboad2.Report>(),
                Filters = new List<ViewModels.Dashboad2.Filter> {
                    new ViewModels.Dashboad2.Filter() { FilterName = "LOC_Region", FieldValue = LOC_Region, FieldType = "varchar" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "LOC_Country", FieldValue = LOC_Country, FieldType = "varchar" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "LOC_AllocatedAmount", FieldValue = LOC_AllocatedAmount, FieldType = "varchar" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "LOC_DeaDate_From", FieldValue = LOC_DeaDate_From, FieldType = "datetime" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "LOC_DeaDate_To", FieldValue = LOC_DeaDate_To, FieldType = "datetime" },
                    new ViewModels.Dashboad2.Filter() { FilterName = "LOC_SigningDate_From", FieldValue = LOC_SigningDate_From, FieldType = "datetime" },
                     new ViewModels.Dashboad2.Filter() { FilterName = "LOC_SigningDate_To", FieldValue = LOC_SigningDate_To, FieldType = "datetime" }

                },
            };

            ddmodel = new ReportViewer()
            {
                DisplayName = "standard_report_Repaid_LOCs".Replace("standard_report_", "").Replace("_", " "),
                ProcedureName = "standard_report_Repaid_LOCs",
                Reports = new List<ViewModels.Dashboad2.Report>(),
            };

            model = new Dashboard2ServiceLayer().RunReport(model, PageNo, PageSize);
            ddmodel = new Dashboard2ServiceLayer().RunReport(ddmodel, PageNo, PageSize);
            ViewBag.dropdown = ddmodel;
            return View(model);
        }

        public ActionResult delReport(string proc)
        {
            ReportingService.deleteStoredProc(proc);

            return RedirectToAction("Index", "Report");
        }

        [HttpPost]
        [FormValueRequired("Search")]
        [ActionName("Report")]
        public ActionResult Report(ReportViewer model)
        {
            TempData["ReportViewer"] = model;
            model = new Dashboard2ServiceLayer().RunReport(model);
            return View(model);
        }

        [HttpPost]
        [FormValueRequired("Excel")]
        [ActionName("Report")]
        public ActionResult ReportExcel(ReportViewer model)
        {
            DataSet dataset = new Dashboard2ServiceLayer().RunExcelReport(model);
            using (ExcelPackage package = new ExcelPackage())
            {
                foreach(DataTable set in dataset.Tables)
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet 1");
                    workSheet.Cells["A1"].LoadFromDataTable(set,true);
                }
                var bytes = package.GetAsByteArray();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0}.xlsx", model.DisplayName));
            }

        }

        public ActionResult StandardRegionWiseReportExcel(ReportViewer model)
        {
            DataSet dataset = new Dashboard2ServiceLayer().RunExcelReport(model);
            using (ExcelPackage package = new ExcelPackage())
            {
                foreach (DataTable set in dataset.Tables)
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet 1");
                    workSheet.Cells["A1"].LoadFromDataTable(set, true);
                    //workSheet.Cells["E:H"].Style.Numberformat.Format = "#,##0.00";
                    int totalRows = workSheet.Dimension.End.Row;
                    int totalCols = workSheet.Dimension.End.Column;
                    var headerCells = workSheet.Cells[1, 1, 1, totalCols];
                    var headerFont = headerCells.Style.Font;
                    var allCells = workSheet.Cells[1, 1, totalRows, totalCols];
                    var headerFill = headerCells.Style.Fill;
                    headerFill.PatternType = ExcelFillStyle.Solid;
                    headerFill.BackgroundColor.SetColor(Color.Yellow);
                    allCells.AutoFitColumns();
                    headerFont.Bold = true;
                }
                var bytes = package.GetAsByteArray();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0}.xlsx", model.DisplayName));
            }

        }

        public ActionResult StandardLOCReportExcel(ReportViewer model)
        {
            DataSet dataset = new Dashboard2ServiceLayer().RunExcelReport(model);
            using (ExcelPackage package = new ExcelPackage())
            {
                foreach (DataTable set in dataset.Tables)
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet 1");
                    workSheet.Cells["A1"].LoadFromDataTable(set, true);
                    //workSheet.Cells["H:H"].Style.Numberformat.Format = "#,##0.00";
                    //workSheet.Cells["K:K"].Style.Numberformat.Format = "#,##0.00";
                    //workSheet.Cells["L:L"].Style.Numberformat.Format = "#,##0.00";
                    //workSheet.Cells["M:M"].Style.Numberformat.Format = "#,##0.00";
                    //workSheet.Cells["F:F"].Style.Numberformat.Format = "d-mmm-yy";
                    //workSheet.Cells["G:G"].Style.Numberformat.Format = "d-mmm-yy";
                    //workSheet.Cells["J:J"].Style.Numberformat.Format = "d-mmm-yy";
                    int totalRows = workSheet.Dimension.End.Row;
                    int totalCols = workSheet.Dimension.End.Column;
                    var headerCells = workSheet.Cells[1, 1, 1, totalCols];
                    var headerFont = headerCells.Style.Font;
                    var allCells = workSheet.Cells[1, 1, totalRows, totalCols];
                    var headerFill = headerCells.Style.Fill;
                    headerFill.PatternType = ExcelFillStyle.Solid;
                    headerFill.BackgroundColor.SetColor(Color.Yellow);
                    allCells.AutoFitColumns();
                    headerFont.Bold = true;
                }
                var bytes = package.GetAsByteArray();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0}.xlsx", model.DisplayName));
            }

        }

    }
}