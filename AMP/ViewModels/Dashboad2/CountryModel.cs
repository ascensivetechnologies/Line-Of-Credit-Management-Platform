using AMP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class CountryModel : MasterModel
    {
        public int Id { get; set; }

        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        [DisplayName("Region")]
        public int RegionId { get; set; }
        public List<SelectListItem> Regions {
            get
            {
                return new Dashboard2ServiceLayer().GetAllRegions().Select(x => 
                {
                    return new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    };
                }).ToList();
            }
        }
        public string RegionName { get; set; }

        [DisplayName("CIF")]
        public string CIF { get; set; }

        [DisplayName("Risk Category Classification")]
        public string RiskClassification { get; set; }
        public List<SelectListItem> RiskClassifications { get
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
        [Required(ErrorMessage = "LOC Guidelines Classification is required")]
        public string LOCClassification { get; set; }
        public List<SelectListItem> LOCClassifications {
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
            //get
            //{
            //    return new Dashboard2ServiceLayer().GetAllTerms().Select(e =>
            //    {
            //        return new SelectListItem()
            //        {
            //            Text = e.Name,
            //            Value = e.Id.ToString()
            //        };

            //    }).ToList();
            //}
        }

        [DisplayName("Interest Rate (%)")]
        public float InterestRate { get; set; }

        [DisplayName("Interest Type")]
        public string InterestType { get; set; }

        [DisplayName("Commitment Fee (%)")]
        public float CommitmentFee { get; set; }

        [DisplayName("Management Fee (%)")]
        public float ManagementFee { get; set; }

        //Interest Equalization
        [DisplayName("Interest Equalization")]
        public string Type { get; set; }
        public List<SelectListItem> Types { get {
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
        public List<SelectListItem> ApprovalTypes { get
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
        public float ICR { get; set; }

        [DisplayName("Added On")]
        public string AddedOn { get; set; }

        [DisplayName("Added By")]
        public string AddedBy { get; set; }

        [DisplayName("Red Flag")]
        public string RedFlag { get; set; }

        public List<SelectListItem> RedFlagOptions
        {
            get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.RedFlag).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
            }
        }

    }
}