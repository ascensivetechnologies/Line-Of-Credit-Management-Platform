using AMP.EmailAlerts.Rules;
using AMP.Models;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EmailProvider.Services;
using AMP.Services;
using System.Data.SqlClient;

namespace AMP.EmailAlerts
{
    public class EmailRuleService
    {
        private int GetTotalDays(int monthCount, int weekCount, int dayCount) => monthCount * 30 + weekCount * 7 + dayCount;
        public void GenerateRuleTransactions()
        {
            List<TBL_EmailRules> allRules;
            using (var ctx = new AMPEntities())
            {
                allRules = ctx.TBL_EmailRules.AsNoTracking().Where(m => m.IsActive).ToList();
            }
            foreach(var rule in allRules)
            {
                var matchedRecords = GetMatchedRecordsForRule(rule);
                InsertRuleTransactions(matchedRecords, rule);
            }
        }

        public void GenerateEmailSchedule(DateTime runDate)
        {
            string command = "exec CreateEmailSchedule @rundate";
            SqlParameter parameter = new SqlParameter("@rundate", runDate);
            using(var ctx = new AMPEntities())
            {
                ctx.Database.ExecuteSqlCommand(command, parameter);
            }
        }

        private List<int> GetMatchedRecordsForRule(TBL_EmailRules emailRule)
        {
            string className = emailRule.RuleName.Split('.')[0];
            string methodName = emailRule.RuleName.Split('.')[1];
            int offsetDays = GetOffsetDays(emailRule);
            Type ruleType = Type.GetType("AMP.EmailAlerts.Rules." + className);
            List<int> ids = new List<int>();
            try
            {
                ids = (List<int>)ruleType.InvokeMember(methodName, System.Reflection.BindingFlags.InvokeMethod
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Static, null, null, new Object[] { offsetDays });
            }
            catch(Exception ex)
            {
                new Dashboard2ServiceLayer().InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "EmailRuleService.GetMatchedRecordsForRule"
                });
            }
            return ids;
        }

        private int GetOffsetDays(TBL_EmailRules emailRule)
        {
            return GetTotalDays(emailRule.OffMonth.HasValue ? emailRule.OffMonth.Value : 0,
                emailRule.OffWeek.HasValue ? emailRule.OffWeek.Value : 0,
                emailRule.OffDay.HasValue ? emailRule.OffDay.Value : 0);
        }

        private void InsertRuleTransactions(List<int> ids, TBL_EmailRules emailRule)
        {
            using (var ctx = new AMPEntities())
            {
                foreach(var id in ids)
                {
                    TBL_RuleTransactions transaction = new TBL_RuleTransactions();
                    transaction.IsActive = true;
                    transaction.TriggerDate = DateTime.Now;
                    transaction.NextRunDate = DateTime.Now;
                    transaction.RecordId = id;
                    transaction.RuleId = emailRule.Id;
                    if (emailRule.RuleName.Contains("CommitmentFee"))
                    {
                        transaction.NextRunDate = GetClosestMondayForDate(transaction.NextRunDate);
                        transaction.LastRunDate = LocRules.LastDate_CommitmentFee(id, GetOffsetDays(emailRule));
                        ctx.TBL_RuleTransactions.Add(transaction);
                    }
                    else
                    {
                        ctx.TBL_RuleTransactions.Add(transaction);
                    }
                    
                }
                ctx.SaveChanges();
            }
        }

        private DateTime GetClosestMondayForDate(DateTime inputDate)
        {
            DateTime result = inputDate;
            for(DateTime dt = inputDate; dt.Date <= inputDate.AddDays(7).Date; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == DayOfWeek.Monday)
                {
                    result = dt;
                    break;
                }
            }
            return result;
        }

        public void GenerateMailBody()
        {
            using(var ctx =new AMPEntities())
            {
                var activeRuleTransactions = ctx.TBL_RuleTransactions.Where(m => m.IsActive
                && DbFunctions.TruncateTime(m.NextRunDate) <= DbFunctions.TruncateTime(DateTime.Now))
                    .ToList();
                foreach(var rule in activeRuleTransactions)
                {
                    if(rule.TBL_EmailRules.RuleFor == RulesFor.Contract)
                    {
                        BodyForContractRules(ctx, rule);
                    }
                    if (rule.TBL_EmailRules.RuleFor == RulesFor.Project)
                    {
                        BodyForProjectRules(ctx, rule);
                    }
                    if (rule.TBL_EmailRules.RuleFor == RulesFor.LOC)
                    {
                        BodyForLocRules(ctx, rule);
                    }
                }
                ctx.SaveChanges();
            }
            
        }

        public void GenerateMailBodyFromEmailSchedule(DateTime rundate)
        {
            using(var ctx = new AMPEntities())
            {
                var allRecords = ctx.TBL_EmailSchedule.Where(m => DbFunctions.DiffDays(m.SendDate, rundate) == 0).ToList();
                foreach(var rec in allRecords)
                {
                    if(rec.RecordType == RulesFor.LOC)
                    {
                        BodyForLocRules(ctx, rec);
                    }
                    else if (rec.RecordType == RulesFor.Contract)
                    {
                        BodyForContractRules(ctx, rec);
                    }
                    else if (rec.RecordType == RulesFor.Project)
                    {
                        BodyForProjectRules(ctx, rec);
                    }
                }
            }
        }

        private void BodyForContractRules(AMPEntities ctx, TBL_RuleTransactions rule)
        {
            var data = GetVerifiedRecordForRules<ContractModel>(rule.RecordId.HasValue ? rule.RecordId.Value : 0, rule.TBL_EmailRules);
            if (data == null)
            {
                rule.LastRunDate = rule.NextRunDate;
                rule.IsActive = false;
            }
            else
            {
                int nextRunDays = GetTotalDays(rule.TBL_EmailRules.FreqMonth.HasValue ? rule.TBL_EmailRules.FreqMonth.Value : 0,
                    rule.TBL_EmailRules.FreqWeek.HasValue ? rule.TBL_EmailRules.FreqWeek.Value : 0,
                    rule.TBL_EmailRules.FreqDay.HasValue ? rule.TBL_EmailRules.FreqDay.Value : 0);
                var templatePath = TemplateReader.GetHtmlTemplatePath(rule.TBL_EmailRules.RuleName);
                var body = EmailBodyService.CreateEmailBody<ContractModel>(data, templatePath);
                var teams = new Dashboard2ServiceLayer().GetTeam("TBL_Project", data.ProjectId);
                var to = teams.Select(m => m.Email);
                if (!to.Any())
                    to = new Dashboard2ServiceLayer().GetAllStringMappers("DefaultEmail").Select(m => m.Value);
                var toAddress = string.Join(",", to);
                var subject = new Dashboard2ServiceLayer().GetAllStringMappers("Subject." + rule.TBL_EmailRules.RuleName).FirstOrDefault();
                if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(body))
                    InsertEmailToSend(ctx, subject.Value, body, rule.Id, toAddress, "", "");
                rule.NextRunDate = rule.NextRunDate.AddDays(nextRunDays);
                rule.IsActive = true;
            }
        }

        private void BodyForContractRules(AMPEntities ctx, TBL_EmailSchedule schedule)
        {
            var data = GetScheduledRecordForRules<ContractModel>(schedule.RecordId.HasValue ? schedule.RecordId.Value : 0, schedule.Notes, schedule.TemplateName);
            if(data != null)
            {
                var templatePath = TemplateReader.GetHtmlTemplatePath(schedule.TemplateName);
                var body = EmailBodyService.CreateEmailBody<ContractModel>(data, templatePath);
                var teams = new Dashboard2ServiceLayer().GetTeam("TBL_Project", data.ProjectId);
                var to = teams.Select(m => m.Email);
                if (!to.Any())
                    to = new Dashboard2ServiceLayer().GetAllStringMappers("DefaultEmail").Select(m => m.Value);
                var toAddress = string.Join(",", to);
                var subject = new Dashboard2ServiceLayer().GetAllStringMappers("Subject." + schedule.TemplateName).FirstOrDefault();
                if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(body))
                    InsertEmailToSend(ctx, subject.Value, body, schedule.Id, toAddress, "", "");
            }
        }

        private void BodyForProjectRules(AMPEntities ctx, TBL_RuleTransactions rule)
        {
            var data = GetVerifiedRecordForProjectRules(rule.RecordId.HasValue ? rule.RecordId.Value : 0, rule.TBL_EmailRules);
            if (data == null)
            {
                rule.LastRunDate = rule.NextRunDate;
                rule.IsActive = false;
            }
            else
            {
                int nextRunDays = GetTotalDays(rule.TBL_EmailRules.FreqMonth.HasValue ? rule.TBL_EmailRules.FreqMonth.Value : 0,
                    rule.TBL_EmailRules.FreqWeek.HasValue ? rule.TBL_EmailRules.FreqWeek.Value : 0,
                    rule.TBL_EmailRules.FreqDay.HasValue ? rule.TBL_EmailRules.FreqDay.Value : 0);
                var templatePath = TemplateReader.GetHtmlTemplatePath(rule.TBL_EmailRules.RuleName);
                var body = EmailBodyService.CreateEmailBody<ProjectPqModel>(data, templatePath);
                var teams = new Dashboard2ServiceLayer().GetTeam("TBL_Project", data.ProjectId);
                var to = teams.Select(m => m.Email);
                if (!to.Any())
                    to = new Dashboard2ServiceLayer().GetAllStringMappers("DefaultEmail").Select(m => m.Value);
                var toAddress = string.Join(",", to);
                var subject = new Dashboard2ServiceLayer().GetAllStringMappers("Subject." + rule.TBL_EmailRules.RuleName).FirstOrDefault();
                if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(body))
                    InsertEmailToSend(ctx, subject.Value, body, rule.Id, toAddress, "", "");
                rule.NextRunDate = rule.NextRunDate.AddDays(nextRunDays);
                rule.IsActive = true;
            }
        }

        private void BodyForProjectRules(AMPEntities ctx, TBL_EmailSchedule schedule)
        {
            var data = GetScheduledRecordForRules<ProjectPqModel>(schedule.RecordId.HasValue ? schedule.RecordId.Value : 0, schedule.Notes, schedule.TemplateName);
            if (data != null)
            {
                var templatePath = TemplateReader.GetHtmlTemplatePath(schedule.TemplateName);
                var body = EmailBodyService.CreateEmailBody<ProjectPqModel>(data, templatePath);
                var teams = new Dashboard2ServiceLayer().GetTeam("TBL_Project", data.Id);
                var to = teams.Select(m => m.Email);
                if (!to.Any())
                    to = new Dashboard2ServiceLayer().GetAllStringMappers("DefaultEmail").Select(m => m.Value);
                var toAddress = string.Join(",", to);
                var subject = new Dashboard2ServiceLayer().GetAllStringMappers("Subject." + schedule.TemplateName).FirstOrDefault();
                if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(body))
                    InsertEmailToSend(ctx, subject.Value, body, schedule.Id, toAddress, "", "");
            }
        }

        private void BodyForLocRules(AMPEntities ctx, TBL_RuleTransactions rule)
        {
            var data = GetVerifiedRecordForRules<LOCModel>(rule.RecordId.HasValue ? rule.RecordId.Value : 0, rule.TBL_EmailRules);
            if (data == null)
            {
                rule.LastRunDate = rule.NextRunDate;
                rule.IsActive = false;
            }
            else
            {
                if(rule.LastRunDate.HasValue && rule.NextRunDate.Date < rule.LastRunDate.Value.Date)
                {
                    int nextRunDays = GetTotalDays(rule.TBL_EmailRules.FreqMonth.HasValue ? rule.TBL_EmailRules.FreqMonth.Value : 0,
                    rule.TBL_EmailRules.FreqWeek.HasValue ? rule.TBL_EmailRules.FreqWeek.Value : 0,
                    rule.TBL_EmailRules.FreqDay.HasValue ? rule.TBL_EmailRules.FreqDay.Value : 0);
                    var templatePath = TemplateReader.GetHtmlTemplatePath(rule.TBL_EmailRules.RuleName);
                    var body = EmailBodyService.CreateEmailBody<LOCModel>(data, templatePath);
                    var teams = new Dashboard2ServiceLayer().GetTeam("TBL_Loc", data.Id);
                    var to = teams.Select(m => m.Email);
                    if (!to.Any())
                        to = new Dashboard2ServiceLayer().GetAllStringMappers("DefaultEmail").Select(m => m.Value);
                    var toAddress = string.Join(",", to);
                    var subject = new Dashboard2ServiceLayer().GetAllStringMappers("Subject." + rule.TBL_EmailRules.RuleName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(body))
                        InsertEmailToSend(ctx, subject.Value, body, rule.Id, toAddress, "", "");
                    rule.NextRunDate = rule.NextRunDate.AddDays(nextRunDays);
                    rule.IsActive = true;
                }
                else
                {
                    rule.IsActive = false;
                }
                
            }
        }

        private void BodyForLocRules(AMPEntities ctx, TBL_EmailSchedule schedule)
        {
            var data = GetScheduledRecordForRules<LOCModel>(schedule.RecordId.HasValue ? schedule.RecordId.Value : 0, schedule.Notes, schedule.TemplateName);
            if (data != null)
            {
                var templatePath = TemplateReader.GetHtmlTemplatePath(schedule.TemplateName);
                var body = EmailBodyService.CreateEmailBody<LOCModel>(data, templatePath);
                var teams = new Dashboard2ServiceLayer().GetTeam("TBL_Loc", data.Id);
                var to = teams.Select(m => m.Email);
                if (!to.Any())
                    to = new Dashboard2ServiceLayer().GetAllStringMappers("DefaultEmail").Select(m => m.Value);
                var toAddress = string.Join(",", to);
                var subject = new Dashboard2ServiceLayer().GetAllStringMappers("Subject." + schedule.TemplateName).FirstOrDefault();
                if (!string.IsNullOrEmpty(toAddress) && !string.IsNullOrEmpty(body))
                    InsertEmailToSend(ctx, subject.Value, body, schedule.Id, toAddress, "", "");
            }
        }

        private T GetVerifiedRecordForRules<T>(int recordId, TBL_EmailRules emailRule) where T : class
        {
            string className = emailRule.RuleName.Split('.')[0];
            string methodName = "Verify_" + emailRule.RuleName.Split('.')[1];
            Type ruleType = Type.GetType("AMP.EmailAlerts.Rules." + className);
            try
            {
                T data = (T)ruleType.InvokeMember(methodName, System.Reflection.BindingFlags.InvokeMethod
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Static, null, null, new Object[] { recordId });
                return data;
            }
            catch (Exception ex)
            {
                new Dashboard2ServiceLayer().InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "EmailRuleService.GetVerifiedRecordForRules"
                });
                return null;
            }
        }

        private T GetScheduledRecordForRules<T>(int recordId, string notes, string ruleName) where T : class
        {
            string className = ruleName.Split('.')[0];
            string methodName = "Schedule_" + ruleName.Split('.')[1];
            Type ruleType = Type.GetType("AMP.EmailAlerts.Rules." + className);
            try
            {
                T data = (T)ruleType.InvokeMember(methodName, System.Reflection.BindingFlags.InvokeMethod
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Static, null, null, new Object[] { recordId, notes });
                return data;
            }
            catch (Exception ex)
            {
                new Dashboard2ServiceLayer().InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "EmailRuleService.GetScheduledRecordForRules"
                });
                return null;
            }
        }

        private ProjectPqModel GetVerifiedRecordForProjectRules(int recordId, TBL_EmailRules emailRule)
        {
            if (emailRule.RuleName.Contains("Verify_BidsSubmissionPQ"))
            {
                return ProjectRules.Verify_BidsSubmissionPQ(recordId);
            }
            else
            {
                return null;
            }
        }

        private void InsertEmailToSend(AMPEntities ctx, string subject, string body, int transactionId, string toAddress, string ccAddress, string bccAddress)
        {
            TBL_MailBody mailBody = new TBL_MailBody();
            mailBody.MailSubject = subject;
            mailBody.mail_body = body;
            mailBody.RuleTransactionId = transactionId;
            mailBody.to_address = toAddress;
            mailBody.CreatedOn = DateTime.Now;
            mailBody.cc = ccAddress;
            mailBody.bcc = bccAddress;
            mailBody.status = "Pending";
            ctx.TBL_MailBody.Add(mailBody);
            ctx.SaveChanges();
        }
    }
}