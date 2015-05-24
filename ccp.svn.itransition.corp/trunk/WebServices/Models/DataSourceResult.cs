using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCP.WebApi.Models
{
    public class DataSourceResult
    {
        public IList Data { get; set; }
        public int DataCount { get; set; }
    }
}