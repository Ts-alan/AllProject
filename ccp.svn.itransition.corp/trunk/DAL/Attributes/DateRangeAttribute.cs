using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCP.DAL.Attributes
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute()
            : base(typeof(DateTime),
                    DateTime.Now.AddYears(-6).ToShortDateString(),
                    DateTime.Now.AddYears(+6).ToShortDateString())
        { }
    }
}
