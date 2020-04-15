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
    public class ContractRules
    {
        public static List<int> TerminalDateofDisbursement(int offsetDays)
        {
            using(var ctx = new AMPEntities())
            {
                List<int> contractIds = ctx.TBL_Contracts.AsNoTracking().Join(ctx.Finacle_Contract_Transanctions, 
                    contract => contract.CGSId, finacle => finacle.AccountId, 
                    (contract, finacle) => new {Contract = contract, Finacle = finacle })
                    .Where(m => m.Contract.TerminalDateOfDisbursement.HasValue &&
                      DbFunctions.DiffDays(DateTime.Now, m.Contract.TerminalDateOfDisbursement) == offsetDays
                      && m.Finacle.CummulativeCredit.HasValue && m.Contract.EstimateValue.HasValue)
                    .Select(m => new { 
                        m.Contract.Id,
                        DisbursementDate = m.Contract.TerminalDateOfDisbursement,
                        ContractAmount = m.Contract.EstimateValue,
                        DisbursedAmount = m.Finacle.CummulativeCredit
                    })
                    .Where(m => m.DisbursedAmount < m.ContractAmount)
                    .Select(m => m.Id).Distinct().ToList();
                return contractIds;
            }
        }

        public static ContractModel Verify_TerminalDateofDisbursement(int contractId)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking().Join(ctx.Finacle_Contract_Transanctions,
                    contract => contract.CGSId, finacle => finacle.AccountId,
                    (contract, finacle) => new { Contract = contract, Finacle = finacle })
                    .Where(m => m.Contract.TerminalDateOfDisbursement.HasValue && m.Contract.Id == contractId
                        && DbFunctions.DiffDays(DateTime.Now, m.Contract.TerminalDateOfDisbursement) >= 0
                      && m.Finacle.CummulativeCredit.HasValue && m.Contract.EstimateValue.HasValue)
                    .ToList()
                    .Select(m => new ContractModel
                    {
                        Id = m.Contract.Id,
                        AmountDisbursed = m.Finacle.CummulativeCredit.HasValue ? m.Finacle.CummulativeCredit.Value : 0,
                        EstimateValue = m.Contract.EstimateValue.HasValue ? m.Contract.EstimateValue.Value : 0,
                        ProjectId = m.Contract.TBL_Projects_PQ.ProjectId,
                        CGSId = m.Contract.CGSId,
                        ContractorCIF = m.Contract.ContractorCIF,
                        ContractorName = m.Contract.ContractorName,
                        PackageDisplayId = m.Contract.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                        TypeOfPackage = m.Contract.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                        TerminalDateOfDisbursement = m.Contract.TerminalDateOfDisbursement.Value.ToString("dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture)
                    }).FirstOrDefault();
                    return cm.AmountDisbursed < cm.EstimateValue ? cm : null;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Verify_TerminalDateofDisbursement"
                    });
                    return null;
                }
            }
        }

        public static ContractModel Schedule_TerminalDateofDisbursement(int contractId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.Id == contractId)
                    .ToList();
                    var data =cm.Select(m => new ContractModel
                    {
                        Id = m.Id,
                        ProjectId = m.TBL_Projects_PQ.ProjectId,
                        CGSId = m.CGSId,
                        ContractorCIF = m.ContractorCIF,
                        ContractorName = m.ContractorName,
                        PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                        TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                        TerminalDateOfDisbursement = m.TerminalDateOfDisbursement.Value.ToString("dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture)
                    }).FirstOrDefault();
                    return data;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Schedule_TerminalDateofDisbursement"
                    });
                    return null;
                }
            }
        }

        public static List<int> AdvancePaymentGuarantee(int offsetDays)
        {
            using (var ctx = new AMPEntities())
            {
                List<int> contractIds = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.AdvPmtGrntExpiry.HasValue && DbFunctions.DiffDays(DateTime.Now, m.AdvPmtGrntExpiry) == offsetDays)
                    .Select(m => m.Id).Distinct().ToList();
                return contractIds;
            }
        }

        public static ContractModel Verify_AdvancePaymentGuarantee(int contractId)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.AdvPmtGrntExpiry.HasValue && m.Id == contractId
                    && DbFunctions.DiffDays(DateTime.Now, m.AdvPmtGrntExpiry) >= 0)
                   .ToList()
                   .Select(m => new ContractModel
                   {
                       Id = m.Id,
                       EstimateValue = m.EstimateValue.HasValue ? m.EstimateValue.Value : 0,
                       CGSId = m.CGSId,
                       ProjectId = m.TBL_Projects_PQ.ProjectId,
                       ContractorCIF = m.ContractorCIF,
                       ContractorName = m.ContractorName,
                       PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                       TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                       AdvPmtGrntExpiry = m.AdvPmtGrntExpiry.Value.ToString("dd/MM/yyyy",
                       System.Globalization.CultureInfo.InvariantCulture),
                       AdvPmtGrntAmount = m.AdvPmtGrntAmount.HasValue ? m.AdvPmtGrntAmount.Value : 0
                   }).FirstOrDefault();
                    return cm;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Verify_AdvancePaymentGuarantee"
                    });
                    return null;
                }
            }
        }

        public static ContractModel Schedule_AdvancePaymentGuarantee(int contractId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.Id == contractId)
                   .ToList()
                   .Select(m => new ContractModel
                   {
                       Id = m.Id,
                       EstimateValue = m.EstimateValue.HasValue ? m.EstimateValue.Value : 0,
                       CGSId = m.CGSId,
                       ProjectId = m.TBL_Projects_PQ.ProjectId,
                       ContractorCIF = m.ContractorCIF,
                       ContractorName = m.ContractorName,
                       PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                       TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                       AdvPmtGrntExpiry = m.AdvPmtGrntExpiry.Value.ToString("dd/MM/yyyy",
                       System.Globalization.CultureInfo.InvariantCulture),
                       AdvPmtGrntAmount = m.AdvPmtGrntAmount.HasValue ? m.AdvPmtGrntAmount.Value : 0
                   }).FirstOrDefault();
                    return cm;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Schedule_AdvancePaymentGuarantee"
                    });
                    return null;
                }
            }
        }

        public static List<int> PerformanceGuarantee(int offsetDays)
        {
            using (var ctx = new AMPEntities())
            {
                List<int> contractIds = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.PerBankGrntExpiry.HasValue && DbFunctions.DiffDays(DateTime.Now, m.PerBankGrntExpiry) == offsetDays)
                    .Select(m => m.Id).Distinct().ToList();
                return contractIds;
            }
        }

        public static ContractModel Verify_PerformanceGuarantee(int contractId)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.PerBankGrntExpiry.HasValue && m.Id == contractId
                    && DbFunctions.DiffDays(DateTime.Now, m.PerBankGrntExpiry) >= 0)
                   .ToList()
                   .Select(m => new ContractModel
                   {
                       Id = m.Id,
                       EstimateValue = m.EstimateValue.HasValue ? m.EstimateValue.Value : 0,
                       CGSId = m.CGSId,
                       ProjectId = m.TBL_Projects_PQ.ProjectId,
                       ContractorCIF = m.ContractorCIF,
                       ContractorName = m.ContractorName,
                       PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                       TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                       PerBankGrntExpiry = m.PerBankGrntExpiry.Value.ToString("dd/MM/yyyy",
                       System.Globalization.CultureInfo.InvariantCulture),
                       PerBankGrntAmount = m.PerBankGrntAmount.HasValue ? m.PerBankGrntAmount.Value : 0
                   }).FirstOrDefault();
                    return cm;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Verify_PerformanceGuarantee"
                    });
                    return null;
                }
            }
        }

        public static ContractModel Schedule_PerformanceGuarantee(int contractId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.Id == contractId)
                   .ToList()
                   .Select(m => new ContractModel
                   {
                       Id = m.Id,
                       EstimateValue = m.EstimateValue.HasValue ? m.EstimateValue.Value : 0,
                       CGSId = m.CGSId,
                       ProjectId = m.TBL_Projects_PQ.ProjectId,
                       ContractorCIF = m.ContractorCIF,
                       ContractorName = m.ContractorName,
                       PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                       TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                       PerBankGrntExpiry = m.PerBankGrntExpiry.Value.ToString("dd/MM/yyyy",
                       System.Globalization.CultureInfo.InvariantCulture),
                       PerBankGrntAmount = m.PerBankGrntAmount.HasValue ? m.PerBankGrntAmount.Value : 0
                   }).FirstOrDefault();
                    return cm;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Schedule_PerformanceGuarantee"
                    });
                    return null;
                }
            }
        }

        public static List<int> ScheduleCompleteDate(int offsetDays)
        {
            using (var ctx = new AMPEntities())
            {
                List<int> contractIds = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.ScheduledCompDate.HasValue && DbFunctions.DiffDays(DateTime.Now, m.ScheduledCompDate) == offsetDays)
                    .Select(m => m.Id).Distinct().ToList();
                return contractIds;
            }
        }

        public static ContractModel Verify_ScheduleCompleteDate(int contractId)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m => m.ScheduledCompDate.HasValue && m.Id == contractId
                    && DbFunctions.DiffDays(DateTime.Now, m.ScheduledCompDate) >= 0)
                   .ToList()
                   .Select(m => new ContractModel
                   {
                       Id = m.Id,
                       EstimateValue = m.EstimateValue.HasValue ? m.EstimateValue.Value : 0,
                       CGSId = m.CGSId,
                       ProjectId = m.TBL_Projects_PQ.ProjectId,
                       ContractorCIF = m.ContractorCIF,
                       ContractorName = m.ContractorName,
                       PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                       TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                       ScheduledCompDate = m.ScheduledCompDate.Value.ToString("dd/MM/yyyy",
                       System.Globalization.CultureInfo.InvariantCulture)
                   }).FirstOrDefault();
                    return cm;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Verify_ScheduleCompleteDate"
                    });
                    return null;
                }
            }
        }

        public static ContractModel Schedule_ScheduleCompleteDate(int contractId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var cm = ctx.TBL_Contracts.AsNoTracking()
                    .Where(m =>  m.Id == contractId)
                   .ToList()
                   .Select(m => new ContractModel
                   {
                       Id = m.Id,
                       EstimateValue = m.EstimateValue.HasValue ? m.EstimateValue.Value : 0,
                       CGSId = m.CGSId,
                       ProjectId = m.TBL_Projects_PQ.ProjectId,
                       ContractorCIF = m.ContractorCIF,
                       ContractorName = m.ContractorName,
                       PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                       TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                       ScheduledCompDate = m.ScheduledCompDate.Value.ToString("dd/MM/yyyy",
                       System.Globalization.CultureInfo.InvariantCulture)
                   }).FirstOrDefault();
                    return cm;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Schedule_ScheduleCompleteDate"
                    });
                    return null;
                }
            }
        }

        public static List<int> FirstCommitmentFeeCat1(int offsetDays)
        {
            using (var ctx = new AMPEntities())
            {
                var insetredLoc = ctx.TBL_RuleTransactions.Where(m => m.TBL_EmailRules.RuleFor.Contains("Contract")
                && m.TBL_EmailRules.RuleName.Contains("FirstCommitmentFeeCat1")).Select(m => m.RecordId);

                var ids =  ctx.TBL_Contracts.Join(ctx.TBL_LOC_Contract, c => c.Id, lc => lc.ContractId, (c, lc) => new {c, lc })
                    .Where(m => m.lc.TBL_LOC.MEAAppDate.HasValue && m.lc.TBL_LOC.Classification.Contains("Cat I as per 2015 Guidelines")
                && DbFunctions.DiffMonths(DateTime.Now, m.lc.TBL_LOC.MEAAppDate) > 0
                && DbFunctions.DiffMonths(DateTime.Now, m.lc.TBL_LOC.MEAAppDate) <= 12
                && !insetredLoc.Contains(m.c.Id)).Select(m => m.c.Id).Distinct().ToList();

                return ids;
            }
        }

        public static ContractModel Verify_FirstCommitmentFeeCat1(int recordId)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var loc_contracts = ctx.TBL_Contracts.Where(m => m.Id == recordId).FirstOrDefault();
                    var contract = loc_contracts.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault();

                    var locInterestDueDateInfo = ctx.TBL_LOC.Join(ctx.Finacle_InterestDue, l => l.LocAccountNo, fi => fi.AccountId, (l, fi) => new { l, fi })
                        .Where(m => m.fi.LastDate.HasValue && DbFunctions.TruncateTime(m.fi.LastDate) > DbFunctions.TruncateTime(DateTime.Now)
                        && m.l.Id == contract.LocId).Select(m => new { LocId = m.l.Id, DueDate = m.fi.LastDate }).Distinct().FirstOrDefault();
                    if (locInterestDueDateInfo != null)
                    {
                        var data = ctx.TBL_Contracts.Where(m => m.Id == recordId).ToList().Select(m => new ContractModel
                        {
                            Id = m.Id,
                            ContractorName = m.ContractorName,
                            CGSId = m.CGSId,
                            PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                            TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                            SigningDate = locInterestDueDateInfo.DueDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        }).FirstOrDefault();
                        return data;
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
                        ServiceName = "ContractRules.Verify_FirstCommitmentFeeCat1"
                    });
                    return null;
                }
            }
        }

        public static ContractModel Schedule_FirstCommitmentFeeCat1(int recordId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var data = ctx.TBL_Contracts.Where(m => m.Id == recordId).ToList().Select(m => new ContractModel
                    {
                        Id = m.Id,
                        ContractorName = m.ContractorName,
                        CGSId = m.CGSId,
                        PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault()?.TBL_LOC.LocNumber,
                        TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault()?.TBL_LOC.Name,
                        SigningDate = notes,
                    }).Select(m => new ContractModel
                        {
                            Id = m.Id,
                            ContractorName = m.ContractorName,
                            CGSId = m.CGSId,
                            PackageDisplayId = string.IsNullOrEmpty(m.PackageDisplayId) == true ? "NA" : m.PackageDisplayId,
                            TypeOfPackage = string.IsNullOrEmpty(m.TypeOfPackage) == true ? "NA" : m.TypeOfPackage,
                            SigningDate = notes,
                        }).FirstOrDefault();
                    return data;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Schedule_FirstCommitmentFeeCat1"
                    });
                    return null;
                }
            }
        }

        public static List<int> FirstCommitmentFeeOCat(int offsetDays)
        {
            using (var ctx = new AMPEntities())
            {
                var insetredLoc = ctx.TBL_RuleTransactions.Where(m => m.TBL_EmailRules.RuleFor.Contains("Contract")
                && m.TBL_EmailRules.RuleName.Contains("FirstCommitmentFeeCat1")).Select(m => m.RecordId);

                var ids = ctx.TBL_Contracts.Join(ctx.TBL_LOC_Contract, c => c.Id, lc => lc.ContractId, (c, lc) => new { c, lc })
                    .Where(m => m.lc.TBL_LOC.MEAAppDate.HasValue 
                    && (m.lc.TBL_LOC.Classification.Contains("Cat II as per 2015 Guidelines") || m.lc.TBL_LOC.Classification.Contains("Cat III as per 2015 Guidelines"))
                && DbFunctions.DiffMonths(DateTime.Now, m.lc.TBL_LOC.MEAAppDate) > 0
                && DbFunctions.DiffMonths(DateTime.Now, m.lc.TBL_LOC.MEAAppDate) <= 2
                && !insetredLoc.Contains(m.c.Id)).Select(m => m.c.Id).Distinct().ToList();

                return ids;
            }
        }

        public static ContractModel Verify_FirstCommitmentFeeOCat(int recordId)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var loc_contracts = ctx.TBL_Contracts.Where(m => m.Id == recordId).FirstOrDefault();
                    var contract = loc_contracts.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault();

                    var locInterestDueDateInfo = ctx.TBL_LOC.Join(ctx.Finacle_InterestDue, l => l.LocAccountNo, fi => fi.AccountId, (l, fi) => new { l, fi })
                        .Where(m => m.fi.LastDate.HasValue && DbFunctions.TruncateTime(m.fi.LastDate) > DbFunctions.TruncateTime(DateTime.Now)
                        && m.l.Id == contract.LocId).Select(m => new { LocId = m.l.Id, DueDate = m.fi.LastDate }).Distinct().FirstOrDefault();
                    if (locInterestDueDateInfo != null)
                    {
                        var data = ctx.TBL_Contracts.Where(m => m.Id == recordId).ToList().Select(m => new ContractModel
                        {
                            Id = m.Id,
                            ContractorName = m.ContractorName,
                            CGSId = m.CGSId,
                            PackageDisplayId = contract.TBL_LOC.LocNumber,
                            TypeOfPackage = contract.TBL_LOC.Name,
                            SigningDate = locInterestDueDateInfo.DueDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        }).FirstOrDefault();
                        return data;
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
                        ServiceName = "ContractRules.Verify_FirstCommitmentFeeCat1"
                    });
                    return null;
                }
            }
        }

        public static ContractModel Schedule_FirstCommitmentFeeOCat(int recordId, string notes)
        {
            using (var ctx = new AMPEntities())
            {
                try
                {
                    var data = ctx.TBL_Contracts.Where(m => m.Id == recordId).ToList().Select(m => new ContractModel
                    {
                        Id = m.Id,
                        ContractorName = m.ContractorName,
                        CGSId = m.CGSId,
                        PackageDisplayId = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault()?.TBL_LOC.LocNumber,
                        TypeOfPackage = m.TBL_Projects_PQ.TBL_Projects.TBL_LOC_Project.FirstOrDefault()?.TBL_LOC.Name,
                        SigningDate = notes,
                    }).Select(m => new ContractModel
                    {
                        Id = m.Id,
                        ContractorName = m.ContractorName,
                        CGSId = m.CGSId,
                        PackageDisplayId = string.IsNullOrEmpty(m.PackageDisplayId) == true ? "NA" : m.PackageDisplayId,
                        TypeOfPackage = string.IsNullOrEmpty(m.TypeOfPackage) == true ? "NA" : m.TypeOfPackage,
                        SigningDate = notes,
                    }).FirstOrDefault();
                    return data;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ContractRules.Schedule_FirstCommitmentFeeOCat"
                    });
                    return null;
                }
            }
        }
    }
}