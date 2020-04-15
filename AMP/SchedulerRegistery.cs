using AMP.BobEProcure.Tasks;
using AMP.Finacle.Tasks;
using AMP.EmailAlerts.Tasks;
using FluentScheduler;

namespace AMP
{
    public class SchedulerRegistery : Registry
    {
        public SchedulerRegistery()
        {
            Schedule<FinacleSync>().NonReentrant().ToRunNow().AndEvery(3600);
            //Schedule<BobEProcureSync>().NonReentrant().ToRunNow().AndEvery(3600);
            Schedule<EmailBodySchedule>().NonReentrant().ToRunEvery(1).Days().At(05, 30);
            Schedule<EmailSchedule>().NonReentrant().ToRunEvery(1).Days().At(09, 30);
            Schedule<MonthlyFinacleDataSync>().NonReentrant().ToRunEvery(1).Months().On(1).At(00, 30);
            //Schedule<MonthlyFinacleDataSync>().NonReentrant().ToRunNow().AndEvery(10);
        }
    }
}