using System;
using CCP.DAL.DataModels;

namespace CCP.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly CCPEntities _context;
        private GenericRepository<Contract> _contractRepository;
        private GenericRepository<User> _userRepository;
        //private GenericRepository<SalesPerson> _salesPersonRepository;
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<EndUser> _endUsersRepository;
        private GenericRepository<ContractStatusType> _statusesRepository;
        private GenericRepository<Role> _rolesRepository;
        private GenericRepository<ApproverTier> _approverTierRepository;
        private GenericRepository<Tier> _tiersRepository;
        private GenericRepository<Area> _areaRepository;
        private GenericRepository<AreaRole> _areaRoleRepository; 
        private GenericRepository<ApproveStatus> _approveStatusRepository;
        private GenericRepository<ApproveStatusType> _approveStatusTypeRepository;
        private GenericRepository<ContractStatusType> _contractStatusTypeRepository;
        private GenericRepository<Message> _messageRepository;
        private GenericRepository<AreaRoleView> _vAreaRoleRepository;

        public UnitOfWork()
        {
            this._context = new CCPEntities();
            this._context.Configuration.ValidateOnSaveEnabled = false;
            this._context.Configuration.AutoDetectChangesEnabled = false;
            this._context.Configuration.LazyLoadingEnabled = false;
            this._context.Configuration.ProxyCreationEnabled = false;
        }
        public GenericRepository<Contract> ContractRepository
        {
            get
            {
                if (_contractRepository == null)
                {
                    _contractRepository = new GenericRepository<Contract>(_context);
                }
                return _contractRepository;
            }
        }

        public GenericRepository<Message> MessageRepository
        {
            get
            {
                if (_messageRepository == null)
                {
                    _messageRepository = new GenericRepository<Message>(_context);
                }
                return _messageRepository;
            }
        }

        public GenericRepository<AreaRoleView> vAreaRoleRepository
        {
            get
            {
                if (_vAreaRoleRepository == null)
                {
                    _vAreaRoleRepository = new GenericRepository<AreaRoleView>(_context);
                }
                return _vAreaRoleRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {

                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<User>(_context);
                }
                return _userRepository;
            }
        }

        //public GenericRepository<SalesPerson> SalesPersonRepository
        //{
        //    get
        //    {

        //        if (_salesPersonRepository == null)
        //        {
        //            _salesPersonRepository = new GenericRepository<SalesPerson>(_context);
        //        }
        //        return _salesPersonRepository;
        //    }
        //}

        public GenericRepository<Area> AreaRepository
        {
            get
            {

                if (_areaRepository == null)
                {
                    _areaRepository = new GenericRepository<Area>(_context);
                }
                return _areaRepository;
            }
        }

        public GenericRepository<Customer> CustomerRepository
        {
            get
            {

                if (_customerRepository == null)
                {
                    _customerRepository = new GenericRepository<Customer>(_context);
                }
                return _customerRepository;
            }
        }


        public GenericRepository<EndUser> EndUsersRepository
        {
            get
            {
                if (_endUsersRepository == null)
                {
                    _endUsersRepository = new GenericRepository<EndUser>(_context);
                }
                return _endUsersRepository;
            }
        }

        public GenericRepository<ContractStatusType> StatusesRepository
        {
            get
            {
                if (_statusesRepository == null)
                {
                    _statusesRepository = new GenericRepository<ContractStatusType>(_context);
                }
                return _statusesRepository;
            }
        }

        public GenericRepository<Role> RolesRepository
        {
            get
            {
                if (_rolesRepository== null)
                {
                    _rolesRepository = new GenericRepository<Role>(_context);
                }
                return _rolesRepository;
            }
        }

        public GenericRepository<ApproverTier> ApproverTierRepository
        {
            get
            {
                if (_approverTierRepository == null)
                {
                    _approverTierRepository = new GenericRepository<ApproverTier>(_context);
                }
                return _approverTierRepository;
            }
        }

        public GenericRepository<Tier> TiersRepository
        {
            get
            {
                if (_tiersRepository == null)
                {
                    _tiersRepository = new GenericRepository<Tier>(_context);
                }
                return _tiersRepository;
            }
        }

        public GenericRepository<ApproveStatus> ApproveStatusRepository
        {
            get
            {
                if (_approveStatusRepository == null)
                {
                    _approveStatusRepository = new GenericRepository<ApproveStatus>(_context);
                }
                return _approveStatusRepository;
            }
        }

        public GenericRepository<ApproveStatusType> ApproveStatusTypeRepository
        {
            get
            {
                if (_approveStatusTypeRepository== null)
                {
                    _approveStatusTypeRepository = new GenericRepository<ApproveStatusType>(_context);
                }
                return _approveStatusTypeRepository;
            }
        }

        public GenericRepository<ContractStatusType> ContractStatusTypeRepository
        {
            get
            {
                if (_contractStatusTypeRepository == null)
                {
                    _contractStatusTypeRepository = new GenericRepository<ContractStatusType>(_context);
                }
                return _contractStatusTypeRepository;
            }
        }

        public GenericRepository<AreaRole> AreaRoleRepository
        {
            get
            {
                if (_areaRoleRepository == null)
                {
                    _areaRoleRepository = new GenericRepository<AreaRole>(_context);
                }
                return _areaRoleRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
