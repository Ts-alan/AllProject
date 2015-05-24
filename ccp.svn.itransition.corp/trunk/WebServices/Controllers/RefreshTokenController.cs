using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CCP.WebApi.Resources;
using CCP.WebApi.Services;

namespace CCP.WebApi.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {

        private UserService service = null;

        public RefreshTokensController()
        {
            service = new UserService();
        }

        [Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(service.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await service.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest(Res.TokenIdDoesNotExist);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                service.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
