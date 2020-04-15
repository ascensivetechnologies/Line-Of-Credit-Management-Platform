using AMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AMP.ViewModels.Dashboad2;
using AMP.Services;

namespace AMP.EmailAlerts.Rules
{
    public class ProjectRules
    {
        public static List<int> BidsSubmissionPQ(int offset)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                var result = ampEntities.TBL_Projects_PQ.Where(e => e.LastSubmissionOn.HasValue && DbFunctions.DiffDays(DateTime.Now, e.LastSubmissionOn) == offset).Select(e => e.Id).ToList();
                return result;
            }
        }

        public static ProjectPqModel Verify_BidsSubmissionPQ(int ProjectPqId)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                try
                {
                    var result = ampEntities.TBL_Projects_PQ.Where(e => e.Id == ProjectPqId &&
                             e.LastSubmissionOn.HasValue && DbFunctions.DiffDays(DateTime.Now, e.LastSubmissionOn) >= 0).ToList()
                             .Select(e =>
                             {
                                 return new ProjectPqModel()
                                 {
                                     LocNumber = e.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                                     Country = e.Country,
                                     Name = e.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                                     LastSubmissionOn = e.LastSubmissionOn.ToString(),
                                     Category = e.Category,
                                     ProjectId = e.ProjectId
                                 };

                             }).FirstOrDefault();

                    return result;
                }
                catch(Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ProjectRules.Verify_BidsSubmissionPQ"
                    });
                    return null;
                }
            }
        }

        public static ProjectPqModel Schedule_BidsSubmissionPQ(int ProjectPqId, string notes)
        {
            using (AMPEntities ampEntities = new AMPEntities())
            {
                try
                {
                    var result = ampEntities.TBL_Projects_PQ.Where(e => e.Id == ProjectPqId).ToList()
                             .Select(e =>
                             {
                                 return new ProjectPqModel()
                                 {
                                     LocNumber = e.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.LocNumber,
                                     Country = e.Country,
                                     Name = e.TBL_Projects.TBL_LOC_Project.FirstOrDefault().TBL_LOC.Name,
                                     LastSubmissionOn = e.LastSubmissionOn.ToString(),
                                     Category = e.Category,
                                     ProjectId = e.ProjectId
                                 };

                             }).FirstOrDefault();

                    return result;
                }
                catch (Exception ex)
                {
                    new Dashboard2ServiceLayer().InsertLog(new LogsModel
                    {
                        FullMessage = ex.Message,
                        Message = ex.Source,
                        ServiceName = "ProjectRules.Schedule_BidsSubmissionPQ"
                    });
                    return null;
                }
            }
        }
    }
}