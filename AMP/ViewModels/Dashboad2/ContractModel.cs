using AMP.Models;
using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class ContractModel
    {
        public int Id { get; set; }
        public int PQId { get; set; }
        public string ContractorCIF { get; set; }
        public string ContractorName { get; set; }
        public string ContractStart { get; set; }
        public string ContractEnd { get; set; }
        public string CGSId { get; set; }
        public string ContractType { get; set; }
        public string Description { get; set; }
        public string TenderIssueDate { get; set; }
        public string TenderLastDate { get; set; }
        public string PACDate { get; set; }
        public string DefectsLiabilityEndDate { get; set; }
        public string FACDate { get; set; }
        public string CompReportDate { get; set; }
        public decimal AdvPmtGrntAmount { get; set; }
        public string AdvPmtGrntExpiry { get; set; }
        public decimal PerBankGrntAmount { get; set; }
        public string PerBankGrntExpiry { get; set; }
        public decimal OtherGrntAmount { get; set; }
        public string OtherGrntExpiry { get; set; }
        public string GuaranteeNote { get; set; }
        public string ContractId { get; set; }
        public decimal EstimateValue { get; set; }
        public decimal AmountDisbursed { get; set; }
        public string PackageDisplayId { get; set; }
        public string TypeOfPackage { get; set; }
        public string CNote { get; set; }
        public string ScheduledCompDate { get; set; }
        public string SigningDate { get; set; }
        public string SignEffectiveDate { get; set; }
        public int DurationYear { get; set; }
        public int DurationMonth { get; set; }
        public int DurationDay { get; set; }
        public int ProjectId { get; set; }
        public ProjectPqModel ProjectPqDetail { get; set; }
        public TBL_Projects Project { get; set; }// { return new Dashboard2ServiceLayer().GetAllProjects("").ToList().Where(x => x.Id == this.ProjectPqDetail.ProjectId).FirstOrDefault(); } }
        public List<TBL_Files> Files { get; set; }

        #region Amount
        public decimal? LocAmount { get; set; }
        public decimal? BorowerGovt { get; set; }
        public decimal? OtherSources { get; set; }
        #endregion

        public List<ApplicantsModel> Applicants { get; set; }

        public List<ResponsibilityModel> Responsibility { get; set; }

        public List<PaymentTermsModel> PaymentTerms { get; set; }

        public List<ContentRequirementModel> Content { get; set; }

        public List<CreditLetterModel> LetterOfCredits { get; set; }

        public List<LocMapModel> Locs { get; set; }

        public List<SelectListItem> ContractTypes
        {
            get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.ContractType).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
            }
        }

        public List<ActivityModel> Activities { get
            {
                return new Dashboard2ServiceLayer().GetAllActivity("TBl_Contract", this.Id);
            }
        }

        public List<SelectListItem> LocNumbers { get
            {
                return new Dashboard2ServiceLayer().GetAllLocs().Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.LOCNumber,
                        Value = x.Id.ToString()
                    };
                }).ToList();
            } }

        public string ContractApprovalDate { get;  set; }
        public string RevisedCompletionDate { get;  set; }
        public string ActualCompletionDate { get;  set; }
        public string TerminalDateOfDisbursement { get;  set; }
        public string DateOfReceiptOfContractByEximBank { get; set; }   

        public ContractModel()
        {
            ProjectPqDetail = new ProjectPqModel();
            Files = new List<TBL_Files>();
        }

    }
    public class LocMapModel
    {
        public int ContractId { get; set; }
        public int LocId { get; set; }
        public string LocNumber { get; set; }
        public LOCModel Loc { get; set; }
        public DateTime DateAdded { get; set; }
        public decimal? Value { get; set; }
    }

}