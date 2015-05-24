using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCP.DAL;
using CCP.DAL.DataModels;
using CCP.DAL.Helpers;
using CCP.DAL.Interfaces;
using CCP.WebApi.Extensions;
using CCP.WebApi.Helpers;
using CCP.WebApi.Models;
using CCP.WebApi.Services;

namespace CCP.WebApi.Controllers
{
    public class ApproverStatusesController : ApiController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IRepository<ApproveStatus> _approveStatusRepository;
        private readonly ApproveStatusService _approveStatusService = new ApproveStatusService();

        [HttpGet]
        [Route("api/ApproveStatus")]
        public HttpResponseMessage Get(long data)
        {
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, _approveStatusService.GetByContractId(data));
        }

        [HttpGet]
        [Route("api/ContractsOfApprover")]
        public HttpResponseMessage GetListContractsOfApprover(long data)
        {
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, _approveStatusService.GetByApproverId(data));
        }
    }
}
