using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCP.WebApi.Models
{
    public class DataSourceRequest
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public IList<SortItem> SortItems { get; set; }

        public IList<FilterItem> FilterItems { get; set; }
    }
}