using AMP.Services;
using FluentScheduler;
using System.Web.Hosting;

namespace AMP.Finacle.Tasks
{
    public class FinacleSync : IJob, IRegisteredObject
    {
        public void Execute()
        {
            //Sync Logic
            string exp = "", error = "";
            bool status = true;
            FinacleConnector conn = new FinacleConnector();

            status = conn.LocDetails(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "LocDetails");

            status = conn.AmpContracts(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "AmpContracts");

            status = conn.AmpDemands(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "AmpDemands");

            status = conn.CgsView(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "CgsView");

            status = conn.Disbursment(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "Disbursment");

            status = conn.RepaymentSchdule(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "RepaymentSchdule");

            status = conn.PrincipalDue(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "PrincipalDue");

            status = conn.InterestDue(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "InterestDue");

            status = conn.ContractTransactions(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "ContractTransactions");

            status = conn.LocTransactions(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "LocTransactions");

            status = conn.LOCFinancials(out exp) ? status : false;
            error += "::>" + exp;
            logit(exp, "LOCFinancials");

            if (status)
                logit("", "FinacleSyncSuccess");
        }

        public void logit(string exp, string serviceName)
        {
            new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            {
                Status = exp == "" ? "Success" : "Fail",
                System = "Finacle",
                FullMessage = exp,
                ServiceName = serviceName,
            });
        }
        
        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);
        }    
    }
}