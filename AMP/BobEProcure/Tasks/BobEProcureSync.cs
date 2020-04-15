using AMP.Services;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace AMP.BobEProcure.Tasks
{
    public class BobEProcureSync : IJob, IRegisteredObject
    {
        public void Execute()
        {
            //string exp = "", error = "";
            //bool status = true;
            //BobEProcureConnector conn = new BobEProcureConnector();
            //status = conn.PackageSync(out exp) ? status : false;
            //error += "::>" + exp;
            //new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            //{
            //    Status = exp == "" ? "Success" : "Fail",
            //    System = "Bob eProcure",
            //    FullMessage = exp.ToString(),
            //    ServiceName = "PackageSync",
            //});
            //status = conn.StatusSync(out exp) ? status : false;
            //error += "::>" + exp;
            //new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            //{
            //    Status = exp == "" ? "Success" : "Fail",
            //    System = "Bob eProcure",
            //    FullMessage = exp.ToString(),
            //    ServiceName = "StatusSync",
            //});
            //status = conn.VendorSync(out exp) ? status : false;
            //error += "::>" + exp;
            //new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            //{
            //    Status = exp == "" ? "Success" : "Fail",
            //    System = "Bob eProcure",
            //    FullMessage = exp.ToString(),
            //    ServiceName = "VendorSync",
            //});

            //if (status)
            //{
            //    new Dashboard2ServiceLayer().InsertSyncLog(new ViewModels.Dashboad2.SyncModel()
            //    {
            //        Status = "Success",
            //        System = "Bob eProcure",
            //        FullMessage = "",
            //        ServiceName = "BobSyncSuccess",
            //    });
            //}
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);
        }
    }
}