using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CCP.DAL.DataModels;
using CCP.DAL.Helpers;
using Microsoft.AspNet.Identity;

namespace CCP.WebApi.Controllers
{
    public class TestController : ApiController
    {
        public async Task<HttpResponseMessage> Get(string login, string password = "000000")
        {
            var r = new CCPUserStore(new CCPEntities());
            var u = await r.FindByEmailAsync(login);
            if (u == null)
            {
                u = new User() { Email = login };
                await r.CreateAsync(u);
            }
            var p = "qwaszx@1";
            await r.SetPasswordHashAsync(u, new PasswordHasher().HashPassword(password));
            await r.UpdateAsync(u);
            r.Dispose();
            return null;
        }
    }
}
