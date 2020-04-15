using AMP.Models;
using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class MapReportModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string FullMessage { get; set; }
        public string ServiceName { get; set; }
        public string CreatedOn { get; set; }

        public List<Markers> Locations
        {
            get
            {
                return new Dashboard2ServiceLayer().GetLocations("TBL_Project", Id);
            }
        }

        public class Markers
        {
            public string lat { get; set; }
            public string lon { get; set; }
            public string color { get; set; }
            public string maptype { get; set; }
            public string id { get; set; }
            public string info1 { get; set; }
            public string info2 { get; set; }
            public string info3 { get; set; }
            public string sector { get; set; }
        }
    }
}