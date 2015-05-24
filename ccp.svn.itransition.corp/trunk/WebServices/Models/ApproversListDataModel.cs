using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCP.WebApi.Models
{
    public class ApproversListDataModel
    {
        public string Term { get; set; }
        public long UserId { get; set; }
        public long[] ApproversIds { get; set; }
    }
}