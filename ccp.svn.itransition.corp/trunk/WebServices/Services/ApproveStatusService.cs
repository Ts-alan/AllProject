using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.ModelBinding;
using CCP.DAL;
using CCP.DAL.DataModels;
using CCP.DAL.Helpers;
using CCP.DAL.Interfaces;
using CCP.WebApi.Extensions;
using CCP.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace CCP.WebApi.Services
{
    public partial class ApproveStatusService 
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRepository<ApproveStatus> _approveStatusRepository;

        public ApproveStatusService()
        {
            _approveStatusRepository = _unitOfWork.ApproveStatusRepository;
        }

        private ApproveStatusType GetApproveStatusByTag(string tag)
        {
            IRepository<ApproveStatusType> statusTypes = _unitOfWork.ApproveStatusTypeRepository;
            return statusTypes.Get().Single(s => s.ApproverStatusTag == tag);
        }

        public ApproveStatus GetByContractId(long id)
        {
            if (id > 0)
            {
                return _approveStatusRepository
                    .Get(u => (u.ContractId == id && u.StatusId == 2))
                    .Include(u => u.ApproverTier.Approver).Include(u=> u.ApproveStatusType)
                    .SingleOrDefault();
            }

            return new ApproveStatus();
        }

        public void changeStatusOfApproversByRejected(long id)
        {
            List<ApproveStatus> approveStatuses = _approveStatusRepository
                    .Get(u => (u.ContractId == id))
                    .ToList();
            foreach (ApproveStatus status in approveStatuses) 
            {
                if(status.StatusId == 2) 
                {
                    status.StatusId = 4;
                    _approveStatusRepository.Update(status);
                }
                else if (status.StatusId == 1) 
                {
                    status.StatusId = 7;
                    _approveStatusRepository.Update(status);
                }

                _approveStatusRepository.Save();
            }
        }

        public void changeStatusOfApproversByRejectedAsAdmin(long id)
        {
            List<ApproveStatus> approveStatusRepository = _approveStatusRepository
                    .Get(u => (u.ContractId == id))
                    .ToList();
            foreach (ApproveStatus status in approveStatusRepository) // Loop through List with foreach
            {
                if (status.StatusId == 2)
                {
                    status.StatusId = 8;
                    _approveStatusRepository.Update(status);
                }
                else if (status.StatusId == 1)
                {
                    status.StatusId = 7;
                    _approveStatusRepository.Update(status);
                }

                _approveStatusRepository.Save();
            }
        }

        public List<ApproveStatus> GetByApproverId(long id)
        {
            if (id > 0)
            {
                return _approveStatusRepository
                .Get()
                .Include(u => u.ApproverTier)
                .Include(u => u.Contract)
                .Where(u => u.ApproverTier.ApproverId == id && u.StatusId == 2)
                .ToList();
           }

           return new List<ApproveStatus>(); //new IQueryable<ApproveStatus>();
        }
    }
}
