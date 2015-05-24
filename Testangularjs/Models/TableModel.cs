using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Testangularjs.Models
{
    public class TableModel
    {
            
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTimeOffset DateGreate { get; set; }
            public DateTimeOffset? DateChange { get; set; }
            public string Description { get; set; }
        
    }
}