using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class ReportViewer
    {
        public string ProcedureName { get; set; }
        public string DisplayName { get; set; }
        public List<Filter> Filters { get; set; }
        public List<Report> Reports { get; set; }
    }

    public class Report
    {
        public int Id { get; set; }
        public List<string> Columns { get; set; }
        public GridModel<ReportRow> Rows { get; set; }
    }

    public class ReportRow
    {
        public List<string> Rows { get; set; }
    }

    public class Filter
    {
        public string FilterName { get; set; }
        public string DisplayName {
            get
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                return textInfo.ToTitleCase(this.FilterName.Replace("@", "").Replace("_"," "));
            }
        }
        public string FieldType { get; set; }
        public bool isNullable { get; set; }
        public string FieldValue { get; set; }
        public bool hasDefault { get; set; }
        public int Sequence { get; set; }
    }
    
}