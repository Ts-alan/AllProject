using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCP.WebApi.Models
{
    public class ErrorsModel
    {
        public string LogicError { get; set; }

        public Dictionary<string, IEnumerable<string>> DataErrors;
    }
}