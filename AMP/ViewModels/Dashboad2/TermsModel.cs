using AMP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class TermsModel : MasterModel
    {
        public TermsModel()
        {
            ICR = 75;
        }

        public int Id { get; set; }

        [DisplayName("Risk Category Classification")]
        public string RiskClassification { get; set; }
        public List<SelectListItem> RiskClassifications
        {
            get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.Risk).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
            }
        }

        [DisplayName("LOC Guidelines Classification")]
        public string LOCClassification { get; set; }
        public List<SelectListItem> LOCClassifications
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

        [DisplayName("Interest Rate (%)")]
        public double InterestRate { get; set; }

        [DisplayName("Interest Type")]
        public string InterestType { get; set; }
        

        [DisplayName("Commitment Fee (%)")]
        public double CommitmentFee { get; set; }

        [DisplayName("Management Fee (%)")]
        public double ManagementFee { get; set; }

        //Interest Equalization
        [DisplayName("Management Fee (%)")]
        public string Type { get; set; }
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

        [DisplayName("Percentage (%)")]
        public double Percentage { get; set; }

        [DisplayName("Approval")]
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

        [DisplayName("Special Condtions")]
        public string SpecialConditions { get; set; }

        [DisplayName("Tenure in Years")]
        public int YearTenure { get; set; }

        [DisplayName("Moratorium in Years")]
        public int MoratoriumYears { get; set; }

        [DisplayName("Indian Content Requirement (%)")]
        public double ICR { get; set; }
    }
}