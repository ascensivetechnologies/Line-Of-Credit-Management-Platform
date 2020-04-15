using AMP.Models;
using AMP.ViewModels.ReportBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using AMP.Extensions;

namespace AMP.Services
{
    public static class ReportingService
    {

        public static List<string> GetEntities(string Search = "")
        {
            using (AMPEntities entity = new AMPEntities())
            {
                string sql = @"select TABLE_NAME from INFORMATION_SCHEMA.VIEWS where TABLE_NAME like 'report_%'";
                if (!string.IsNullOrWhiteSpace(Search))
                {
                    sql += string.Format("and TABLE_NAME like '%s'", Search);
                }
                var views = entity.Database.SqlQuery<string>(sql).ToList();

                return views;
            }
        }

        public static Dictionary<string, string> GetFieldsForEntity(string EntityName)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                string sql = string.Format("select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '{0}' order by COLUMN_NAME", EntityName);
                var views = entity.Database.SqlQuery<string>(sql).ToList();
                Dictionary<string, string> stringmapper = stringMapper();
                Dictionary<string, string> fieldswithkey = new Dictionary<string, string>();
                foreach (string item in views)
                {
                    if (stringmapper.ContainsKey(item))
                    {
                        fieldswithkey.Add(item, stringmapper[item]);
                    }
                    else
                    {
                        fieldswithkey.Add(item, item);
                    }
                }

