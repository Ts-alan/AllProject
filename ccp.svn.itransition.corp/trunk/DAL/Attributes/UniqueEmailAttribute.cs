using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCP.DAL.DataModels;
using CCP.DAL.Interfaces;

namespace CCP.DAL.Attributes
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRepository<User> _userRepository;

        public UniqueEmailAttribute()
        {
            _userRepository = _unitOfWork.UserRepository;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && _userRepository.Get().Any(u=>u.Email == (string) value))
                return new ValidationResult("Email must be unique.");
            else
                return null;
        }

       
    }
}
