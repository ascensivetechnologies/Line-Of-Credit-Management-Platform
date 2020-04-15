using AMP.Models;
using AMP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class LOCModel
    {
        public int Id { get; set; }

        [DisplayName("OM Number")]
        public string OMNumber { get; set; }

        public string LOCName { get; set; }

        [DisplayName("Sanction Amount")]
        public string SanctionAmount { get; set; }

        [DisplayName("Amount Allocated")]
        public string AmountAllocated { get; set; }

        [DisplayName("Associated Projects")]
        public int AssociatedProjects
        {
            get; set;
        }

        //[DisplayName("GOILOC-182 -")]
        public string LOCNumber { get; set; }

        public string LOCAccountNumber { get; set; }

        public int? CountryId { get; set; }

        [DisplayName("Country")]
        public string CountryName { get; set; }

        //[DisplayName("Description")]
        public string LocPurpose { get; set; }

        [DisplayName("Approved By")]
        public int? ApprovedBy { get; set; }

        [DisplayName("Approved On")]
        public string ApprovedOn { get; set; }


        //Loc Terms
        [DisplayName("Terminal date of signing LOC")]
        public string TerminalDate { get; set; }

        [DisplayName("Indian Content Requirement")]
        public float ICR { get; set; }

        [DisplayName("Date of MD Approval")]
        public string MDDate { get; set; }

        [DisplayName("Date of MEA/DEA Go Ahead")]
        public string MeaDate { get; set; }

        [DisplayName("Date of DEA Go Ahead")]
        public string DeaDate { get; set; }

        [DisplayName("Date of Offer Letter")]
        public string OfferLetterDate { get; set; }

        [DisplayName("Interest Rate")]
        public float InterestRate { get; set; }

        [DisplayName("Interest Type")]
        public string InterestType { get; set; }

        [DisplayName("Committment Fees")]
        public float CommitmentFee { get; set; }

        [DisplayName("Management Fees")]
        public float ManagementFee { get; set; }

        public string SignedDate { get; set; }
        public DateTime? _SignedDate { get; set; }

        public string InterestEqualization { get; set; }
        public string SpecialCondition { get; set; }

        //Finacle data
        public string DisbursementUnderLoc { get; set; }
        public string AmountDisbursed { get; set; }
        public string RepaymentDueDate { get; set; }
        public string RepaymentDueDate2 { get; set; }

        public string InterestDueDate { get; set; }
        public string InterestDueDate2 { get; set; }

        public string PrincipalRepaymentDate { get; set; }

        public string PrincipalRepaymentDateEnd { get; set; }

        public string LoanOutstanding { get; set; }
        public string TotalDisbursed { get; set; }
        public string PrincipalDemand { get; set; }
        public string PrincipalCollection { get; set; }
        public string PrincipalOverdue { get; set; }
        public string InterestDemand { get; set; }
        public string InterestCollection { get; set; }
        public string InterestOverdue { get; set; }
        public string BalanceConfirmationDate { get; set; }

        //Interset Equalization
        public string MeaType { get; set; }
        public string DeaType { get; set; }
        public List<SelectListItem> Types
        {
            get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.InterestType).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
            }
        }

        public float MeaPercentage { get; set; }
        public float DeaPercentage { get; set; }

        public string ApprovalType { get; set; }
        public List<SelectListItem> ApprovalTypes
        {
            get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.InterestApproval).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
            }
        }

        //End

        [DisplayName("Tenure")]
        public int? TenureYears { get; set; }

        [DisplayName("Moratorium")]
        public int? Moratorium { get; set; }

        public int? AmmendmentNumber { get; set; }
        public string GOIDeedDate { get; set; }
        public string EffectiveDate { get; set; }
        public string AgreementAmmendDate { get; set; }
        public string Classification { get; set; }
        public List<SelectListItem> Classifications
        {
            get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.GuideLines).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
            }
        }
        //End
        public decimal Utilization { get; set; }
        public bool status { get; set; }
        public RegionModel Region { get; set; }
        public string CIF { get; set; }

        public string VerificationNote { get; set; }
        public string AmendmentNote { get; set; }
        public List<ProjectModel> Projects { get; set; }
        public List<BalanceModel> Balanaces { get; set; }
        public List<ActivityModel> RecentActivty
        {
            get
            {
                return new Dashboard2ServiceLayer().GetAllActivity("TBL_Loc", this.Id);
            }
        }
        public List<TBL_Files> Files { get; set; }
        public List<AmendmentsModel> Amendments { get; set; }
        public List<SelectListItem> Countries { get; set; }
        //public List<SelectListItem> Classifications { get; set; }

        public List<LocProjectGridModal> LocProjectGridData { get; set; }

        public List<TeamMappingModel> TeamMembers
        {
            get
            {
                return new Dashboard2ServiceLayer().GetTeam("TBL_LOC", Id);
            }
        }

        public List<SelectListItem> AllUsers
        {
            get
            {
                return new Dashboard2ServiceLayer().GetAllUsers("").Select(e =>
                {
                    return new SelectListItem
                    {
                        Text = string.Format("{0}-({1})", e.DisplayName, e.Username),
                        Value = e.Id.ToString()
                    };
                }).ToList();
            }
        }

        public class LocProjectGridModal
        {
            public string ProjectId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Added { get; set; }
            public decimal ProjectValue { get; set; }
            public decimal Allocation { get; set; }
            public float FinancialProgress { get; set; }
            public float PhysicalProgress { get; set; }
        }

        public LOCModel()
        {
            Countries = new List<SelectListItem>();
            //Classifications = new List<SelectListItem>();
            LocProjectGridData = new List<LocProjectGridModal>();
            Files = new List<TBL_Files>();
        }
    }

    public class LOCoption {
        public int LOCid { get; set; }
        public string LOCName { get; set; }
    }
}