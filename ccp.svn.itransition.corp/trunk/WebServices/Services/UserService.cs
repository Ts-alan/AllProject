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
using ModelState = System.Web.Http.ModelBinding.ModelState;
using UserModel = CCP.WebApi.Models.UserModel;

namespace CCP.WebApi.Services
{
    public partial class UserService : CCPUserManager
    {
        private readonly string DefinedPassword = ConfigurationManager.AppSettings.Get("definedPassword");
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ApproverTier> _approverTierRepository;
        private readonly IRepository<Tier> _tiersRepository;

        public UserService()
        {
            _userRepository = _unitOfWork.UserRepository;
            _approverTierRepository = _unitOfWork.ApproverTierRepository;
            _tiersRepository = _unitOfWork.TiersRepository;
        }

        public DataSourceResult GetUsers(DataSourceRequest model)
        {
            if (model != null)
            {
                return _userRepository
                    .Get()
                    .OrderByDescending(c => c.UserId)
                    .Include(u => u.Role)
                    .GetDataSource(model);
            }
            return new DataSourceResult { Data = null, DataCount = 0 };
        }

        public User GetUser(long id)
        {
            if (id > 0)
            {
                return _userRepository
                    .Get(u => u.UserId == id)
                    .Include(u => u.Role)
                    .Include(u => u.Approvers.Select(a => a.Approver))
                    .SingleOrDefault();
            }
            return new User();
        }

        public User GetUser(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                return _userRepository.Get(filter: u => u.Email.Equals(email)).FirstOrDefault();
            }
            return new User();
        }

        public int Insert(User entity)
        {
            if (entity == null || String.IsNullOrEmpty(entity.Email))
            {
                return -1;
            }
            if (_userRepository.Get().Any(u => u.Email == entity.Email))
            {
                return 1;
            }

            entity.PasswordHash = (new PasswordHasher()).HashPassword(DefinedPassword);
            entity.Approvers = null;
            _userRepository.Insert(entity);
            _userRepository.Save();
            return 0;
        }

        public int Update(User entityToUpdate)
        {
            if (entityToUpdate == null || entityToUpdate.UserId <= 0 || String.IsNullOrEmpty(entityToUpdate.Email))
            {
                return -1;
            }
            if (_userRepository.Get(u => u.UserId != entityToUpdate.UserId).Any(u => u.Email == entityToUpdate.Email))
            {
                return 1;
            }
            IRepository<Contract> contractRepository = _unitOfWork.ContractRepository;
            if (contractRepository.Get().Any(c => c.SalesPersonId == entityToUpdate.UserId))
            {
                return 3;
            }
            var approvers = _approverTierRepository.Get(a => a.SalesPersonId == entityToUpdate.UserId);
            foreach (var approver in approvers)
            {
                _approverTierRepository.Delete(approver);
            }
            entityToUpdate.Approvers = null;
            _userRepository.Update(entityToUpdate);
            _userRepository.Save();
            return 0;
        }

        public int Insert(UserModel entity)
        {
            if (entity == null || entity.User == null || String.IsNullOrEmpty(entity.User.Email))
            {
                return -1;
            }
            if (_userRepository.Get().Any(u => u.Email == entity.User.Email))
            {
                return 1; //Email is not unique
            }
            var tiersCount = _tiersRepository.Get().Count();
            if (entity.ApproverTiers.Length != tiersCount || !entity.ApproverTiers.Any())
            {
                return 2; //Bad approvers
            }
            entity.User.PasswordHash = (new PasswordHasher()).HashPassword(DefinedPassword);
            entity.User.Approvers.Clear();
            foreach (var approverTier in entity.ApproverTiers)
            {
                approverTier.SalesPerson = entity.User;
                entity.User.Approvers.Add(approverTier);
                //_approverTierRepository.Insert(approverTier);
            }
            _userRepository.Insert(entity.User);
            _userRepository.Save();
            return 0;
        }

        public int Update(UserModel entityToUpdate)
        {
            if (entityToUpdate == null || entityToUpdate.User == null || entityToUpdate.User.UserId <= 0 || String.IsNullOrEmpty(entityToUpdate.User.Email))
            {
                return -1;
            }
            if (_userRepository.Get(u => u.UserId != entityToUpdate.User.UserId).Any(u => u.Email == entityToUpdate.User.Email))
            {
                return 1;
            }
            var tiersCount = _tiersRepository.Get().Count();
            if (entityToUpdate.ApproverTiers.Length != tiersCount || !entityToUpdate.ApproverTiers.Any())
            {
                return 2;   //Bad approvers
            }
            var tiers =
                _approverTierRepository.Get(a => a.SalesPersonId == entityToUpdate.User.UserId);
            if (tiers.Count() < tiersCount)     //update from notSalesPerson to SalesPerson
            {
                foreach (var tier in entityToUpdate.ApproverTiers)
                {
                    _approverTierRepository.Insert(tier);
                }
            }
            else                               //update from SalesPerson to SalesPerson
            {
                foreach (var tier in tiers)
                {
                    tier.ApproverId = entityToUpdate.ApproverTiers.Single(t => t.TierId == tier.TierId).ApproverId;
                    _approverTierRepository.Update(tier);
                }
            }
            entityToUpdate.User.Approvers = null;
            _userRepository.Update(entityToUpdate.User);
            _userRepository.Save();
            return 0;
        }

        public int GetTiersCount()
        {
            return _tiersRepository.Get().Count();
        }

        public User GetSalesPerson(string email)
        {
            var user = _userRepository.Get(otherActions: u => u.Include(a => a.Approvers)).SingleOrDefault(u => u.Email == email);
            if (user != null && user.IsSalesPerson)
            {
                return user;
            }
            return null;
        }
    }
}