using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Verst.Models;

namespace MvcApplication.Models
{
    public class BatchNear: Batch
    {
        public BatchNear() : base() { }
        public BatchNear(Crystal.mparttRow ba) : base(ba) { }
        public int distance { get; set; }
        public string nop { get; set; }
    }
}