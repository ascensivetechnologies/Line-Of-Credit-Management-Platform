using FluentScheduler;
using System.Web.Hosting;
using System;
using AMP.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace AMP.Finacle.Tasks 
{
    public class MonthlyFinacleDataSync : IJob, IRegisteredObject
    {
        public void Execute()
        {
            //int month = DateTime.Now.Month==1 ? 12: DateTime.Now.Month-1;
            //string MonthName= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            //Console.WriteLine("hellllllloooooo");
            Dashboard2ServiceLayer d2s = new Dashboard2ServiceLayer();
            d2s.InsertMonthlyFinacleData(DateTime.Now);
            
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);
        }
    }
}