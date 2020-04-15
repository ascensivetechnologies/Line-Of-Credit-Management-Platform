using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class PqDetailPageModel
    {
        public ProjectPqModel PqDetail { get; set; }
        public List<SelectListItem> PQCategories
        {
            get
            {
                var list = new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.ContractType).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
                list.Insert(0, new SelectListItem { Text = "-Select-", Value = "" });
                return list;
            }
        }
        public List<SelectListItem> ApplicantStatuses
        {
            get
            {
                var list = new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.ApplicantStatus).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Id.ToString()
                    };
                }).ToList();
                return list;
            }
        }
        public List<SelectListItem> PQStatuses
        {
            get
            {
                var list = new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.PQStatus).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Id.ToString()
                    };
                }).ToList();
                return list;
            }
        }
        public List<ActivityModel> Activity
        {
            get; set;
        }

        public PqDetailPageModel()
        {
            PqDetail = new ProjectPqModel();
        }
    }
}