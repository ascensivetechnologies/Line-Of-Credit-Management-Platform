using FluentScheduler;
using System.Web.Hosting;
using System;
namespace AMP.EmailAlerts.Tasks
{
    public class EmailBodySchedule : IJob, IRegisteredObject
    {
        public void Execute()
        {
            EmailRuleService ruleService = new EmailRuleService();
            //ruleService.GenerateRuleTransactions();
            //ruleService.GenerateMailBody();
            ruleService.GenerateEmailSchedule(runDate: DateTime.Now.Date);
            ruleService.GenerateMailBodyFromEmailSchedule(rundate: DateTime.Now.Date);
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);
        }
    }
}