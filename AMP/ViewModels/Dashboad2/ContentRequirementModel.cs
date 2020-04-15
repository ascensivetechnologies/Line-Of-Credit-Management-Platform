using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class ContentRequirementModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public decimal Value { get; set; }
        public decimal? Percentage { get; set; }
        public bool isExempt { get; set; }
        public string RevisedRequirement { get; set; }
        public string MEAApprovalRefNo { get; set; }
        public string MEAApprovalDate { get; set; }
        public string Remarks { get; set; }

        public string Type { get; set; }
        public List<SelectListItem> Types { get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.ContentRequirement).ToList().Select(x =>
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