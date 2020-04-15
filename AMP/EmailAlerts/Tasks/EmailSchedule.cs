using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmailProvider.Models;
using EmailProvider;
using AMP.ViewModels.Dashboad2;
using FluentScheduler;
using System.Web.Hosting;
using AMP.Services;

namespace AMP.EmailAlerts.Tasks
{
    public class EmailSchedule : IJob, IRegisteredObject
    {
        public void Execute()
        {
            SendMail();
        }
        async void SendMail()
        {
            Dashboard2ServiceLayer d2s = new Dashboard2ServiceLayer();
            List<MailDB> list = new List<MailDB>();
            list = d2s.FetchPendingMails();
            foreach(MailDB l in list)
            {
                bool status = false;
                try
                {
                   var mailStatus = await EmailService.SendMailsAsync(l);
                    status = mailStatus.Status;
                }
                catch
                {
                    status = false;
                }
                if (status)
                {
                    d2s.UpdatePendingMails(l.Id);
                }
                    
            }
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);
        }
    }
}