                return fieldswithkey;
            }
        }

        public static List<FieldType> GetFieldsForEntityGroups(string EntityName)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                string sql = string.Format("select COLUMN_NAME as FieldName,DATA_TYPE as Type from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '{0}'", EntityName);

                var views = entity.Database.SqlQuery<FieldType>(sql).ToList();
                views = views.Select(e =>
                {
                    var record = new FieldType()
                    {
                        Type = e.Type,
                        FieldName = e.FieldName,
                        ApplicableTypes = new List<GroupByTypes>()
                    };

                    record.ApplicableTypes.Add(GroupByTypes.Count);
                    record.ApplicableTypes.Add(GroupByTypes.Max);
                    record.ApplicableTypes.Add(GroupByTypes.Min);

                    if (!(e.Type.ToLower().Contains("varchar") || e.Type.ToLower().Contains("date")))
                    {
                        record.ApplicableTypes.Add(GroupByTypes.Sum);
                        record.ApplicableTypes.Add(GroupByTypes.Avg);
                    }

                    return record;

                }).ToList();
                return views;
            }
        }

        public static DataSet RunExcelReport(ReportModel model)
        {
            bool groupByEnabled = false;
            DataSet set = new DataSet();
            Dictionary<string, string> mapper = GetFieldsForEntity(model.SelectedEntity);
            try
            {

                string connStr = ConfigurationManager.ConnectionStrings["ARIESConnectionString"].ConnectionString;
                string FY = "", FYCond = "";
                if (!string.IsNullOrEmpty(model.FY))
                {
                    FY = string.Format(@"(case when isdate([{0}])=1 then
                                        (case when month([{0}]) < 4 then convert(varchar(4), year([{0}]) - 1) + '-' + convert(varchar(4), year([{0}]))
                                            when month([{0}]) >= 4 then convert(varchar(4), year([{0}])) + '-' + convert(varchar(4), year([{0}) + 1)
                                        else '' end)
                                        else ''
                                        end) FinancialYear,", model.FY);
                }

                if (!string.IsNullOrEmpty(model.FYValue) && !string.IsNullOrEmpty(model.FY))
                {
                    //ViewModels.ReportBuilder.Filter f = new ViewModels.ReportBuilder.Filter();
                    //f.FieldName = FY;
                    //f.Value = model.FYValue;
                    //f.WhereFilter = GroupOperationsType.EqualTo;
                    //model.Filters.Add(f);
                    FYCond = string.Format(@"(case when isdate([{0}])=1 then
                                        (case when month([{0}]) < 4 then convert(varchar(4), year([{0}]) - 1) + '-' + convert(varchar(4), year([{0}]))
                                            when month([{0}]) >= 4 then convert(varchar(4), year([{0}])) + '-' + convert(varchar(4), year([{0}]) + 1)
                                        else '' end)
                                        else ''
                                        end) ", model.FY);
                }

                string conditions = string.Join(" and ", (model.Filters ?? new List<ViewModels.ReportBuilder.Filter>()).Select(e => (e.WhereFilter.GetDisplayName() == "like" ? string.Format("([{0}] like '%{1}%')", e.FieldName, e.Value) : string.Format("([{0}] {1} {2})", e.FieldName, e.WhereFilter.GetDisplayName(), e.Value))).ToList());
                if (!string.IsNullOrWhiteSpace(FYCond))
                {
                    if (!string.IsNullOrWhiteSpace(conditions))
                        conditions += " and " + FYCond.Remove(FYCond.Length - 1, 1) + "='" + model.FYValue + "'";
                    else
                        conditions += FYCond.Remove(FYCond.Length - 1, 1) + "='" + model.FYValue + "'";

                }

                string fieldsForGroupByClause = string.Join(",", (model.GroupBySelectedFields ?? "").Split(',').Select(e => string.Format("[{0}]", e)).ToList());
                string fieldsToDisplay = "";
                string groupByClause = "";

                if (groupByEnabled)
                {
                    if (model.GroupBySelectedFields != null)
                    {
                        if (model.SelectedFields.Contains(','))
                        {
                            foreach (string s in model.SelectedFields.Split(','))
                            {
                                if (fieldsForGroupByClause.Contains(s))
                                {
                                    fieldsToDisplay += s + ",";

                                }
                            }
                            if (!string.IsNullOrWhiteSpace(fieldsToDisplay))
                                fieldsToDisplay = fieldsToDisplay.Remove(fieldsToDisplay.Length - 1, 1);
                        }
                        else
                        {
                            fieldsToDisplay += model.SelectedFields;

                        }

                    }
                    else
                        fieldsToDisplay = model.SelectedFields;

                    groupByClause = string.Format("group by {0}", fieldsForGroupByClause);
                }
                else
                {
                    foreach (string s in model.SelectedFields.Split(','))
                    {
                        if (!model.GroupBys.Any(e => e.FieldName == s))
                        {
                            fieldsToDisplay += s + ",";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(fieldsToDisplay))
                        fieldsToDisplay = fieldsToDisplay.Remove(fieldsToDisplay.Length - 1, 1);

                    groupByClause = string.Format("group by {0}", fieldsToDisplay);
                }


                string fields = string.Join(",", (fieldsToDisplay ?? "").Split(',').Select(e => (e == "" ? "" : string.Format("[{0}] as [{1}]", e, mapper[e]))).ToList());
                string groups = string.Format(",{0}", string.Join(",", (model.GroupBys ?? new List<GroupBy>()).Select(e => string.Format("{0}([{1}]) as [{0}({2})]", e.Types.ToString(), e.FieldName, mapper[e.FieldName])).ToList()));


                //string groupByClause = string.Format("group by {0} {1}", fieldsForGroupByClause,
                //    (model.GroupByFilters ?? new List<GroupByFilter>()).Any() ?
                //    string.Format("having {0}", string.Join(" and ", ((model.GroupByFilters ?? new List<GroupByFilter>()).Select(e => string.Format("{0}([{1}]) {2} '{3}'", e.Operand.ToString(), e.FieldName, e.Operator.GetDisplayName(), e.Value)).ToList())))
                //    : ""
                //    );


                //string groupByClause = string.Format("group by {0}", fieldsForGroupByClause);


                if ((model.GroupBys ?? new List<GroupBy>()).Any())
                    fields += groups;

                if (fields[0] == ',')
                {
                    fields = fields.Remove(0, 1);
                }

                //string sql = string.Format("select distinct {0} from {1} where {2} {3}", fields, model.SelectedEntity, string.IsNullOrEmpty(conditions) ? "1 = 1" : conditions, ((model.GroupBys ?? new List<GroupBy>()).Any() || (model.GroupByFilters ?? new List<GroupByFilter>()).Any()) ? groupByClause : "");
                string sql = string.Format("select distinct {0}{1} from {2} where {3} {4}", FY, fields, model.SelectedEntity, string.IsNullOrEmpty(conditions) ? "1 = 1" : conditions,
                    //(model.GroupBySelectedFields != null ? groupByClause : ""));
                    (((model.GroupBys != null && model.GroupBys.Count > 0) || model.GroupBySelectedFields != null) ? groupByClause : ""));
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand sqlComm = new SqlCommand(sql, conn);

                    //sqlComm.CommandType = CommandType.TableDirect;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(set);
                }
            }
            catch (Exception ex)
            {
                model.Errors = new List<string>();
                model.Errors.Add(ex.Message);
            }

            return set;
        }

        public static void CreateReport(ReportModel model)
        {
            try
            {
                bool groupByEnabled = false;
                Dictionary<string, string> mapper = GetFieldsForEntity(model.SelectedEntity);
                string FY = "", FYCond = "";
                if (!string.IsNullOrEmpty(model.FY))
                {
                    FY = string.Format(@"(case when isdate([{0}])=1 then
                                        (case when month([{0}]) < 4 then convert(varchar(4), year([{0}]) - 1) + '-' + convert(varchar(4), year([{0}]))
                                            when month([{0}]) >= 4 then convert(varchar(4), year([{0}])) + '-' + convert(varchar(4), year([{0}]) + 1)
                                        else '' end)
                                        else ''
                                        end) FinancialYear,", model.FY);
                }

                if (!string.IsNullOrEmpty(model.FYValue) && !string.IsNullOrEmpty(model.FY))
                {
                    //ViewModels.ReportBuilder.Filter f = new ViewModels.ReportBuilder.Filter();
                    //f.FieldName = FY;
                    //f.Value = model.FYValue;
                    //f.WhereFilter = GroupOperationsType.EqualTo;
                    //model.Filters.Add(f);
                    FYCond = string.Format(@"(case when isdate([{0}])=1 then
                                        (case when month([{0}]) < 4 then convert(varchar(4), year([{0}]) - 1) + '-' + convert(varchar(4), year([{0}]))
                                            when month([{0}]) >= 4 then convert(varchar(4), year([{0}])) + '-' + convert(varchar(4), year([{0}]) + 1)
                                        else '' end)
                                        else ''
                                        end) ", model.FY);
                }
                string filters = "";//string.Join(",", (model.Filters ?? new List<ViewModels.ReportBuilder.Filter>()).Select(e => string.Format("@{0} varchar(max) = ''", e.FieldName.Replace(' ', '_'))).ToList());

                string conditions = "";//string.Join(" and ", (model.Filters ?? new List<ViewModels.ReportBuilder.Filter>()).Select(e => e.WhereFilter.GetDisplayName().Equals("like") ? string.Format("(isnull(@{2},'') = '' or [{1}] like '%'+@{2}+'%')", e.Value, e.FieldName, e.FieldName.Replace(' ', '_')) : string.Format("(isnull(@{2},'') = '' or CONVERT(varchar, [{1}]) {2} {3})", e.Value, e.FieldName, e.WhereFilter.GetDisplayName(), e.FieldName.Replace(' ', '_'))).ToList());

                foreach (var e in model.Filters)
                {
                    if (e.FilterType.GetDisplayName() == "Datetime")
                    {
                        filters += (filters != "" ? ", " : "") + string.Format("@{3}_From datetime = null,  @{3}_To datetime = null", e.Value, e.FieldName, e.WhereFilter.GetDisplayName(), e.FieldName.Replace(' ', '_'));
                        conditions += (conditions != "" ? " \n and " : "") + string.Format("(@{3}_From is null or @{3}_From <= [{1}]) and (@{3}_To is null or @{3}_To >= [{1}]) ", e.Value, e.FieldName, e.WhereFilter.GetDisplayName(), e.FieldName.Replace(' ', '_'));
                    }
                    else if (e.FilterType.GetDisplayName() == "Number")
                    {
                        filters += (filters != "" ? ", " : "") + string.Format("@{3}_From float = null,  @{3}_To float = null", e.Value, e.FieldName, e.WhereFilter.GetDisplayName(), e.FieldName.Replace(' ', '_'));
                        conditions += (conditions != "" ? " \n and " : "") + string.Format("(@{3}_From is null or @{3}_From <= [{1}]) and (@{3}_To is null or @{3}_To >= [{1}]) ", e.Value, e.FieldName, e.WhereFilter.GetDisplayName(), e.FieldName.Replace(' ', '_'));
                    }
                    else if (e.FilterType.GetDisplayName() == "Text")
                    {
                        filters += (filters != "" ? ", " : "") + string.Format("@{3} varchar(max) = ''", e.Value, e.FieldName, e.WhereFilter.GetDisplayName(), e.FieldName.Replace(' ', '_'));
                        conditions += (conditions != "" ? " \n and " : "") + (e.WhereFilter.GetDisplayName().Equals("like") ? string.Format("(isnull(@{2},'') = '' or [{1}] like '%'+@{2}+'%')", e.Value, e.FieldName, e.FieldName.Replace(' ', '_')) : string.Format("(isnull(@{3},'') = '' or CONVERT(varchar, [{1}]) {2} {3})", e.Value, e.FieldName, e.WhereFilter.GetDisplayName(), e.FieldName.Replace(' ', '_')));
                    }
                }

                if (!string.IsNullOrWhiteSpace(FYCond))
                {
                    if (!string.IsNullOrWhiteSpace(conditions))
                        conditions += " and " + FYCond.Remove(FYCond.Length - 1, 1) + "='" + model.FYValue + "'";
                    else
                        conditions += FYCond.Remove(FYCond.Length - 1, 1) + "='" + model.FYValue + "'";

                }
                string fieldsForGroupByClause = string.Join(",", (model.GroupBySelectedFields ?? "").Split(',').Select(e => string.Format("[{0}]", e)).ToList());
                string fieldsToDisplay = "";
                string groupByClause = "";

                if (groupByEnabled)
                {
                    if (model.GroupBySelectedFields != null)
                    {
                        if (model.SelectedFields.Contains(','))
                        {
                            foreach (string s in model.SelectedFields.Split(','))
                            {
                                if (fieldsForGroupByClause.Contains(s))
                                {
                                    fieldsToDisplay += s + ",";

                                }
                            }
                            if (!string.IsNullOrWhiteSpace(fieldsToDisplay))
                                fieldsToDisplay = fieldsToDisplay.Remove(fieldsToDisplay.Length - 1, 1);
                        }
                        else
                        {
                            fieldsToDisplay += model.SelectedFields;

                        }

                    }
                    else
                        fieldsToDisplay = model.SelectedFields;

                    groupByClause = string.Format("group by {0}", fieldsForGroupByClause);
                }
                else
                {
                    foreach (string s in model.SelectedFields.Split(','))
                    {
                        if (!model.GroupBys.Any(e => e.FieldName == s))
                        {
                            fieldsToDisplay += s + ",";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(fieldsToDisplay))
                        fieldsToDisplay = fieldsToDisplay.Remove(fieldsToDisplay.Length - 1, 1);

                    groupByClause = string.Format("group by {0}", fieldsToDisplay);
                }

                //string fields = string.Join(",", model.SelectedFields.Split(',').Select(e => string.Format("[{0}]", e)).ToList());
                //string groups = string.Format(",{0}", string.Join(",", (model.GroupBys ?? new List<GroupBy>()).Select(e => string.Format("{0}([{1}]) as [{0}({1})]", e.Types.ToString(), e.FieldName)).ToList()));
                string fields = string.Join(",", (fieldsToDisplay ?? "").Split(',').Select(e => e == "" ? "" : string.Format("[{0}] as [{1}]", e, mapper[e])).ToList());
                string groups = string.Format(",{0}", string.Join(",", (model.GroupBys ?? new List<GroupBy>()).Select(e => string.Format("{0}([{1}]) as [{0}({2})]", e.Types.ToString(), e.FieldName, mapper[e.FieldName])).ToList()));

                //string groupByClause = string.Format("group by {0} {1}", fields,
                //    (model.GroupByFilters ?? new List<GroupByFilter>()).Any() ?
                //    string.Format("having {0}", string.Join(" and ", ((model.GroupByFilters ?? new List<GroupByFilter>()).Select(e => string.Format("{0}([{1}]) {2} '{3}'", e.Operand.ToString(), e.FieldName, e.Operator.GetDisplayName(), e.Value)).ToList()))) : ""
                //    );


                if ((model.GroupBys ?? new List<GroupBy>()).Any())
                    fields += groups;

                if (fields[0] == ',')
                {
                    fields = fields.Remove(0, 1);
                }

                //    string sql1 = string.Format(@"

                //        Create PROCEDURE [dbo].[report_{0}]
                //        {1}
                //        AS
                //        BEGIN                    
                //            select {2} from {3} where {4} {5}
                //        END

                //", model.ReportName.Replace(" ", "_"), filters, fields, model.SelectedEntity, string.IsNullOrEmpty(conditions) ? "1 = 1" : conditions, ((model.GroupBys ?? new List<GroupBy>()).Any() || (model.GroupByFilters ?? new List<GroupByFilter>()).Any()) ? groupByClause : "");

                string sql1 = string.Format(@"
                    
                    Create PROCEDURE [dbo].[report_{0}]
                    {6}
                    AS
                    BEGIN                    
                        select distinct {1} {2} from {3} where {4} {5}
                    END

            ", model.ReportName.Replace(" ", "_"), FY, fields, model.SelectedEntity, string.IsNullOrEmpty(conditions) ? "1 = 1" : conditions,
            //////////////////////////////////////////(model.GroupBySelectedFields != null ? groupByClause : ""), filters);
            (((model.GroupBys != null && model.GroupBys.Count > 0) || model.GroupBySelectedFields != null) ? groupByClause : ""), filters);


                using (AMPEntities entity = new AMPEntities())
                {
                    entity.Database.ExecuteSqlCommand(sql1);
                }
            }
            catch (Exception ex)
            {
                model.Errors = new List<string>();
                model.Errors.Add(ex.Message);
            }
        }

        public static Dictionary<string, string> stringMapper()
        {
            using (AMPEntities entity = new AMPEntities())
            {
                string sql = "select [key],value from TBL_String_Mapper";
                Dictionary<string, string> stringMapper = new Dictionary<string, string>();
                stringMapper = entity.Database.SqlQuery<DictionaryResult>(sql).ToDictionary(r => r.Key, r => r.Value);
                return stringMapper;
            }
        }

        public static bool deleteStoredProc(string proc)
        {
            try
            {
                string sql1 = string.Format(@"DROP PROCEDURE [dbo].[{0}] ", proc);


                using (AMPEntities entity = new AMPEntities())
                {
                    entity.Database.ExecuteSqlCommand(sql1);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}