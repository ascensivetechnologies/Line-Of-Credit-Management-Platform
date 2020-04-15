using AMP.Models;
using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class ProjectDetailModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public decimal ProjectValue { get; set; }
        public string Duration { get; set; }
        public decimal AmountDisbursed { get; set; }
        public int Status { get; set; }
        public string SectorName { get; set; }
        public string SubsectorName { get; set; }
        public string SectorFaIcon
        {
            get
            {
                return (new Dashboard2ServiceLayer().GetAllOptions(Search: this.SectorName, type: OptionTypes.Sector).FirstOrDefault() ?? new Options()).FaIcon;
            }
        }

        public string Address { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lon { get; set; }
        public string CountriesName { get; set; }
        public string Note { get; set; }

        public List<ProjectContractModel> ProjectContracts { get; set; }
        public List<TBL_Status> ProjectStatuses { get; set; }
        public List<ProjectLocModel> Locs { get; set; }
        public List<ContactModel> ProjectContacts { get; set; }
        public List<CountryModel> Countries { get; set; }
        public List<TBL_ContactTypes> ContactTypes { get; set; }
        public List<ProjectTimelineModel> TimeLines { get; set; }
        public List<ProjectLocationModel> Locations { get; set; }

        public List<ProjectPqModel> ProjectPQs { get; set; }

        public List<TBL_Files> Files { get; set; }

        public List<ActivityModel> Activity { get
            {
                return new Dashboard2ServiceLayer().GetAllActivity("TBL_Project", this.Id);
            }
        }

        public List<TeamMappingModel> TeamMembers
        {
            get
            {
                return new Dashboard2ServiceLayer().GetTeam("TBL_Project", Id);
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

        public class ProjectLocModel
        {
            public string LocName { get; set; }
            public string Country { get; set; }
            public string Added { get; set; }
            public string Classification { get; set; }
            public string AccountNumber { get; set; }
            public decimal AllocatedValue { get; set; }
            public string LocId { get; set; }
            public int LinkId { get; set; }
        }

        public class ProjectLocationModel
        {
            public int SequenceNumber { get; set; }
            public int? Deleted { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Color { get; set; }
            public string MapType { get; set; }
            public string ProjectId { get; set; }
        }

        public class Markers
        {
            public string lat { get; set; }
            public string lon { get; set; }
            public string color { get; set; }
            public string maptype { get; set; }
            public string id { get; set; }
        }

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
        public ProjectDetailModel()
        {
            Locs = new List<ProjectLocModel>();
            TimeLines = new List<ProjectTimelineModel>();
            ContactTypes = new List<TBL_ContactTypes>();
            Countries = new List<CountryModel>();
            ProjectStatuses = new List<TBL_Status>();
            Files = new List<TBL_Files>();
            Locations = new List<ProjectLocationModel>();
            ProjectPQs = new List<ProjectPqModel>();
        }
    }

    
}