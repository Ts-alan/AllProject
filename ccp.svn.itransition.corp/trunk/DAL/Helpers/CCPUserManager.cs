using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCP.DAL.DataModels;
using Microsoft.AspNet.Identity;

namespace CCP.DAL.Helpers
{
    public class CCPUserManager : UserManager<User, long>
    {
        public CCPUserManager()
            : base(new CCPUserStore())
        {
        }
    }
}
