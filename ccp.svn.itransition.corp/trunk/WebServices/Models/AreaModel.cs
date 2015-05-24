using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCP.WebApi.Models
{
    public class AreaModel
    {
        public string AreaName { get; set; }

        public List<string> Roles { get; set; } 
    }
}