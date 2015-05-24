using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using CCP.DAL.DataModels;
using CCP.WebApi.Helpers;
using CCP.WebApi.Models;
using CCP.WebApi.Resources;
using CCP.WebApi.Services;
using Microsoft.AspNet.Identity;

namespace CCP.WebApi.Controllers
{
    [Authorize]
    public class ContractsController : ApiController
    {
        private readonly ContractService _contractService = new ContractService();
        private readonly ApproveStatusService _approveStatusService = new ApproveStatusService();

        public HttpResponseMessage Post(DataSourceRequest model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dataSourceResult = _contractService.GetContracts(model);
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, dataSourceResult);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [Authorize(Roles = "Admin,Initiator")]
        [Route("api/save/contract")]
        public HttpResponseMessage Save(Contract model)
        {
            var result = 0;
            //if (model != null && ModelState.IsValid)
            //{
                if (model.ContractId == 0)
                {
                    result = _contractService.Insert(model);
                }
                else
                {
                    result = _contractService.Update(model, User);
                }

                if (result == 1)
                {
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest,ErrorsModelBuilder.GetErrorsModel(Res.ContractEdition));
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            //}
            //////return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, Res.InvalidModel);
            //return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(ModelState));
        }

        [Authorize(Roles = "Admin,Initiator")]
        [Route("api/submit/contract")]
        public HttpResponseMessage Submit(Contract model)
        {
            var result = 0;
            if (model != null && ModelState.IsValid)
            {
                if (model.ContractId == 0)
                {
                    result = _contractService.InsertAndSubmit(model);
                }
                else
                {
                    result = _contractService.UpdateAndSubmit(model, User);
                }
                if (result == 1)
                {
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(Res.ContractEdition));
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(ModelState));
        }

        [Authorize(Roles = "Initiator,Admin")]
        [HttpGet]
        [Route("api/cancel/contract")]
        public HttpResponseMessage Cancel(long data)
        {
            var result = 0;
            if (ModelState.IsValid)
            {
                if (data > 0)
                {
                    if (User.IsInRole("Initiator") || User.IsInRole("Admin"))
                    {
                        result = _contractService.Cancel(data, User);
                    }
                }

                if (result == 1)
                {
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(Res.ContractEdition));
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, Res.InvalidModel);
        }

        [Authorize(Roles = "Approver,Admin")]
        [HttpGet]
        [Route("api/return/contract")]
        public HttpResponseMessage Return(long data)
        {
            var result = 0;
            if (data > 0)
            {
                result = _contractService.Return(data, User);
            }
            if (result == 1)
            {
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(Res.ContractEdition));
            }
            if (result == 3)
            {
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(Res.NonexistentContract));
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Approver,Admin")]
        [HttpGet]
        [Route("api/Contract/Reject")]
        public HttpResponseMessage Reject(long data)
        {
            var result = 0;
            if( data > 0)
            {
                result = _contractService.Reject(data, User);
            }
            if (result == 1)
            {
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(Res.ContractEdition));
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/Contract")]
        public HttpResponseMessage Get(long data)
        {
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, _contractService.GetById(data));
        }

        [HttpGet]
        [Route("api/Contract/GetApprovers")]
        public HttpResponseMessage GetApprovers(long data)
        {
                if (data > 0 && ModelState.IsValid)
                {
                    var dataSourceResult = _contractService.GetApprovers(data);
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, dataSourceResult);
        }
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, Res.InvalidModel);
        }

        [HttpGet]
        [Route("api/Contract/GetInitApprovers")]
        public HttpResponseMessage GetInitApprovers(long data)
        {
            if (data > 0 && ModelState.IsValid)
            {
                var dataSourceResult = _contractService.GetInitApprovers(data);
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, dataSourceResult);
            }
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, Res.InvalidModel);
        }



        [Authorize(Roles="Approver,Admin")]
        [HttpGet]
        [Route("api/PendingContracts")]
        public HttpResponseMessage GetPendingContracts()
        {

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, _contractService.GetPendingContracts(User));
        }

        [Authorize(Roles = "Admin,Approver")]        
        [HttpGet]
        [Route("api/Contract/Approve")]
        public HttpResponseMessage ApproveContract(long data)
        {
            int result = 0;
            if (data > 0)
            {
                result = _contractService.Approve(data, User);
            }
            if (result == 1)
            {
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(Res.ContractEdition));
            }
            if (result == 3)
            {
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(Res.NonexistentContract));
            }
            return new HttpResponseMessage(HttpStatusCode.OK);    
        }


        [Authorize(Roles = "Initiator")]        
        [HttpPost]
        [Route("api/MyContracts")]
        public HttpResponseMessage MyContracts(MyCPRsDataModel model) //email
        {

            if (!String.IsNullOrEmpty(model.Email))
            {
                var contracts = _contractService.GetOwnedContracts(model);
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, contracts);
            }
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, new Contract());
        }

    }
}
