using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AMP.Models
{
    public class ProcedureParams
    {
        public string Name { get; set; }
        public SqlDbType Type { get; set; }
        public string Value { get; set; }

    }
}