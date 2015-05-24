using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CCP.DAL;
using CCP.DAL.DataModels;
using CCP.DAL.Interfaces;
using CCP.WebApi.Extensions;
using CCP.WebApi.Models;

namespace CCP.WebApi.Services
{
    public class SalesPersonService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRepository<SalesPerson> _salesPersonRepository;
        private readonly IRepository<User> _usersRepository;
        public SalesPersonService()
        {
            _salesPersonRepository = _unitOfWork.SalesPersonRepository;
            _usersRepository = _unitOfWork.UserRepository;
        }

        public DataSourceResult GetSalesPersons(DataSourceRequest model)
        {
            var context = new CCPEntities();
            return  _salesPersonRepository
                .Get(otherActions: q => q.Include(s => s.User).Include(s => s.Contracts).AsNoTracking(), orderBy: q => q.OrderByDescending(c => c.SalesPersonId)).GetDataSource(model);
        }

        public SalesPerson GetSalesPerson(long id)
        {
            return _salesPersonRepository.Get(s => s.SalesPersonId == id).Include(s => s.User).Single();
        }

        public void AddSalesPerson(SalesPerson entity)
        {
            _salesPersonRepository.Insert(entity);
            _salesPersonRepository.Save();
        }

        public void UpdateSalesPerson(SalesPerson entityToUpdate)
        {
            _usersRepository.Update(_usersRepository.GetById(entityToUpdate.UserId));
            _salesPersonRepository.Update(entityToUpdate);
            _salesPersonRepository.Save();
        }



    }
}