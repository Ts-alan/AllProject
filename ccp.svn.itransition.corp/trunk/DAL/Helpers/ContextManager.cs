using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCP.DAL.DataModels;

namespace CCP.DAL.Helpers
{
    public class ContextManager
    {
        internal static CCPEntities GetContext()
        {
            return new CCPEntities();
        }
    }
}
