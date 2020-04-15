using AMP.Finacle.Models;
using AMP.Models;
using AMP.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AMP.Finacle
{
    public class FinacleConnector
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["FinaleEntites"].ToString();
            }
        }

        public bool LocDetails(out string exception)
        {
            exception = "";
            List<LocDetailsModel> records = new List<LocDetailsModel>();

            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select Limit_B2KID,ACCT_NAME,FREE_TEXT,LIMIT_PREFIX,LIMIT_SUFFIX, SANCTION_LIMIT, CIF from AMP_LOC_DET";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            records.Add(new LocDetailsModel()
                            {
                                LimitB2KID = reader.GetValue(0).ToString(),
                                AccountName = reader.GetValue(1).ToString(),
                                FreeText = reader.GetValue(2).ToString(),
                                LimitPrefix = reader.GetValue(3).ToString(),
                                LimitSuffix = reader.GetValue(4).ToString(),
                                SanctionAmount = reader.GetDecimal(5),
                                CIF = reader.GetValue(6).ToString()
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "LocDetails.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from loc in records
                                    join l in model.Finacle_LocDetails on loc.LimitPrefix equals l.LimitPrefix
                                    where (loc.AccountName != l.AccountName) || (loc.FreeText != loc.FreeText) || (loc.LimitB2KID != l.LimitB2KID) || (loc.LimitPrefix != l.LimitPrefix) || (loc.LimitSuffix != l.LimitSuffix) || (loc.SanctionAmount != l.SanctionAmount) || (loc.CIF != l.CIF)
                                    select new LocDetailsModel()
                                    {
                                        AccountName = loc.AccountName,
                                        SanctionAmount = loc.SanctionAmount,
                                        FreeText = loc.FreeText,
                                        LimitB2KID = loc.LimitB2KID,
                                        LimitSuffix = loc.LimitSuffix,
                                        CIF = loc.CIF,
                                        Id = l.Id
                                    }
                     ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_LocDetails.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.AccountName = record.AccountName;
                            data.FreeText = record.FreeText;
                            data.LimitB2KID = record.LimitB2KID;
                            data.LimitPrefix = record.LimitPrefix;
                            data.LimitSuffix = record.LimitSuffix;
                            data.SanctionAmount = record.SanctionAmount;
                            data.CIF = record.CIF;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while paring for update. Error: {0}", ex.Message),
                        ServiceName = "LocDetails.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    #region To Insert
                    var ToInsert = (from loc in records
                                    join l in model.Finacle_LocDetails on loc.LimitPrefix equals l.LimitPrefix into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_LocDetails()
                                    {
                                        AccountName = loc.AccountName,
                                        SanctionAmount = loc.SanctionAmount,
                                        FreeText = loc.FreeText,
                                        LimitB2KID = loc.LimitB2KID,
                                        LimitPrefix = loc.LimitPrefix,
                                        LimitSuffix = loc.LimitSuffix,
                                        CIF = loc.CIF,
                                    }
                                    ).ToList();
                    model.Finacle_LocDetails.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "LocDetails.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "LocDetails.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool RepaymentSchdule(out string exception)
        {
            List<RepaymentSchedule> records = new List<RepaymentSchedule>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select FORACID,LimitPrefix,ACCT_NAME,FLOW_START_DATE,FLOW_ID,FLOW_AMT, ACCT_CRNCY_CODE from amp_repay_schd";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            records.Add(new RepaymentSchedule()
                            {
                                FORACID = reader.GetValue(0).ToString(),
                                LimitPrefix = reader.GetValue(1).ToString(),
                                AccountName = reader.GetValue(2).ToString(),
                                FlowStart = reader.GetDateTime(3),
                                FlowId = reader.GetValue(4).ToString(),
                                FlowAmount = reader.GetDecimal(5),
                                Currency = reader.GetValue(6).ToString()
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "RepaymentSchdule.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from repayment in records.AsEnumerable()
                                    join r in model.Finacle_RepaymentSchedule
                                    on new { repayment.FORACID, repayment.FlowId, repayment.LimitPrefix, repayment.Currency, FlowStart = repayment.FlowStart.ToString("MM/dd/yyyy") } equals new { r.FORACID, r.FlowId, r.LimitPrefix, r.Currency, FlowStart = r.FlowStart.HasValue ? r.FlowStart.Value.ToString("MM/dd/yyyy") : "" }
                                    where repayment.FlowAmount != r.FlowAmount
                                    select new RepaymentSchedule()
                                    {
                                        Id = r.Id,
                                        FlowAmount = repayment.FlowAmount

                                    }
                                 ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_RepaymentSchedule.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.FlowAmount = record.FlowAmount;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while paring for update. Error: {0}", ex.Message),
                        ServiceName = "RepaymentSchdule.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    #region To Insert
                    var ToInsert = (from repayment in records
                                    join r in model.Finacle_RepaymentSchedule
                                    on new { repayment.FORACID, repayment.FlowId, repayment.LimitPrefix, repayment.Currency, FlowStart = repayment.FlowStart.ToString("MM/dd/yyyy") } equals new { r.FORACID, r.FlowId, r.LimitPrefix, r.Currency, FlowStart = r.FlowStart.HasValue ? r.FlowStart.Value.ToString("MM/dd/yyyy") : "" } into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_RepaymentSchedule()
                                    {
                                        AccountName = repayment.AccountName,
                                        Currency = repayment.Currency,
                                        FlowAmount = repayment.FlowAmount,
                                        FORACID = repayment.FORACID,
                                        FlowId = repayment.FlowId,
                                        FlowStart = repayment.FlowStart,
                                        LimitPrefix = repayment.LimitPrefix
                                    }
                                    ).ToList();
                    model.Finacle_RepaymentSchedule.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "RepaymentSchdule.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "RepaymentSchdule.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool Disbursment(out string exception)
        {
            List<DisbersmentModel> records = new List<DisbersmentModel>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select acid,LimitPrefix,limit_b2kid,disb_srl_num,foracid,acct_name,sanct_lim,disb_amount,crncy_code,disbdate from amp_disb_view";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            DateTime? disDate = null;
                            if (!reader.IsDBNull(9))
                                disDate = reader.GetDateTime(9);

                            records.Add(new DisbersmentModel()
                            {
                                ACID = reader.GetValue(0).ToString(),
                                LimitPrefix = reader.GetValue(1).ToString(),
                                LimitB2KID = reader.GetValue(2).ToString(),
                                DisbSerialNo = reader.GetValue(3).ToString(),
                                FORACID = reader.GetValue(4).ToString(),
                                AccountName = reader.GetValue(5).ToString(),
                                SanctionLimit = reader.GetDecimal(6),
                                DisbAmount = reader.GetDecimal(7),
                                CurrencyCode = reader.GetValue(8).ToString(),
                                DisDate = disDate
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "Disbursment.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from dis in records
                                    join d in model.Finacle_Disbursement on new { dis.ACID, DisbSerialNo = dis.DisbSerialNo } equals new { d.ACID, DisbSerialNo = d.DisbSerialNo }
                                    where (dis.DisbAmount != d.DisbAmount) || (dis.DisDate != d.DisDate) || (dis.SanctionLimit != d.SanctionLimit) || (dis.LimitB2KID != d.LimitB2KID)
                                    select new DisbersmentModel()
                                    {
                                        Id = d.Id,
                                        DisbAmount = dis.DisbAmount,
                                        DisDate = dis.DisDate,
                                        SanctionLimit = dis.SanctionLimit,
                                        LimitB2KID = dis.LimitB2KID
                                    }
                     ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_Disbursement.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.DisbAmount = record.DisbAmount;
                            data.DisDate = record.DisDate;
                            data.SanctionLimit = record.SanctionLimit;
                            data.LimitB2KID = record.LimitB2KID;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while paring for update. Error: {0}", ex.Message),
                        ServiceName = "Disbursment.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    #region To Insert
                    var ToInsert = (from dis in records
                                    join d in model.Finacle_Disbursement on new { dis.ACID, DisbSerialNo = dis.DisbSerialNo } equals new { d.ACID, DisbSerialNo = d.DisbSerialNo } into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_Disbursement()
                                    {
                                        AccountName = dis.AccountName,
                                        ACID = dis.ACID,
                                        CurrencyCode = dis.CurrencyCode,
                                        DisbAmount = dis.DisbAmount,
                                        FORACID = dis.FORACID,
                                        DisbSerialNo = dis.DisbSerialNo,
                                        DisDate = dis.DisDate,
                                        LimitB2KID = dis.LimitB2KID,
                                        LimitPrefix = dis.LimitPrefix,
                                        SanctionLimit = dis.SanctionLimit
                                    }
                                    ).ToList();
                    model.Finacle_Disbursement.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "Disbursment.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "Disbursment.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool AmpDemands(out string exception)
        {
            List<AMPDemandModel> records = new List<AMPDemandModel>();
            exception = "";
            #region Date
            string query = "";
            using (AMPEntities model = new AMPEntities())
            {
                var date = model.Finacle_AMDemands.ToList().Max(e => e.CreatedOn).GetValueOrDefault();
                if (date != null)
                {
                    query = string.Format("select foracid,acct_name,dmd_flow_id,dmd_amt,last_adj_date,tot_adj_amt,dmd_eff_date,rcre_time from AMP_DEMANDS where rcre_time > TO_DATE('{0}', 'DD/MM/YYYY' ) ", date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
                else
                {
                    query = "select foracid,acct_name,dmd_flow_id,dmd_amt,last_adj_date,tot_adj_amt,dmd_eff_date,rcre_time from AMP_DEMANDS";
                }
            }
            #endregion

            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = query;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            DateTime? adjDate = null;
                            if (!reader.IsDBNull(4))
                                adjDate = reader.GetDateTime(4);

                            records.Add(new AMPDemandModel()
                            {
                                FORACID = reader.GetValue(0).ToString(),
                                AccountName = reader.GetValue(1).ToString(),
                                DemandFlowId = reader.GetValue(2).ToString(),
                                DemandAmount = reader.GetDecimal(3),
                                LastAdjustmentDate = adjDate,
                                TotalAdjustmentAmount = reader.GetDecimal(5),
                                DemandEffectiveDate = reader.GetDateTime(6),
                                CreationDate = reader.GetDateTime(7)
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "AmpDemands.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Insert
                    var ToInsert = records.Select(x =>
                    {
                        return new Finacle_AMDemands()
                        {
                            FORACID = x.FORACID,
                            AccountName = x.AccountName,
                            DemandFlowId = x.DemandFlowId,
                            DemandAmount = x.DemandAmount,
                            LastAdjustmentDate = x.LastAdjustmentDate,
                            TotalAdjustmentAmount = x.TotalAdjustmentAmount,
                            DemandEffectiveDate = x.DemandEffectiveDate,
                            CreatedOn = x.CreationDate

                        };
                    });

                    model.Finacle_AMDemands.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "AmpDemands.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "AmpDemands.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool CgsView(out string exception)
        {
            List<CGSModel> records = new List<CGSModel>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select Foracid,acct_name,acct_opn_date,clr_bal_amt,acct_mgr_user_id,CIF,SCHEMECODE,ACCT_CRNCY_CODE from cgs_view";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            records.Add(new CGSModel()
                            {
                                FORACID = reader.GetValue(0).ToString(),
                                AccountName = reader.GetValue(1).ToString(),
                                AccountOpenDate = reader.GetDateTime(2),
                                ClearBalanceAmount = reader.GetDecimal(3),
                                AccountMgr = reader.GetValue(4).ToString(),
                                CIF = reader.GetValue(5).ToString(),
                                SchemeCode = reader.GetValue(6).ToString(),
                                Currency = reader.GetValue(7).ToString()
                            });
                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "CgsView.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from cgs in records
                                    join c in model.Finacle_CGS on cgs.FORACID equals c.FORACID
                                    where (cgs.AccountMgr != c.AccountMgr) || (cgs.AccountName != c.AccountName) || (cgs.AccountOpenDate != c.AccountOpenDate) || (cgs.ClearBalanceAmount != c.ClearBalanceAmount) || (cgs.CIF != c.CIF) || (cgs.SchemeCode != c.SchemeCode) || (cgs.Currency != c.Currency)
                                    select new CGSModel()
                                    {
                                        AccountMgr = cgs.AccountMgr,
                                        AccountName = cgs.AccountName,
                                        AccountOpenDate = cgs.AccountOpenDate,
                                        ClearBalanceAmount = cgs.ClearBalanceAmount,
                                        Id = c.Id,
                                        CIF = cgs.CIF,
                                        SchemeCode = cgs.SchemeCode,
                                        Currency = cgs.Currency
                                    }
                     ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_CGS.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.AccountMgr = record.AccountMgr;
                            data.AccountName = record.AccountName;
                            data.AccountOpenDate = record.AccountOpenDate;
                            data.ClearBalanceAmount = record.ClearBalanceAmount;
                            data.CIF = record.CIF;
                            data.SchemeCode = record.SchemeCode;
                            data.Currency = record.Currency;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while paring for update. Error: {0}", ex.Message),
                        ServiceName = "CgsView.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    #region To Insert
                    var ToInsert = (from cgs in records
                                    join c in model.Finacle_CGS on cgs.FORACID equals c.FORACID into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_CGS()
                                    {
                                        AccountMgr = cgs.AccountMgr,
                                        AccountName = cgs.AccountName,
                                        AccountOpenDate = cgs.AccountOpenDate,
                                        ClearBalanceAmount = cgs.ClearBalanceAmount,
                                        FORACID = cgs.FORACID,
                                        CIF = cgs.CIF,
                                        SchemeCode = cgs.SchemeCode,
                                        Currency = cgs.CIF,
                                        Id = cgs.Id
                                    }
                                    ).ToList();
                    model.Finacle_CGS.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "CgsView.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "CgsView.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool AmpContracts(out string exception)
        {
            List<ContractModel> records = new List<ContractModel>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select CIF_ID,\"Contract ID\",\"Contractor Name\",\"Contract value\",\"Contract Approval Date\",\"PREFIX\",\"SUFFIX\", CGS_ID from amp_contract";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            records.Add(new ContractModel()
                            {
                                CIF = reader.GetValue(0).ToString(),
                                ContractId = reader.GetValue(1).ToString(),
                                ContractorName = reader.GetValue(2).ToString(),
                                ContractValue = reader.GetDecimal(3),
                                ContractDate = reader.GetDateTime(4),
                                Prefix = reader.GetValue(5).ToString(),
                                Suffix = reader.GetValue(6).ToString(),
                                CGS_ID = reader.GetValue(7).ToString(),
                            });
                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "AmpContracts.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from con in records
                                    join c in model.Finacle_Contracts on new { con.CIF, con.ContractId } equals new { c.CIF, c.ContractId }
                                    where (con.ContractDate != c.ContractDate) || (con.ContractorName != c.ContractorName) || (con.ContractValue != c.ContractValue) || (con.Prefix != c.Prefix) || (con.Suffix != c.Suffix) || (con.CGS_ID != c.CGS_ID)
                                    select new ContractModel()
                                    {
                                        Id = c.Id,
                                        CIF = con.CIF,
                                        ContractDate = con.ContractDate,
                                        ContractId = con.ContractId,
                                        ContractValue = con.ContractValue,
                                        ContractorName = con.ContractorName,
                                        Prefix = con.Prefix,
                                        Suffix = con.Suffix,
                                        CGS_ID = con.CGS_ID
                                    }
                     ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_Contracts.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.ContractDate = record.ContractDate;
                            data.ContractorName = record.ContractorName;
                            data.ContractValue = record.ContractValue;
                            data.Prefix = record.Prefix;
                            data.Suffix = record.Suffix;
                            data.CGS_ID = record.CGS_ID;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while paring for update. Error: {0}", ex.Message),
                        ServiceName = "AmpContracts.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    #region To Insert
                    var ToInsert = (
                                    from con in records
                                    join c in model.Finacle_Contracts on new { con.CIF, con.ContractId } equals new { c.CIF, c.ContractId } into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_Contracts()
                                    {
                                        CIF = con.CIF,
                                        ContractDate = con.ContractDate,
                                        ContractValue = con.ContractValue,
                                        ContractId = con.ContractId,
                                        ContractorName = con.ContractorName,
                                        Prefix = con.Prefix,
                                        Suffix = con.Suffix,
                                        CGS_ID = con.CGS_ID
                                    }
                                    ).ToList();
                    model.Finacle_Contracts.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "AmpContracts.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "AmpContracts.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool PrincipalDue(out string exception)
        {
            List<PrincipalDueModel> records = new List<PrincipalDueModel>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select FORACID,FLOW_ID,DUE_DATE,LR_FREQ_TYPE,DUE_AMOUNT,RUN_DATE from amp_loc_principal_due";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            records.Add(new PrincipalDueModel()
                            {
                                AccountId = reader.GetValue(0).ToString(),
                                DemandType = reader.GetValue(1).ToString(),
                                DueDate = reader.GetDateTime(2),
                                Frequency = reader.GetValue(3).ToString(),
                                DueAmount = reader.GetDecimal(4),
                                RunDate = reader.GetDateTime(5)
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "PrincipalDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

           

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    //model.Database.ExecuteSqlCommand("TRUNCATE TABLE [Finacle_PrincipalDue]");

                    #region To Update
                    var ToUpdate = (from principaldue in records.AsEnumerable()
                                    join r in model.Finacle_PrincipalDue
                                    on new { principaldue.AccountId, principaldue.DemandType, principaldue.Frequency, DueDate = principaldue.DueDate.ToString("MM/dd/yyyy") } equals new { r.AccountId, r.DemandType, r.Frequency, DueDate = r.DueDate.HasValue ? r.DueDate.Value.ToString("MM/dd/yyyy") : "" }
                                    where principaldue.DueDate != r.DueDate
                                    select new PrincipalDueModel()
                                    {
                                        Id = r.Id,
                                        DueDate = principaldue.DueDate
                                    }
                                 ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_PrincipalDue.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.DueAmount = record.DueAmount;
                            data.RunDate = record.RunDate;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for update. Error: {0}", ex.Message),
                        ServiceName = "PrincipalDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }
                

                try
                {
                    #region To Insert
                    var ToInsert = (from principaldue in records
                                    join r in model.Finacle_PrincipalDue
                                    on new { principaldue.AccountId, principaldue.DemandType, principaldue.Frequency, DueDate = principaldue.DueDate.ToString("MM/dd/yyyy") } equals new { r.AccountId, r.DemandType, r.Frequency, DueDate = r.DueDate.HasValue ? r.DueDate.Value.ToString("MM/dd/yyyy") : "" } into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_PrincipalDue()
                                    {
                                        AccountId = principaldue.AccountId,
                                        DemandType = principaldue.DemandType,
                                        DueDate = principaldue.DueDate,
                                        Frequency = principaldue.Frequency,
                                        DueAmount = principaldue.DueAmount,
                                        RunDate = principaldue.RunDate
                                    }
                                    ).ToList();
                    model.Finacle_PrincipalDue.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "PrincipalDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "PrincipalDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool InterestDue(out string exception)
        {
            List<InterestDueModel> records = new List<InterestDueModel>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select FORACID,LAST_RUN_DATE,NEXT_RUN_DATE from amp_loc_interest_due";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            DateTime? ldate = null, ndate = null;

                            if (!reader.IsDBNull(1))
                                ldate = reader.GetDateTime(1);
                            if (!reader.IsDBNull(2))
                                ndate = reader.GetDateTime(2);

                            records.Add(new InterestDueModel()
                            {
                                AccountId = reader.GetValue(0).ToString(),
                                LastDate = ldate,
                                NextDate = ndate
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "InterestDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from interestdue in records.AsEnumerable()
                                    join r in model.Finacle_InterestDue
                                    on new { interestdue.AccountId } equals new { r.AccountId }
                                    where interestdue.LastDate != r.LastDate || interestdue.NextDate != r.NextDate
                                    select new InterestDueModel()
                                    {
                                        Id = r.Id,
                                        LastDate = interestdue.LastDate,
                                        NextDate = interestdue.NextDate
                                    }
                                 ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_InterestDue.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.LastDate = record.LastDate;
                            data.NextDate = record.NextDate;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for update. Error: {0}", ex.Message),
                        ServiceName = "InterestDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    #region To Insert
                    var ToInsert = (from interestdue in records
                                    join r in model.Finacle_InterestDue
                                    on new { interestdue.AccountId } equals new { r.AccountId } into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_InterestDue()
                                    {
                                        AccountId = interestdue.AccountId,
                                        LastDate = interestdue.LastDate,
                                        NextDate = interestdue.NextDate
                                    }
                                    ).ToList();
                    model.Finacle_InterestDue.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "InterestDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "PrincipalDue.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool ContractTransactions(out string exception)
        {
            List<ContractTransaction> records = new List<ContractTransaction>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select FORACID, CIF_ID, TRAN_DATE, TRAN_AMT, TRAN_PARTICULAR, TRAN_CRNCY_CODE, SANCT_LIM, CUM_DR_AMT, CUM_CR_AMT, TRAN_ID from amp_bcd_transactions";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            records.Add(new ContractTransaction()
                            {
                                AccountId = reader.GetValue(0).ToString(),
                                CIF = reader.GetValue(1).ToString(),
                                TranDate = reader.GetDateTime(2),
                                TranAmount = reader.GetDecimal(3),
                                Particulars = reader.GetValue(4).ToString(),
                                Currency = reader.GetValue(5).ToString(),
                                SanctionedAmount = reader.GetDecimal(6),
                                CummulativeDebit = reader.GetDecimal(7),
                                CummulativeCredit = reader.GetDecimal(8),
                                TranId = reader.GetValue(9).ToString()
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "ContractTransactions.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from cont in records
                                    join d in model.Finacle_Contract_Transanctions on new { cont.AccountId, cont.TranDate, cont.TranId } equals new { d.AccountId, d.TranDate, d.TranId }
                                    where (cont.Particulars != d.Particulars) || (cont.CummulativeCredit != d.CummulativeCredit) || (cont.CummulativeDebit != d.CummulativeDebit)
                                    select new ContractTransaction()
                                    {
                                        Id = d.Id,
                                        Particulars = cont.Particulars,
                                        CummulativeDebit = cont.CummulativeDebit,
                                        CummulativeCredit = cont.CummulativeCredit
                                    }
                     ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_Contract_Transanctions.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.Particulars = record.Particulars;
                            data.CummulativeDebit = record.CummulativeDebit;
                            data.CummulativeCredit = record.CummulativeCredit;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for update. Error: {0}", ex.Message),
                        ServiceName = "ContractTransactions.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    #region To Insert
                    var ToInsert = (from cont in records
                                    join d in model.Finacle_Contract_Transanctions on new { cont.AccountId, cont.TranDate, cont.TranId } equals new { d.AccountId, d.TranDate, d.TranId } into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_Contract_Transanctions()
                                    {
                                        AccountId = cont.AccountId,
                                        CIF = cont.CIF,
                                        TranDate = cont.TranDate,
                                        TranAmount = cont.TranAmount,
                                        Particulars = cont.Particulars,
                                        Currency = cont.Currency,
                                        SanctionedAmount = cont.SanctionedAmount,
                                        CummulativeDebit = cont.CummulativeDebit,
                                        CummulativeCredit = cont.CummulativeCredit,
                                        TranId = cont.TranId
                                    }
                                    ).ToList();
                    model.Finacle_Contract_Transanctions.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "ContractTransactions.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "ContractTransactions.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool LocTransactions(out string exception)
        {
            List<LocTransactions> records = new List<LocTransactions>();
            exception = "";
            #region Date
            string query = "";
            using (AMPEntities model = new AMPEntities())
            {
                var date = model.Finacle_LocTransactions.ToList().Max(e => e.CreatedOn).GetValueOrDefault(); ;
                if (date != null)
                {
                    query = string.Format("select Foracid,CIF_Id,DMD_FLOW_ID,DEL_FLG,DMD_AMT,LAST_ADJ_DATE,TOT_ADJ_AMT,RCRE_TIME,LDT_CRNCY_CODE,Prefix from amp_loc_transactions where rcre_time > TO_DATE('{0}','DD/MM/YYYY') ", date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
                else
                {
                    query = "select Foracid,CIF_Id,DMD_FLOW_ID,DEL_FLG,DMD_AMT,LAST_ADJ_DATE,TOT_ADJ_AMT,RCRE_TIME,LDT_CRNCY_CODE,Prefix from amp_loc_transactions";
                }
            }
            #endregion

            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = query;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            DateTime? adjDate = null;
                            if (!reader.IsDBNull(5))
                                adjDate = reader.GetDateTime(5);

                            records.Add(new LocTransactions()
                            {
                                FORACID = reader.GetValue(0).ToString(),
                                CIFID = reader.GetValue(1).ToString(),
                                DemandFlow = reader.GetValue(2).ToString(),
                                DeleteFlag = reader.GetValue(3).ToString(),
                                DemandAmount = reader.GetDecimal(4),
                                LastAdjustmentDate = adjDate,
                                TotalAdjustedAmount = reader.GetDecimal(6),
                                CreatedOn = reader.GetDateTime(7),
                                CurrencyCode = reader.GetValue(8).ToString(),
                                Prefix = reader.GetValue(9).ToString()
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "LocTransactions.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Insert
                    var ToInsert = records.Select(x =>
                    {
                        return new Finacle_LocTransactions()
                        {
                            CIFID = x.CIFID,
                            DemandAmount = x.DemandAmount,
                            CreatedOn = x.CreatedOn,
                            FORACID = x.FORACID,
                            LastAdjustmentDate = x.LastAdjustmentDate,
                            TotalAdjustedAmt = x.TotalAdjustedAmount,
                            CurrencyCode = x.CurrencyCode,
                            DeleteFlag = x.DeleteFlag,
                            DemandFlow = x.DemandFlow,
                            Prefix = x.Prefix
                        };
                    });

                    model.Finacle_LocTransactions.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "LocTransactions.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "LocTransactions.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }

        public bool LOCFinancials(out string exception)
        {
            List<LOCFinancials> records = new List<LOCFinancials>();
            exception = "";
            #region Fetch data
            string constr = ConnectionString;
            using (OracleConnection conn = new OracleConnection(constr))
            {
                conn.ConnectionString = constr;
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = (OracleConnection)conn;

                    cmd.CommandText = "select LIMITPREFIX, FORACID, ACCT_NAME, LOAN_OUTSTANDING, TOTAL_DISB, PRINCIPAL_DEMAND, PRINCIPAL_COLLECTION, PRINCIPAL_OVERDUE, INTEREST_DEMAND, INTEREST_COLLECTION, INTEREST_OVERDUE from DISB_REPAY_OS_view";

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {

                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            records.Add(new LOCFinancials()
                            {
                                LimitPrefix = reader.GetValue(0).ToString(),
                                FORACID = reader.GetValue(1).ToString(),
                                AccountName = reader.GetValue(2).ToString(),
                                LoanOutstanding = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                                TotalDisbursed = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                                PrincipalDemand = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                                PrincipalCollection = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                                PrincipalOverdue = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                                InterestDemand = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                                InterestCollection = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                                InterestOverdue = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10)
                            });

                        }
                        reader.Dispose();
                    }
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while fetching data. Error: {0}", ex.Message),
                        ServiceName = "LOCFinancials.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion

            #region Insert Or Update
            using (AMPEntities model = new AMPEntities())
            {
                try
                {
                    #region To Update
                    var ToUpdate = (from accountfin in records
                                    join d in model.Finacle_LocFinancials on new { accountfin.FORACID } equals new { d.FORACID }
                                    //where (cont.Particulars != d.Particulars) || (cont.CummulativeCredit != d.CummulativeCredit) || (cont.CummulativeDebit != d.CummulativeDebit)
                                    select new LOCFinancials()
                                    {
                                        Id = d.Id,
                                        LimitPrefix = d.LimitPrefix,
                                        AccountName = d.AccountName,
                                        LoanOutstanding = d.LoanOutstanding.GetValueOrDefault(0m),
                                        TotalDisbursed = d.TotalDisbursed.GetValueOrDefault(0m),
                                        PrincipalDemand = d.PrincipalDemand.GetValueOrDefault(0m),
                                        PrincipalCollection = d.PrincipalCollection.GetValueOrDefault(0m),
                                        PrincipalOverdue = d.PrincipalOverdue.GetValueOrDefault(0m),
                                        InterestDemand = d.InterestDemand.GetValueOrDefault(0m),
                                        InterestCollection = d.InterestCollection.GetValueOrDefault(0m),
                                        InterestOverdue = d.InterestOverdue.GetValueOrDefault(0m),
                                    }
                     ).ToList();
                    foreach (var record in ToUpdate)
                    {
                        var data = model.Finacle_LocFinancials.FirstOrDefault(e => e.Id == record.Id);
                        if (data != null)
                        {
                            data.LimitPrefix = record.LimitPrefix;
                            data.AccountName = record.AccountName;
                            data.LoanOutstanding = record.LoanOutstanding;
                            data.TotalDisbursed = record.TotalDisbursed;
                            data.PrincipalDemand = record.PrincipalDemand;
                            data.PrincipalCollection = record.PrincipalCollection;
                            data.PrincipalOverdue = record.PrincipalOverdue;
                            data.InterestDemand = record.InterestDemand;
                            data.InterestCollection = record.InterestCollection;
                            data.InterestOverdue = record.InterestOverdue;
                            data.SyncDate = DateTime.Now;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for update. Error: {0}", ex.Message),
                        ServiceName = "LOCFinancials.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                {
                    FullMessage = "testlog",
                    Message = string.Format("Record Count: {0}", records.Count.ToString()),
                    ServiceName = "LOCFinancials.FinacleConnector",
                });

                try
                {
                    #region To Insert
                    var ToInsert = (from accountfin in records
                                    join d in model.Finacle_LocFinancials on new { accountfin.FORACID } equals new { d.FORACID } into recs
                                    from rt in recs.DefaultIfEmpty()
                                    where rt == null
                                    select new Finacle_LocFinancials()
                                    {
                                        LimitPrefix = accountfin.LimitPrefix,
                                        FORACID = accountfin.FORACID,
                                        AccountName = accountfin.AccountName,
                                        LoanOutstanding = accountfin.LoanOutstanding,
                                        TotalDisbursed = accountfin.TotalDisbursed,
                                        PrincipalDemand = accountfin.PrincipalDemand,
                                        PrincipalCollection = accountfin.PrincipalCollection,
                                        PrincipalOverdue = accountfin.PrincipalOverdue,
                                        InterestDemand = accountfin.InterestDemand,
                                        InterestCollection = accountfin.InterestCollection,
                                        InterestOverdue = accountfin.InterestOverdue,
                                        SyncDate = DateTime.Now
                                    }
                                    ).ToList();
                    model.Finacle_LocFinancials.AddRange(ToInsert);
                    #endregion
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while parsing for insertion. Error: {0}", ex.Message),
                        ServiceName = "LOCFinancials.FinacleConnector",
                    });
                    exception += ex.Message;
                }

                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                    {
                        FullMessage = ex.ToString(),
                        Message = string.Format("Error occured while saving context. Error: {0}", ex.Message),
                        ServiceName = "LOCFinancials.FinacleConnector",
                    });
                    exception += ex.Message;
                }
            }
            #endregion
            return exception == "" ? true : false;
        }
    }
}