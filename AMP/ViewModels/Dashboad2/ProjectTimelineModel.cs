using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class ProjectTimelineModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTimeLine { get; set; }
        public string TimelineDate { get; set; }

    }
}