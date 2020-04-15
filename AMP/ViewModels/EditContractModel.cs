using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class EditContractModel
    {
        public int ProjectId { get; set; }
        public List<SelectListItem> ContractTypes { get
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

        public ContractModel Contract { get; set; }

        public EditContractModel()
        {
            Contract = new ContractModel();
        }
    }
}