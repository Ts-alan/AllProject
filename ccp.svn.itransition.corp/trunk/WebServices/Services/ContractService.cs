using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CCP.DAL;
using CCP.DAL.DataModels;
using CCP.DAL.Interfaces;
using CCP.WebApi.Extensions;
using System.Security.Principal;
using CCP.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace CCP.WebApi.Services
{
    public class ContractService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRepository<Contract> _contractRepository;
        private IRepository<ApproveStatusType> _approveStatusTypesRepository;
        private IRepository<ContractStatusType> _contractStatusTypesRepository;
        private readonly IRepository<ApproveStatus> _approveStatusRepository;
        private readonly IRepository<ApproverTier> _approverTierRepository;
        private readonly MessageService messageService;
        private readonly UserService userService;

        public ContractService()
        {
            _contractRepository = _unitOfWork.ContractRepository;
            _approveStatusTypesRepository = _unitOfWork.ApproveStatusTypeRepository;
            _contractStatusTypesRepository = _unitOfWork.ContractStatusTypeRepository;
            _approveStatusRepository = _unitOfWork.ApproveStatusRepository;
            _approverTierRepository = _unitOfWork.ApproverTierRepository;
            messageService = new MessageService();
            userService = new UserService();
        }

        public IEnumerable<Contract> Get()
        {
            return _contractRepository.Get(otherActions: a => a.Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver)).Include(c => c.ApproveStatuses.Select(s => s.ApproveStatusType)).AsNoTracking()).Where(c => c.ApproveStatuses.Count > 0).ToList();
        }

        public DataSourceResult GetContracts(DataSourceRequest model)
        {

            return _contractRepository
                .Get(otherActions: q => q
                    .Include(c => c.SalesPerson)
                    .Include(c => c.ContractStatusType)
                    .Include(c => c.ApproveStatuses)
                    .AsNoTracking(),
                    orderBy: q => q.OrderByDescending(c => c.ContractId))
                .GetDataSource(model);
        }

        public int Insert(Contract entity)
        {
            List<ContractStatusType> _contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Dr", _contractStatuses) == null)
            {
                return 2;
            }

            if (entity.StatusId == null || entity.StatusId == GetContractStatusByTag("Dr", _contractStatuses).ContractStatusId)
            {
                entity.StatusId = GetContractStatusByTag("Dr", _contractStatuses).ContractStatusId;
                _contractRepository.Insert(entity);
                _contractRepository.Save();
                var i = entity.ContractId;
                entity.CPRNumber = "";
                while (i < 10000)
                {
                    entity.CPRNumber = entity.CPRNumber + "0";
                    i *= 10;
                }
                entity.CPRNumber = entity.CPRNumber + entity.ContractId;
                _contractRepository.Update(entity);
                _contractRepository.Save();
                return 0;
            }
            return 1;
        }

        public int Update(Contract entityToUpdate, IPrincipal user)
        {
            List<ContractStatusType> contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Dr", contractStatuses) == null || GetContractStatusByTag("Ret", contractStatuses) == null)
            {
                return 2;
            }
            entityToUpdate.ContractStatusType = null;
            entityToUpdate.ApproveStatuses = null;

            if (entityToUpdate.StatusId != GetContractStatusByTag("Dr", contractStatuses).ContractStatusId && entityToUpdate.StatusId != GetContractStatusByTag("Ret", contractStatuses).ContractStatusId)
            {
                if (user.IsInRole("Admin"))
                {
                    entityToUpdate.StatusId = GetContractStatusByTag("Dr", contractStatuses).ContractStatusId;
                    var approveStatuses =
                        _approveStatusRepository.Get(s => s.ContractId == entityToUpdate.ContractId).ToList();
                    foreach (var approveStatus in approveStatuses)
                    {
                        _approveStatusRepository.Delete(approveStatus);
                    }
                    _approverTierRepository.Save();
                    entityToUpdate.ApproveStatuses = null;
                }
                else
                {
                    return 1;
                }


            }
            if (entityToUpdate.CPRNumber.Length < 5)
            {
                var i = entityToUpdate.ContractId;
                entityToUpdate.CPRNumber = "";
                while (i < 10000)
                {
                    entityToUpdate.CPRNumber = entityToUpdate.CPRNumber + "0";
                    i *= 10;
                }
                entityToUpdate.CPRNumber = entityToUpdate.CPRNumber + entityToUpdate.ContractId.ToString();
            }
            _contractRepository.Update(entityToUpdate);
            _contractRepository.Save();
            return 0;

        }


        public int Cancel(long entityIdToCancel, IPrincipal User)
        {
            List<ContractStatusType> _contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Can", _contractStatuses) == null)
            {
                return 2;
            }
            List<ApproveStatusType> _approveStatuses = GetApproveStatuses();
            if (GetApproveStatusByTag("Ass", _approveStatuses) == null ||
                GetApproveStatusByTag("Pen", _approveStatuses) == null ||
                GetApproveStatusByTag("Skip", _approveStatuses) == null ||
                GetApproveStatusByTag("Rej", _approveStatuses) == null)
            {
                return 2;
            }
            string userName = User.Identity.Name;
            var entityToCancel =
                   _contractRepository.Get(c => c.ContractId == entityIdToCancel,
                       otherActions:
                           a => a.Include(c => c.ApproveStatuses)
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Tier))
                               .AsNoTracking())
                       .FirstOrDefault();
            IRepository<ApproveStatus> approveStatusRepository = _unitOfWork.ApproveStatusRepository;
            if (entityToCancel == null)
            {
                return 3;
            }
            var oldContractStatusId = entityToCancel.StatusId.Value;
            //entityToCancel.StatusId = GetContractStatusByTag("Can", _contractStatuses).ContractStatusId;
            if (User.IsInRole("Admin") || User.IsInRole("Initiator"))
            {
                entityToCancel.StatusId = GetContractStatusByTag("Can", _contractStatuses).ContractStatusId;
                var statuses = entityToCancel.ApproveStatuses;
                entityToCancel.ApproveStatuses = null;
                _contractRepository.Update(entityToCancel);
                _contractRepository.Save();
                //var statuses = entityToCancel.ApproveStatuses;
                foreach (ApproveStatus status in statuses)
                {
                    if (status.StatusId == GetApproveStatusByTag("Ass", _approveStatuses).ApproverStatusId)
                    {
                        status.StatusId = GetApproveStatusByTag("Skip", _approveStatuses).ApproverStatusId;

                    }
                    else if (status.StatusId == GetApproveStatusByTag("Pen", _approveStatuses).ApproverStatusId)
                    {
                        status.StatusId = GetApproveStatusByTag("Skip", _approveStatuses).ApproverStatusId;
                    }
                    status.ApproveStatusType = null;
                    status.Contract = null;
                    approveStatusRepository.Update(status);
                    approveStatusRepository.Save();
                }
                entityToCancel.ApproveStatuses = null;
                _contractRepository.Update(entityToCancel);
                _contractRepository.Save();
                var actionAuthorId = userService.GetUser(userName).Id;
                messageService.SaveMessageForInitiator(actionAuthorId, entityToCancel.SalesPersonId.Value, oldContractStatusId, entityToCancel.StatusId.Value);
                return 0;
            }

            return 1;
        }

        public int Return(long entityIdToReturn, IPrincipal User)
        {
            List<ContractStatusType> _contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Ret", _contractStatuses) == null)
            {
                return 2;
            }
            List<ApproveStatusType> _approveStatuses = GetApproveStatuses();
            if (GetApproveStatusByTag("Ass", _approveStatuses) == null ||
                GetApproveStatusByTag("Pen", _approveStatuses) == null ||
                GetApproveStatusByTag("Skip", _approveStatuses) == null ||
                GetApproveStatusByTag("ForcR", _approveStatuses) == null ||
                GetApproveStatusByTag("Rej", _approveStatuses) == null)
            {
                return 2;
            }
            string userName = User.Identity.Name;
            var entityToReturn =
                   _contractRepository.Get(c => c.ContractId == entityIdToReturn,
                       otherActions:
                           a => a.Include(c => c.ApproveStatuses)
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Tier))
                               .AsNoTracking())
                       .FirstOrDefault();
            IRepository<ApproveStatus> approveStatusRepository = _unitOfWork.ApproveStatusRepository;
            if (entityToReturn == null)
            {
                return 3;
            }
            var oldContractStatusId = entityToReturn.StatusId.Value;
            if (User.IsInRole("Admin") || User.IsInRole("Approver"))
            {
                entityToReturn.StatusId = GetContractStatusByTag("Ret", _contractStatuses).ContractStatusId;
                var statuses = entityToReturn.ApproveStatuses;
                entityToReturn.ApproveStatuses = null;
                _contractRepository.Update(entityToReturn);
                _contractRepository.Save();

                foreach (ApproveStatus status in statuses)
                {
                    if (status.StatusId == GetApproveStatusByTag("Ass", _approveStatuses).ApproverStatusId)
                    {
                        status.StatusId = GetApproveStatusByTag("Skip", _approveStatuses).ApproverStatusId;

                    }
                    else if (status.StatusId == GetApproveStatusByTag("Pen", _approveStatuses).ApproverStatusId)
                    {
                        status.StatusId = GetApproveStatusByTag("Ret", _approveStatuses).ApproverStatusId;
                    }
                    status.ApproveStatusType = null;
                    status.Contract = null;
                    approveStatusRepository.Update(status);
                    approveStatusRepository.Save();
                }
                entityToReturn.ApproveStatuses = null;
                _contractRepository.Update(entityToReturn);
                _contractRepository.Save();
                var actionAuthorId = userService.GetUser(userName).Id;
                messageService.SaveMessageForInitiator(actionAuthorId, entityToReturn.SalesPersonId.Value, oldContractStatusId, entityToReturn.StatusId.Value);
                return 0;
            }

            return 1;
        }

        public int Reject(long entityIdToReject, IPrincipal User)
        {
            List<ContractStatusType> _contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Rej", _contractStatuses) == null)
            {
                return 2;
            }
            List<ApproveStatusType> _approveStatuses = GetApproveStatuses();
            if (GetApproveStatusByTag("Ass", _approveStatuses) == null ||
                GetApproveStatusByTag("Pen", _approveStatuses) == null ||
                GetApproveStatusByTag("Skip", _approveStatuses) == null ||
                GetApproveStatusByTag("ForcR", _approveStatuses) == null ||
                GetApproveStatusByTag("Rej", _approveStatuses) == null)
            {
                return 2;
            }
            string userName = User.Identity.Name;
            var entityToReject =
                   _contractRepository.Get(c => c.ContractId == entityIdToReject,
                       otherActions:
                           a => a.Include(c => c.ApproveStatuses)
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Tier))
                               .AsNoTracking())
                       .FirstOrDefault();
            IRepository<ApproveStatus> approveStatusRepository = _unitOfWork.ApproveStatusRepository;
            if (entityToReject == null)
            {
                return 3;
            }
            var oldContractStatusId = entityToReject.StatusId.Value;
            entityToReject.StatusId = GetContractStatusByTag("Rej", _contractStatuses).ContractStatusId;
            if (User.IsInRole("Admin") || User.IsInRole("Approver"))
            {
                var statuses = entityToReject.ApproveStatuses;
                foreach (ApproveStatus status in statuses)
                {
                    if (status.StatusId == GetApproveStatusByTag("Ass", _approveStatuses).ApproverStatusId)
                    {
                        status.StatusId = GetApproveStatusByTag("Skip", _approveStatuses).ApproverStatusId;

                    }
                    else if (status.StatusId == GetApproveStatusByTag("Pen", _approveStatuses).ApproverStatusId)
                    {
                        if (User.IsInRole("Admin"))
                        {
                            status.StatusId = GetApproveStatusByTag("ForcR", _approveStatuses).ApproverStatusId;

                        }
                        else if (User.IsInRole("Approver"))
                        {
                            status.StatusId = GetApproveStatusByTag("Rej", _approveStatuses).ApproverStatusId;
                        }
                    }
                    status.ApproveStatusType = null;
                    status.Contract = null;
                    approveStatusRepository.Update(status);
                    approveStatusRepository.Save();
                }
                entityToReject.ApproveStatuses = null;
                _contractRepository.Update(entityToReject);
                _contractRepository.Save();
                var actionAuthorId = userService.GetUser(userName).Id;
                messageService.SaveMessageForInitiator(actionAuthorId,entityToReject.SalesPersonId.Value,oldContractStatusId,entityToReject.StatusId.Value);
                return 0;
            }

            return 1;
        }

        public int InsertAndSubmit(Contract entity)
        {
            List<ContractStatusType> contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("App", contractStatuses) == null || GetContractStatusByTag("Pen", contractStatuses) == null)
            {
                return 2;
            }
            List<ApproveStatusType> approveStatuses = GetApproveStatuses();
            if (GetApproveStatusByTag("Ass", approveStatuses) == null || GetApproveStatusByTag("Pen", approveStatuses) == null)
            {
                return 2;
            }

            SetApproveStatuses(entity, contractStatuses, approveStatuses);

            _contractRepository.Insert(entity);
            _contractRepository.Save();
            entity.CPRNumber = entity.ContractId.ToString();
            _contractRepository.Update(entity);
            _contractRepository.Save();
            if (entity.StatusId.Value == GetContractStatusByTag("App", contractStatuses).ContractStatusId)
            {
                messageService.SaveMessageForInitiator(entity.SalesPersonId.Value, GetContractStatusByTag("Dr", contractStatuses).ContractStatusId, entity.StatusId.Value);
                
            }
            else
            {
                messageService.SaveMessageForInitiator(entity.SalesPersonId.Value, GetContractStatusByTag("Dr", contractStatuses).ContractStatusId, entity.StatusId.Value);
                messageService.SaveMessageForApprover(entity.SalesPersonId.Value, entity.ApproveStatuses.First().ApproverTier.ApproverId);
            }
            return 0;
        }

        public int UpdateAndSubmit(Contract entityToUpdate, IPrincipal user)
        {
            List<ContractStatusType> contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Dr", contractStatuses) == null ||
                GetContractStatusByTag("Pen", contractStatuses) == null ||
                GetContractStatusByTag("App", contractStatuses) == null ||
                GetContractStatusByTag("Ret", contractStatuses) == null)
            {
                return 2;
            }
            List<ApproveStatusType> approveStatuses = GetApproveStatuses();
            if (GetApproveStatusByTag("Ass", approveStatuses) == null ||
                GetApproveStatusByTag("Pen", approveStatuses) == null)
            {
                return 2;
            }
            IRepository<ApproveStatus> approveStatusRepository = _unitOfWork.ApproveStatusRepository;
            if (entityToUpdate.StatusId != GetContractStatusByTag("Dr", contractStatuses).ContractStatusId)
            {
                if (user.IsInRole("Admin") || entityToUpdate.SalesPerson.Email == user.Identity.Name)
                {
                    var approveStats =
                    _approveStatusRepository.Get(s => s.ContractId == entityToUpdate.ContractId).ToList();
                    foreach (var approveStatus in approveStats)
                    {
                        _approveStatusRepository.Delete(approveStatus);
                    }
                    _approverTierRepository.Save();
                    entityToUpdate.ApproveStatuses = null;
                }
                else
                {
                    return 1;
                }
            }


            IRepository<Tier> tierRepository = _unitOfWork.TiersRepository;
            var tiers = tierRepository.Get();
            var noApproval = tiers.Min(t => t.TDGAMinValue);
            if (entityToUpdate.TDGA < noApproval) //No approvers requred
            {
                entityToUpdate.StatusId = GetContractStatusByTag("App", contractStatuses).ContractStatusId;
            }
            else    //Approvers required
            {
                entityToUpdate.StatusId = GetContractStatusByTag("Pen", contractStatuses).ContractStatusId;
                IRepository<ApproverTier> approverTiersRepository = _unitOfWork.ApproverTierRepository;
                var approvers = approverTiersRepository.Get(t => t.SalesPersonId == entityToUpdate.SalesPersonId);
                foreach (var tier in tiers)
                {
                    if (entityToUpdate.TDGA >= tier.TDGAMinValue)
                    {
                        var approveStatus = new ApproveStatus
                        {
                            StatusId = GetApproveStatusByTag("Ass", approveStatuses).ApproverStatusId,
                            ApproverTier = approvers.Single(t => t.TierId == tier.TierId),
                            ContractId = entityToUpdate.ContractId,
                            Contract = null,
                            ApproverTierId = 0
                        };

                        if (tier.TierId == 1)   //First Approver
                        {
                            approveStatus.StatusId = GetApproveStatusByTag("Pen", approveStatuses).ApproverStatusId;
                        }

                        approveStatusRepository.Insert(approveStatus);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            entityToUpdate.ContractStatusType = null;
            entityToUpdate.SalesPerson = null;
            _contractRepository.Update(entityToUpdate);
            _contractRepository.Save();
            messageService.SaveMessageForApprover( entityToUpdate.SalesPersonId.Value, entityToUpdate.ApproveStatuses.First().ApproverTier.ApproverId);
            return 0;
        }

        private void SetApproveStatuses(Contract entity, List<ContractStatusType> contractStatuses, List<ApproveStatusType> approveStatuses)
        {
            IRepository<Tier> tierRepository = _unitOfWork.TiersRepository;
            var tiers = tierRepository.Get();
            var noApproval = tiers.Min(t => t.TDGAMinValue);
            if (entity.TDGA < noApproval) //No approvers requred
            {
                entity.StatusId = GetContractStatusByTag("App", contractStatuses).ContractStatusId;
            }
            else    //Approvers required
            {
                entity.StatusId = GetContractStatusByTag("Pen", contractStatuses).ContractStatusId;
                IRepository<ApproverTier> approverTiersRepository = _unitOfWork.ApproverTierRepository;
                var approvers = approverTiersRepository.Get(t => t.SalesPersonId == entity.SalesPersonId);
                foreach (var tier in tiers)
                {
                    if (entity.TDGA >= tier.TDGAMinValue)
                    {
                        entity.ApproveStatuses.Add(new ApproveStatus
                        {
                            StatusId = GetApproveStatusByTag("Ass", approveStatuses).ApproverStatusId,
                            ApproverTier = approvers.SingleOrDefault(t => t.TierId == tier.TierId),
                            Contract = entity
                        });
                    }
                }

                entity.ApproveStatuses.Single(a => a.ApproverTier.TierId == tiers.Min(t => t.TierId)).StatusId = GetApproveStatusByTag("Pen", approveStatuses).ApproverStatusId; //ApproveStatus - Pending
            }
        }

        public Contract GetById(long id)
        {
            var contract = _contractRepository.Get
                (
                c => c.ContractId == id,
                    otherActions:
                    c => c.Include(a => a.SalesPerson)
                        .Include(a => a.ApproveStatuses.Select(s => s.ApproveStatusType))
                        .Include(a => a.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                        .Include(a => a.ContractStatusType)
                        )
                        .FirstOrDefault();
            if (contract != null)
            {
                //contract.ApproveStatuses = contract.ApproveStatuses.Where(s => s.ApproveStatusType.ApproverStatusTag == "Pen").ToList();
            }

            return contract;
        }

        private ApproveStatusType GetApproveStatusByTag(string tag, List<ApproveStatusType> _approveStatuses)
        {
            if (_approveStatuses != null)
            {
                foreach (ApproveStatusType status in _approveStatuses)
                {
                    if (status.ApproverStatusTag == tag)
                    {
                        return status;
                    }
                }
            }

            return null;
        }

        private List<ApproveStatusType> GetApproveStatuses()
        {
            if (_approveStatusTypesRepository.Get() != null)
            {
                return _approveStatusTypesRepository.Get().ToList();
            }
            else
            {
                return null;
            }
        }

        private ContractStatusType GetContractStatusByTag(string tag, List<ContractStatusType> _contractStatuses)
        {
            if (_contractStatuses != null)
            {
                foreach (ContractStatusType status in _contractStatuses)
                {
                    if (status.ContractStatusTag == tag)
                    {
                        return status;
                    }
                }
            }

            return null;
        }

        private List<ContractStatusType> GetContractStatuses()
        {
            if (_contractStatusTypesRepository.Get() != null)
            {
                return _contractStatusTypesRepository.Get().ToList();
            }
            else
            {
                return null;
            }
        }

        public int Approve(long id, IPrincipal User)
        {
            List<ContractStatusType> _contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Pen", _contractStatuses) == null || GetContractStatusByTag("App", _contractStatuses) == null)
            {
                return 2;
            }
            List<ApproveStatusType> _approveStatuses = GetApproveStatuses();
            if (GetApproveStatusByTag("App", _approveStatuses) == null || GetApproveStatusByTag("Pen", _approveStatuses) == null || GetApproveStatusByTag("ForcA", _approveStatuses) == null)
            {
                return 2;
            }
            string userEmail = User.Identity.Name;
            var contract =
                   _contractRepository.Get(c => c.ContractId == id,
                       otherActions:
                           a => a.Include(c => c.ApproveStatuses)
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Tier))
                               .AsNoTracking())
                       .Single();
            if (contract.StatusId != GetContractStatusByTag("Pen", _contractStatuses).ContractStatusId)
            {
                return 1; //Approval forbiden
            }
            var oldContractStatusId = contract.StatusId.Value;
            IRepository<ApproveStatus> approveStatusRepository = _unitOfWork.ApproveStatusRepository;
            var pendingApproveStatusId = GetApproveStatusByTag("Pen", _approveStatuses).ApproverStatusId;
            var statuses = contract.ApproveStatuses;
            var pendingApproveStatus = statuses.SingleOrDefault(s => s.StatusId == pendingApproveStatusId);
            if (pendingApproveStatus == null || pendingApproveStatus.ApproverTier == null || pendingApproveStatus.ApproverTier.Approver == null || (!pendingApproveStatus.ApproverTier.Approver.Email.Equals(userEmail) && !User.IsInRole("Admin") && !User.IsInRole("Approver")))
            {
                return 1;   //Approval forbiden
            }
            else if (!pendingApproveStatus.ApproverTier.Approver.Email.Equals(userEmail) && User.IsInRole("Admin"))
            {
                pendingApproveStatus.StatusId = GetApproveStatusByTag("ForcA", _approveStatuses).ApproverStatusId;
            }
            else
            {
                pendingApproveStatus.StatusId = GetApproveStatusByTag("App", _approveStatuses).ApproverStatusId;
            }
            pendingApproveStatus.ApproveStatusType = null;
            pendingApproveStatus.Contract = null;
            approveStatusRepository.Update(pendingApproveStatus);
            if (statuses.Any(s => s.ApproverTier == null))
            {
                return 1;
            }

            if (statuses.Any(s => s.ApproverTier.Tier == null))
            {
                return 1;
            }

            var currentLevel = pendingApproveStatus.ApproverTier.Tier.ApproverLevel;
            var nextLevelStatus = statuses.SingleOrDefault(s => s.ApproverTier.Tier.ApproverLevel == currentLevel + 1);
            if (nextLevelStatus == null)
            {
                contract.StatusId = GetContractStatusByTag("App", _contractStatuses).ContractStatusId;
                contract.ContractStatusType = null;
            }
            else
            {
                nextLevelStatus.StatusId = pendingApproveStatusId;
                nextLevelStatus.ApproveStatusType = null;
                approveStatusRepository.Update(nextLevelStatus);
            }

            contract.ApproveStatuses = null;
            _contractRepository.Update(contract);
            _contractRepository.Save();
            messageService.SaveMessageForInitiator(pendingApproveStatus.ApproverTier.ApproverId,contract.SalesPersonId.Value,oldContractStatusId,contract.StatusId.Value );
            return 0;
        }

        public List<ApproveStatus> GetApprovers(long id)
        {
            if (id > 0)
            {
                Contract contract = GetById(id);
                List<ApproveStatus> result = new List<ApproveStatus>();
                if (contract.ApproveStatuses.Count != 0)
                {
                    result = contract.ApproveStatuses.ToList();
                }
                if (result.Count == 3)
                {
                    return result;
                }
                IRepository<ApproverTier> approverTiersRepository = _unitOfWork.ApproverTierRepository;
                var approvers = approverTiersRepository
                    .Get(t => (t.SalesPersonId == contract.SalesPersonId && t.TierId > result.Count))
                    .Include(t => t.Approver)
                    .ToList();
                List<ApproveStatusType> _approveStatuses = GetApproveStatuses();
                foreach (ApproverTier approver in approvers)
                {
                    ApproveStatus status = new ApproveStatus();
                    status.ApproverTier = approver;
                    status.ApproveStatusType = GetApproveStatusByTag("NotR", _approveStatuses);
                    result.Add(status);
                }
                return result;
            }
            return new List<ApproveStatus>();
        }

        public List<ApproveStatus> GetInitApprovers(long id)
        {
            if (id > 0)
            {
                List<ApproveStatus> result = new List<ApproveStatus>();
                IRepository<ApproverTier> approverTiersRepository = _unitOfWork.ApproverTierRepository;
                var approvers = approverTiersRepository
                    .Get(t => (t.SalesPersonId == id && t.TierId > result.Count))
                    .Include(t => t.Approver)
                    .AsNoTracking()
                    .ToList();
                List<ApproveStatusType> _approveStatuses = GetApproveStatuses();
                foreach (ApproverTier approver in approvers)
                {
                    ApproveStatus status = new ApproveStatus();
                    status.ApproverTier = approver;
                    status.ApproveStatusType = GetApproveStatusByTag("NotR", _approveStatuses);
                    result.Add(status);
                }
                return result;
            }
            return new List<ApproveStatus>();
        }

        public int AdminApprove(long id, string userEmail)
        {
            List<ContractStatusType> _contractStatuses = GetContractStatuses();
            if (GetContractStatusByTag("Pen", _contractStatuses) == null || GetContractStatusByTag("App", _contractStatuses) == null)
            {
                return 2;
            }
            List<ApproveStatusType> _approveStatuses = GetApproveStatuses();
            if (GetApproveStatusByTag("App", _approveStatuses) == null)
            {
                return 2;
            }
            var contract =
                   _contractRepository.Get(c => c.ContractId == id,
                       otherActions:
                           a => a.Include(c => c.ApproveStatuses)
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                               .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Tier))
                               .AsNoTracking())
                       .Single();
            if (contract.StatusId != GetContractStatusByTag("Pen", _contractStatuses).ContractStatusId)
            {
                return 1; //Approval forbiden
            }

            IRepository<ApproveStatus> approveStatusRepository = _unitOfWork.ApproveStatusRepository;
            var statuses = contract.ApproveStatuses;
            foreach (var approveStatus in statuses)
            {
                approveStatus.StatusId = GetApproveStatusByTag("App", _approveStatuses).ApproverStatusId;
                approveStatus.ApproveStatusType = null;
                approveStatus.Contract = null;
                approveStatusRepository.Update(approveStatus);
            }

            contract.StatusId = GetContractStatusByTag("App", _contractStatuses).ContractStatusId;
            contract.ContractStatusType = null;
            contract.ApproveStatuses = null;
            _contractRepository.Update(contract);
            _contractRepository.Save();

            return 0;
        }

        public IEnumerable<Contract> GetOwnedContracts(MyCPRsDataModel model)
        {
            var contracts = _contractRepository.Get(c => c.SalesPerson.Email == model.Email,
                otherActions:
                    a =>
                        a.Include(c => c.ContractStatusType)
                            .Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                            .Include(c => c.ApproveStatuses.Select(s => s.ApproveStatusType))
                            .Include(c => c.SalesPerson).AsNoTracking());
            if (model.StatusId > 0)
            {
                contracts = contracts.Where(c => c.StatusId == model.StatusId);
            }
            return contracts;
        }

        public IEnumerable<Contract> GetPendingContracts(IPrincipal user)
        {
            var contracts = _contractRepository.Get(otherActions:
                a =>
                    a.Include(c => c.ApproveStatuses.Select(s => s.ApproverTier.Approver))
                        .Include(c => c.ApproveStatuses.Select(s => s.ApproveStatusType))
                        .AsNoTracking())
                .Where(
                    c =>
                        c.ApproveStatuses
                            .Any(
                                cc => cc.ApproveStatusType.ApproverStatusTag == "Pen" && cc.ApproverTier.Approver.Email == user.Identity.Name))
                .AsNoTracking();

            //contracts = contracts;
            return contracts;

        }
    }
}