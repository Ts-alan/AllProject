using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCP.WebApi.Models
{
    public class FilterItem
    {
        public List<string> FilterFieldNames { get; set; }

        public string FilterValue { get; set; }

        public string FilterOperation { get; set; }
    }
}