using AMP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DPRDate { get; set; }
        public string BaselineData { get; set; }
        public int Stage { get; set; }
        public string Sector { get; set; }
        public List<SelectListItem> Sectors
        {
            get
            {
                return new Dashboard2ServiceLayer().GetOptionsByType(OptionTypes.Sector).Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value
                    };
                }).ToList();
            }
        }

        public string SubSectorName { get; set; }
        public string Address { get; set; }
        public int SubSector { get; set; }
        //public string SubSectorFaIcon { get
        //    {
        //        return new Dashboard2ServiceLayer().GetAllOptions(this.SubSector, OptionTypes.SubSector)
        //    }
        //}
        public int Status { get; set; }
        public string ProjectStatus { get; set; }
        public decimal ProjectValue { get; set; }
        public string StartDateTxt { get; set; }
        public string EndDateTxt { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public string Note { get; set; }

        public decimal ProjectProgress { get; set; }

        public List<ProjectTimeline> TimeLines { get; set; }
        public List<LOCModel> Locs { get; set; }
        
    }
    public class ProjectTimeline
    {
        public string Message { get; set; }
        public string Date { get; set; }
    }
}