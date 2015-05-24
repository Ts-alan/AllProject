using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CCP.DAL.Entities
{
    public partial class User : IUser<long> 
    {
        public long Id
        {
            get { return UserId; }
        }

        public string UserName
        {
            get { return Email; }
            set { Email = value; }
        }
    }
}
