using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCP.DAL;
using CCP.DAL.DataModels;
using CCP.DAL.Interfaces;
using CCP.WebApi.Extensions;
using CCP.WebApi.Helpers;
using CCP.WebApi.Models;

namespace CCP.WebApi.Controllers
{
    [Authorize]
    public class ListController : ApiController
    {
        
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private IRepository<User> _usersRepository;
        private IRepository<Customer> _customersRepository;
        private IRepository<EndUser> _endUsersRepository;
        private IRepository<ContractStatusType> _statusesRepository;
        private IRepository<Role> _rolesRepository;

        //private IRepository<SalesPerson> _salesPersonRepository;
        //[HttpGet]
        //[Route("api/list/users")]
        //public HttpResponseMessage GetListUsers(string data, int cnt = 10)
        //{
        //    _usersRepository = _unitOfWork.UserRepository;
        //    _salesPersonRepository = _unitOfWork.SalesPersonRepository;
        //    var users = _usersRepository.Get().ToList();
        //    var salesPersons = _salesPersonRepository.Get();
        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        users =
        //            users.Where(
        //                u =>
        //                    ((u.FirstName + u.LastName).ToUpper().Contains(data.ToUpper()) ||
        //                     u.UserId.ToString().Contains(data)) && !(salesPersons.Any(s => (s.UserId == u.UserId))))
        //                .Take(cnt)
        //                .ToList();
        //    }
        //    else
        //    {
        //        users = users.Where(u => !(salesPersons.Any(s => (s.UserId == u.UserId)))).Take(cnt).ToList();
        //    }

        //    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, users);
        //}

        [HttpGet]
        [Route("api/list/customers")]
        public HttpResponseMessage GetListCustomers(string data, int cnt = 10)
        {
            _customersRepository = _unitOfWork.CustomerRepository;
            var customers = _customersRepository.Get().ToList();
            if (!string.IsNullOrEmpty(data))
            {
                customers =
                    customers.Where(
                        c =>
                            (c.CustomerName).ToUpper().Contains(data.ToUpper())).OrderBy(c=>c.CustomerName).ToList();
            }
            else
            {
                customers = customers.OrderBy(c => c.CustomerName).Take(cnt).ToList();
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, customers);
        }

        [HttpGet]
        [Route("api/list/salespersons")]
        public HttpResponseMessage GetListSalesPersons(string data, int cnt = 10)
        {
            
            _usersRepository = _unitOfWork.UserRepository;
            var users = _usersRepository.Get(otherActions: u => u.Include(a => a.Approvers)).Where(u=>u.Approvers.Any());
            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace(" ", "");
                users =
                    users.Where(u => (u.FirstName + u.LastName).ToUpper().Contains(data.ToUpper())).OrderBy(s=>s.FirstName); //.ToList();
            }
            else
            {
                users = users.OrderBy(s => s.FirstName).Take(cnt); //.ToList();
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, users);
        }

        [HttpGet]
        [Route("api/list/endUsers")]
        public HttpResponseMessage GetListEndUsers(string data, int cnt = 10)
        {
            _endUsersRepository = _unitOfWork.EndUsersRepository;
            var endUsers = _endUsersRepository.Get().ToList();
            if (!string.IsNullOrEmpty(data))
            {
                endUsers =
                    endUsers.Where(
                        c =>
                            (c.EndUserName).ToUpper().Contains(data.ToUpper())).OrderBy(u=>u.EndUserName).ToList();
            }
            else
            {
                endUsers = endUsers.OrderBy(u=>u.EndUserName).Take(cnt).ToList();
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, endUsers);
        }

        [HttpGet]
        [Route("api/list/statuses")]
        public HttpResponseMessage GetListStatuses(string data, int cnt = 10)
        {
            _statusesRepository = _unitOfWork.StatusesRepository;
            var statuses = _statusesRepository
                .Get(orderBy: q => q.OrderBy(c => c.ContractStatusName))
                .ToList();
            if (!string.IsNullOrEmpty(data))
            {
                statuses =
                    statuses.Where(
                        c =>
                            (c.ContractStatusName).ToUpper().Contains(data.ToUpper())).OrderBy(s=>s.ContractStatusName).ToList();
            }
            else
            {
                statuses = statuses.OrderBy(s=>s.ContractStatusName).Take(cnt).OrderBy(s=>s.ContractStatusName).ToList();
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, statuses);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/list/roles")]
        public HttpResponseMessage GetListRoles(string data, int cnt = 10)
        {
            _rolesRepository = _unitOfWork.RolesRepository;
            var roles = _rolesRepository.Get().ToList();
            if (!string.IsNullOrEmpty(data))
            {
                roles =
                    roles.Where(
                        c =>
                            (c.RoleName).ToUpper().Contains(data.ToUpper())).OrderBy(r=>r.RoleName).ToList();
            }
            else
            {
                roles = roles.OrderBy(r => r.RoleName).Take(cnt).ToList();
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/list/approvers")]
        public HttpResponseMessage GetListApprovers(ApproversListDataModel data, int cnt = 10)
        {
            var term = data.Term;
            long[] id = data.ApproversIds;
            _usersRepository = _unitOfWork.UserRepository;
            var users = _usersRepository.Get(otherActions: a => a.Include(u => u.Role)).ToList();
            if (!string.IsNullOrEmpty(term))
            {
                users =
                    users.Where(
                        u =>
                            ((u.Role.RoleId == 2) && (!id.Any(i => i == u.UserId)) && (data.UserId != u.UserId) &&
                            (u.FirstName + u.LastName).ToUpper().Contains(term.ToUpper()))).OrderBy(a=>a.FirstName)
                        .Take(cnt)
                        .ToList();
            }
            else
            {
                users = users.Where(u => (u.Role.RoleId == 2) && (!id.Any(i => i == u.UserId)) && (data.UserId != u.UserId)).OrderBy(a => a.FirstName).Take(cnt).ToList();
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, users);
        }

        [Authorize(Roles = "Initiator,Admin")]
        [HttpGet]
        [Route("api/list/initiators")]
        public HttpResponseMessage GetListInitiators(string data, int cnt = 10)
        {
            _usersRepository = _unitOfWork.UserRepository;
            var users =
                _usersRepository.Get(otherActions: u => u.Include(a => a.Approvers)).Where(u => u.Approvers.Any());
            if (!string.IsNullOrEmpty(data))
            {
                users =
                    users.Where(u => (u.FirstName + u.LastName).ToUpper().Contains(data)).OrderBy(i=>i.FirstName)
                        .Take(cnt);
            }
            else
            {
                users = users.OrderBy(i => i.FirstName).Take(cnt);
            }

            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, users);
        }
    }
}
