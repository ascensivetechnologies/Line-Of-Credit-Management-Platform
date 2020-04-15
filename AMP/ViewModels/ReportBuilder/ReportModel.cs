using AMP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.ReportBuilder
{
    public class ReportModel
    {
        private TextInfo textInfo
        {
            get
            {
                return new CultureInfo("en-US", false).TextInfo;
            }
        }
        public string ReportName { get; set; }

        public string FY { get; set; }
        public string FYValue { get; set; }

        public string SelectedEntity { get; set; }
        public List<SelectListItem> Entites { get
            {
                return ReportingService.GetEntities().Select(e =>
                {
                    return new SelectListItem()
                    {
                        Text = textInfo.ToTitleCase(e.Replace("report_", "").Replace("_", "")),
                        Value = e
                    };
                }).ToList();
            }
        }

        public string SelectedFields { get; set; }
        public string GroupBySelectedFields { get; set; }
        public List<SelectListItem> Fields { get
            {
                return ReportingService.GetFieldsForEntity(SelectedEntity).Select(e =>
                {
                    return new SelectListItem()
                    {
                        //Text = textInfo.ToTitleCase(e.Value),
                        Text = e.Value,
                        //Value = e.Value
                        Value = e.Key
                    };
                }).ToList();
            }
        }

        public List<Filter> Filters { get; set; }
        public bool AdvancedMode { get; set; }

        public List<FieldType> GroupByFields { get
            {
                if (!string.IsNullOrEmpty(this.SelectedEntity))
                {
                    return ReportingService.GetFieldsForEntityGroups(this.SelectedEntity);
                }
                else
                {
                    return new List<FieldType>();
                }
            }
        }
        public List<GroupBy> GroupBys { get; set; }
        public List<GroupByFilter> GroupByFilters { get; set; }
       
        public List<string> Errors { get; set; }
        public bool CreateReport { get; set; }
        public bool ExcelExport { get; set; }

    }

    public class Filter
    {
        public string FieldName { get; set; }
        public GroupOperationsType WhereFilter { get; set; }
        public string Value { get; set; }
        public bool Removed { get; set; }
        public FilterDataType FilterType { get; set; }
    }

    public class GroupBy : Filter
    {
        public GroupByTypes Types { get; set; }

    }

    public class GroupByFilter : Filter
    {
        public GroupByTypes Operand { get; set; }
        public GroupOperationsType Operator { get; set; }
    }

    public class WhereOperator
    {
        public GroupOperationsType Operator { get; set; }
    }
    
    public enum GroupByTypes
    {
        [Display(Name ="Sum")]
        Sum,
        [Display(Name = "Max")]
        Max,
        [Display(Name = "Min")]
        Min,
        [Display(Name = "Count")]
        Count,
        [Display(Name = "Avg")]
        Avg
    }

    public enum GroupOperationsType
    {
        //[Display(Name = ">")]
        //GreaterThan,
        //[Display(Name = ">=")]
        //GreaterThanEqual,
        //[Display(Name = ">")]
        //LessThan,
        //[Display(Name = "<=")]
        //LessThanEqual,
        //[Display(Name = "!=")]
        //NotEqualTo,
        [Display(Name = "=")]
        EqualTo,
        [Display(Name = "like")]
        Like
    }

    public enum FilterDataType
    {
        [Display(Name = "Datetime")]
        Datetime,
        [Display(Name = "Number")]
        Number,
        [Display(Name = "Text")]
        Text
    }

    public class FieldType
    {
        public string FieldName { get; set; }
        public string Type { get; set; }
        public List<GroupByTypes> ApplicableTypes { get; set; }
    }

    public class DictionaryResult
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }
    
}