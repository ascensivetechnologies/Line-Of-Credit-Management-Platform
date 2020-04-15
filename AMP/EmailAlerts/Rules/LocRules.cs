using AMP.Models;
using AMP.Services;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AMP.EmailAlerts.Rules
{
    public class LocRules
    {
        public static List<int> BalanceConfirmation(int offset)
        {   
            List<int> result;
            using (AMPEntities ampEntities = new AMPEntities())
            {
                if (DateTime.Now.Month <= 4)
                {
                    DateTime offsetDate = new DateTime((DateTime.Now.Year) - 1, 5, 1);

                    string balanceCnfDate = new DateTime(offsetDate.Year, 3, 31).ToString("dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);

                    result = ampEntities.TBL_LOC.AsNoTracking().Where(m => m.TBL_LocBalance.Count() == 0
                    && DbFunctions.TruncateTime(DateTime.Now) >= offsetDate).Select(m => m.Id).ToList();
                }
                else
                {
                    DateTime offsetDate = new DateTime((DateTime.Now.Year), 5, 1);

                    string balanceCnfDate = new DateTime(offsetDate.Year, 3, 31).ToString("dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);

                    result = ampEntities.TBL_LOC.AsNoTracking().Where(m => m.TBL_LocBalance.Count() == 0
                    && DbFunctions.TruncateTime(DateTime.Now) >= offsetDate).Select(m => m.Id).ToList();
                }

            }
            return result;
        }

        public static LOCModel Verify_BalanceConfirmation(int locId)
        {
            LOCModel model;
            DateTime offsetDate;
            string balanceCnfDate;
            try
            {
                using (AMPEntities ampEntities = new AMPEntities())
                {
                    if (DateTime.Now.Month <= 4)
                    {
                        offsetDate = new DateTime((DateTime.Now.Year) - 1, 5, 1);

                        balanceCnfDate = new DateTime(offsetDate.Year, 3, 31).ToString("dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        offsetDate = new DateTime((DateTime.Now.Year), 5, 1);

                        balanceCnfDate = new DateTime(offsetDate.Year, 3, 31).ToString("dd/MM/yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);
                    }
                    model = ampEntities.TBL_LOC.AsNoTracking().Where(m => m.TBL_LocBalance.Count() == 0
                        && m.Id == locId).Select(m => new LOCModel
                        {
                            Id = m.Id,
                            BalanceConfirmationDate = balanceCnfDate
                        }).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                new Dashboard2ServiceLayer().InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "LocRules.Verify_BalanceConfirmation"
                });
                model = null;
            }
            return model;
        }

        public static LOCModel Schedule_BalanceConfirmation(int locId, string notes)
        {
            LOCModel model;
            DateTime offsetDate;
            string balanceCnfDate;
            try
            {
                using (AMPEntities ampEntities = new AMPEntities())
                {
                    if (DateTime.Now.Month <= 4)
                    {
                        offsetDate = new DateTime((DateTime.Now.Year) - 1, 5, 1);

                        balanceCnfDate = new DateTime(offsetDate.Year, 3, 31).ToString("dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        offsetDate = new DateTime((DateTime.Now.Year), 5, 1);

                        balanceCnfDate = new DateTime(offsetDate.Year, 3, 31).ToString("dd/MM/yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);
                    }
                    model = ampEntities.TBL_LOC.AsNoTracking().Where(m => m.TBL_LocBalance.Count() == 0
                        && m.Id == locId).Select(m => new LOCModel
                        {
                            Id = m.Id,
                            BalanceConfirmationDate = balanceCnfDate
                        }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                new Dashboard2ServiceLayer().InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "LocRules.Schedule_BalanceConfirmation"
                });
                model = null;
            }
            return model;
        }

        public static List<int> AgreementSigningExp(int offset)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                var result = ampEntities.TBL_LOC.AsNoTracking().Where(e =>
                         e.SigningDate == null && e.TerminalDate.HasValue &&
                          DbFunctions.DiffDays(DateTime.Now, e.TerminalDate) == offset)
                          .Select(e => e.Id).ToList();

                return result;
            }
        }

        public static LOCModel Verify_AgreementSigningExp(int LocId)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                try
                {
                    var result = ampEntities.TBL_LOC.Where(e => e.TerminalDate.HasValue && e.SigningDate == null &&
                         e.Id == LocId && DbFunctions.DiffDays(DateTime.Now, e.TerminalDate) >= 0).ToList()
                         .Select(e => new LOCModel()
                         {
                             Id = e.Id,
                             LOCName = e.Name,
                             LOCNumber = e.LocNumber,
                             TerminalDate = e.TerminalDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

                         }).FirstOrDefault();

                    return result;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Verify_AgreementSigningExp"
                    });
                    return null;
                }
            }
            
        }
        public static LOCModel Schedule_AgreementSigningExp(int LocId, string notes)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                try
                {
                    var result = ampEntities.TBL_LOC.Where(e => 
                         e.Id == LocId ).ToList()
                         .Select(e => new LOCModel()
                         {
                             Id = e.Id,
                             LOCName = e.Name,
                             LOCNumber = e.LocNumber,
                             TerminalDate = e.TerminalDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

                         }).FirstOrDefault();

                    return result;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Schedule_AgreementSigningExp"
                    });
                    return null;
                }
            }

        }
        public static List<int> ContractsExpiry(int offset)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                var result = ampEntities.TBL_LOC.Where(m => m.TBL_LOC_Contract.Count == 0
                && m.SigningDate.HasValue
                && DbFunctions.DiffDays(DateTime.Now, DbFunctions.AddMonths(m.SigningDate, 18)) == offset
                ).Select(m => m.Id).ToList();
                return result;
            }
        }

        public static LOCModel Verify_ContractsExpiry(int LocId)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                try
                {
                    var result = ampEntities.TBL_LOC.Where(m => m.TBL_LOC_Contract.Count == 0
                && m.SigningDate.HasValue && m.Id == LocId
                && DbFunctions.DiffDays(DateTime.Now, DbFunctions.AddMonths(m.SigningDate, 18)) >= 0
                ).ToList()
                .Select(e => new LOCModel
                {
                    Id = e.Id,
                    LOCName = e.Name,
                    LOCNumber = e.LocNumber,
                    TerminalDate = e.SigningDate.Value.AddMonths(18).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                }).FirstOrDefault();
                    return result;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Verify_ContractsExpiry"
                    });
                    return null;
                }
            }
        }

        public static LOCModel Schedule_ContractsExpiry(int LocId, string notes)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                try
                {
                    var result = ampEntities.TBL_LOC.Where(m =>  m.Id == LocId).ToList()
                .Select(e => new LOCModel
                {
                    Id = e.Id,
                    LOCName = e.Name,
                    LOCNumber = e.LocNumber,
                    TerminalDate = e.SigningDate.Value.AddMonths(18).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                }).FirstOrDefault();
                    return result;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Schedule_ContractsExpiry"
                    });
                    return null;
                }
            }
        }

        public static List<int> InterestDemand(int offset)
        {
            using(var ctx = new AMPEntities())
            {
                DateTime triggerDate = DateTime.Now;
                
                var lastTransactionId =  ctx.TBL_RuleTransactions.Where(m => m.TBL_EmailRules.RuleFor == RulesFor.LOC
                && m.TBL_EmailRules.RuleName == "InterestDemand").Max(r => r.Id);
                if(lastTransactionId > 0)
                    triggerDate = ctx.TBL_RuleTransactions.Where(m => m.Id == lastTransactionId).Select(m => m.TriggerDate).FirstOrDefault();
                
                DateTime maxTriggerDate = new DateTime(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month,
                    DateTime.DaysInMonth(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month));

                if (lastTransactionId == 0 && !(DateTime.Now.Date >= triggerDate.Date && DateTime.Now.Date <= maxTriggerDate))
                {
                    var ids = ctx.TBL_LOC
                    .Join(ctx.Finacle_InterestDue, l => l.LocAccountNo.Trim(), fd => fd.AccountId.Trim(), (l, fd) => new { l, fd })
                    .Where(lfd => (lfd.fd.NextDate.HasValue && 
                        DbFunctions.TruncateTime(lfd.fd.NextDate) >= DbFunctions.TruncateTime(triggerDate)
                     && DbFunctions.TruncateTime(lfd.fd.NextDate) <= DbFunctions.TruncateTime(maxTriggerDate))
                     || (lfd.fd.LastDate.HasValue && DbFunctions.TruncateTime(lfd.fd.LastDate) >= DbFunctions.TruncateTime(triggerDate)
                     && DbFunctions.TruncateTime(lfd.fd.LastDate) <= DbFunctions.TruncateTime(maxTriggerDate)))
                    .Where(lfd => DbFunctions.DiffDays(triggerDate, lfd.fd.NextDate) >=7 
                    || DbFunctions.DiffDays(triggerDate, lfd.fd.LastDate) >= 7)
                    .Select(m => m.l.Id).Distinct().ToList();
                    return ids;
                }
                else
                {
                    return new List<int>();
                }
            }
        }

        public static LOCModel Verify_InterestDemand(int locId)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    DateTime triggerDate = DateTime.Now;

                    var lastTransactionId = ctx.TBL_RuleTransactions.Where(m => m.TBL_EmailRules.RuleFor == RulesFor.LOC
                   && m.TBL_EmailRules.RuleName == "InterestDemand").Max(r => r == null ? 0 : r.Id);
                    if (lastTransactionId > 0)
                        triggerDate = ctx.TBL_RuleTransactions.Where(m => m.Id == lastTransactionId).Select(m => m.TriggerDate).FirstOrDefault();

                    DateTime maxTriggerDate = new DateTime(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month,
                        DateTime.DaysInMonth(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month));

                    var loc = ctx.TBL_LOC
                         .Join(ctx.Finacle_InterestDue, l => l.LocAccountNo.Trim(), fd => fd.AccountId.Trim(), (l, fd) => new { l, fd })
                         .Where(lfd => lfd.l.Id == locId &&
                         (lfd.fd.NextDate.HasValue && DbFunctions.TruncateTime(lfd.fd.NextDate) >= DbFunctions.TruncateTime(triggerDate)
                          && DbFunctions.TruncateTime(lfd.fd.NextDate) <= DbFunctions.TruncateTime(maxTriggerDate))
                          || (lfd.fd.LastDate.HasValue && DbFunctions.TruncateTime(lfd.fd.LastDate) >= DbFunctions.TruncateTime(triggerDate)
                          && DbFunctions.TruncateTime(lfd.fd.LastDate) <= DbFunctions.TruncateTime(maxTriggerDate))
                         ).FirstOrDefault();
                    if (loc != null)
                    {
                        LOCModel model = new LOCModel();
                        model.Id = loc.l.Id;
                        model.LOCName = loc.l.Name;
                        model.LOCNumber = loc.l.LocNumber;
                        model.LOCAccountNumber = loc.l.LocAccountNo;
                        if (loc.fd.NextDate.HasValue && loc.fd.NextDate.Value.Date > triggerDate.Date && loc.fd.NextDate.Value < maxTriggerDate.Date)
                        {
                            model.InterestDueDate = loc.fd.NextDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (loc.fd.LastDate.HasValue && loc.fd.LastDate.Value.Date > triggerDate.Date && loc.fd.LastDate.Value < maxTriggerDate.Date)
                        {
                            model.InterestDueDate = loc.fd.LastDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            model.InterestDueDate = "";
                        }
                        if (model.InterestDueDate == "")
                            return null;
                        else
                            return model;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Verify_InterestDemand"
                    });
                    return null;
                }
            }
        }

        public static LOCModel Schedule_InterestDemand(int locId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {

                    var loc = ctx.TBL_LOC
                         .Join(ctx.Finacle_InterestDue, l => l.LocAccountNo.Trim(), fd => fd.AccountId.Trim(), (l, fd) => new { l, fd })
                         .Where(lfd => lfd.l.Id == locId ).FirstOrDefault();
                    if (loc != null)
                    {
                        LOCModel model = new LOCModel();
                        model.Id = loc.l.Id;
                        model.LOCName = loc.l.Name;
                        model.LOCNumber = loc.l.LocNumber;
                        model.LOCAccountNumber = loc.l.LocAccountNo;
                        model.InterestDueDate = notes;
                        if (model.InterestDueDate == "")
                            return null;
                        else
                            return model;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Schedule_InterestDemand"
                    });
                    return null;
                }
            }
        }

        public static List<int> PrincipalDemand(int offset)
        {
            using (var ctx = new AMPEntities())
            {
                DateTime triggerDate = DateTime.Now;

                var lastTransactionId = ctx.TBL_RuleTransactions.Where(m => m.TBL_EmailRules.RuleFor == RulesFor.LOC
               && m.TBL_EmailRules.RuleName == "PrincipalDemand").Max(r => r == null ? 0 : r.Id);
                if (lastTransactionId > 0)
                    triggerDate = ctx.TBL_RuleTransactions.Where(m => m.Id == lastTransactionId).Select(m => m.TriggerDate).FirstOrDefault();

                DateTime maxTriggerDate = new DateTime(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month,
                    DateTime.DaysInMonth(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month));

                if (lastTransactionId == 0 && !(DateTime.Now.Date >= triggerDate.Date && DateTime.Now.Date <= maxTriggerDate))
                {
                    var principalQuery = ctx.Finacle_PrincipalDue.Where(m => m.DueDate.HasValue &&
                    DbFunctions.TruncateTime(m.DueDate) >= DbFunctions.TruncateTime(triggerDate)
                    && DbFunctions.TruncateTime(m.DueDate) <= DbFunctions.TruncateTime(maxTriggerDate));

                    var ids = ctx.TBL_LOC
                    .Join(principalQuery, l => l.LocAccountNo.Trim(), fd => fd.AccountId.Trim(), (l, fd) => new { l, fd })
                    .Where(lfd => DbFunctions.DiffDays(triggerDate, lfd.fd.DueDate) >= 7)
                    .Select(m => m.l.Id).Distinct().ToList();
                    return ids;
                }
                else
                {
                    return new List<int>();
                }
            }
        }

        public static LOCModel Verify_PrincipalDemand(int locId)
        {
            using (var ctx = new AMPEntities())
            {
                DateTime triggerDate = DateTime.Now;

                var lastTransactionId = ctx.TBL_RuleTransactions.Where(m => m.TBL_EmailRules.RuleFor == RulesFor.LOC
               && m.TBL_EmailRules.RuleName == "PrincipalDemand").Max(r => r == null ? 0 : r.Id);
                if (lastTransactionId > 0)
                    triggerDate = ctx.TBL_RuleTransactions.Where(m => m.Id == lastTransactionId).Select(m => m.TriggerDate).FirstOrDefault();

                DateTime maxTriggerDate = new DateTime(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month,
                    DateTime.DaysInMonth(triggerDate.AddMonths(1).Year, triggerDate.AddMonths(1).Month));

                if (lastTransactionId == 0 && !(DateTime.Now.Date >= triggerDate.Date && DateTime.Now.Date <= maxTriggerDate))
                {
                    try
                    {
                        var principalQuery = ctx.Finacle_PrincipalDue.Where(m => m.DueDate.HasValue &&
                    DbFunctions.TruncateTime(m.DueDate) >= DbFunctions.TruncateTime(triggerDate)
                    && DbFunctions.TruncateTime(m.DueDate) <= DbFunctions.TruncateTime(maxTriggerDate));

                        var loc = ctx.TBL_LOC
                        .Join(principalQuery, l => l.LocAccountNo.Trim(), fd => fd.AccountId.Trim(), (l, fd) => new { l, fd })
                        .Where(lfd => lfd.l.Id == locId)
                        .FirstOrDefault();
                        if (loc != null)
                        {
                            LOCModel model = new LOCModel();
                            model.Id = loc.l.Id;
                            model.LOCName = loc.l.Name;
                            model.LOCNumber = loc.l.LocNumber;
                            model.LOCAccountNumber = loc.l.LocAccountNo;
                            if (loc.fd.DueDate.HasValue && loc.fd.DueDate.Value.Date > triggerDate.Date && loc.fd.DueDate.Value < maxTriggerDate.Date)
                            {
                                model.InterestDueDate = loc.fd.DueDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                model.InterestDueDate = "";
                            }
                            if (model.InterestDueDate == "")
                                return null;
                            else
                                return model;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch(Exception ex)
                    {
                        new Dashboard2ServiceLayer().InsertLog(new LogsModel
                        {
                            FullMessage = ex.Message,
                            Message = ex.Source,
                            ServiceName = "LocRules.Verify_PrincipalDemand"
                        });
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static LOCModel Schedule_PrincipalDemand(int locId, string notes)
        {
            using (var ctx = new AMPEntities())
            {

                try
                {
                    var loc = ctx.TBL_LOC.Where(lfd => lfd.Id == locId)
                    .FirstOrDefault();
                    if (loc != null)
                    {
                        LOCModel model = new LOCModel();
                        model.Id = loc.Id;
                        model.LOCName = loc.Name;
                        model.LOCNumber = loc.LocNumber;
                        model.LOCAccountNumber = loc.LocAccountNo;
                        model.InterestDueDate = notes;
                        if (model.InterestDueDate == "")
                            return null;
                        else
                            return model;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Schedule_PrincipalDemand"
                    });
                    return null;
                }
            }
        }

        public static List<int> CommitmentFee(int offset)
        {
            using (var ctx = new AMPEntities())
            {
                var ids = ctx.TBL_LOC.Join(ctx.Finacle_InterestDue, l => l.LocAccountNo, fi => fi.AccountId, (l, fi) => new { l, fi })
                    .Where(lfi => (lfi.fi.LastDate.HasValue && DbFunctions.DiffDays(DateTime.Now, lfi.fi.LastDate) == offset)
                    || (lfi.fi.NextDate.HasValue && DbFunctions.DiffDays(DateTime.Now, lfi.fi.NextDate) == offset))
                    .Select(lfi => lfi.l.Id).Distinct().ToList();

                return ids;
            }
        }

        public static DateTime? LastDate_CommitmentFee(int locId, int offset)
        {
            using (var ctx = new AMPEntities())
            {
                var locDetail = ctx.TBL_LOC.Join(ctx.Finacle_InterestDue, l => l.LocAccountNo, fi => fi.AccountId, (l, fi) => new { l, fi })
                    .Where(lfi => lfi.l.Id == locId &&
                    (lfi.fi.LastDate.HasValue && DbFunctions.DiffDays(DateTime.Now, lfi.fi.LastDate) == offset)
                    || (lfi.fi.NextDate.HasValue && DbFunctions.DiffDays(DateTime.Now, lfi.fi.NextDate) == offset))
                    .FirstOrDefault();

                if(locDetail.fi.LastDate.HasValue && (DateTime.Now - locDetail.fi.LastDate.Value).TotalDays == offset)
                {
                    return locDetail.fi.LastDate;
                }
                else if(locDetail.fi.NextDate.HasValue && (DateTime.Now - locDetail.fi.NextDate.Value).TotalDays == offset)
                {
                    return locDetail.fi.NextDate;
                }
                else
                {
                    return null;
                }
                
            }
        }

        public static LOCModel Verify_CommitmentFee(int locId, DateTime lastDate)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var model = ctx.TBL_LOC.Join(ctx.Finacle_InterestDue, l => l.LocAccountNo, fi => fi.AccountId, (l, fi) => new { l, fi })
                    .Where(lfi => lfi.l.Id == locId)
                    .ToList()
                    .Select(lfi => new LOCModel
                    {
                        Id = lfi.l.Id,
                        LOCName = lfi.l.Name,
                        LOCNumber = lfi.l.LocNumber,
                        LOCAccountNumber = lfi.l.LocAccountNo,
                        InterestDueDate = lastDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    }).FirstOrDefault();
                    return model;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Verify_CommitmentFee"
                    });
                    return null;
                }
            }
        }

        public static LOCModel Schedule_CommitmentFee(int locId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var model = ctx.TBL_LOC.Where(lfi => lfi.Id == locId)
                    .ToList()
                    .Select(lfi => new LOCModel
                    {
                        Id = lfi.Id,
                        LOCName = lfi.Name,
                        LOCNumber = lfi.LocNumber,
                        LOCAccountNumber = lfi.LocAccountNo,
                        InterestDueDate = notes
                    }).FirstOrDefault();
                    return model;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "LocRules.Schedule_CommitmentFee"
                    });
                    return null;
                }
            }
        }

    }
}