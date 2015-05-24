using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCP.DAL.Attributes
{
    public class FormatAttribute: ValidationAttribute
    {

        private static Dictionary<Type,string> erorMessagesDictionary = new Dictionary<Type, string>()
        {
            {typeof(int?),"{0} must be a number"},
            {typeof(DateTime),"{0} must be in format \"mm/dd/yyyy\""}
        };

        private string format;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                format = erorMessagesDictionary.ContainsKey(validationContext.GetType()) ? erorMessagesDictionary[validationContext.GetType()] : base.ErrorMessageString;
                this.
                ErrorMessage = String.Format(format, new object[] {validationContext.DisplayName});
                return new ValidationResult(this.ErrorMessage);
            }

            return null;
        }

        //private bool CheckFormat(object value, Type valueType)
        //{
        //    try
        //    {
        //        var q = Convert.ChangeType(value, valueType);
        //    }
        //    catch (Exception)
        //    {
        //        format = erorMessagesDictionary.ContainsKey(valueType)  ? erorMessagesDictionary[valueType] : base.ErrorMessageString;
        //        return false;
        //    }

        //    return true;
        //}

        //public override string FormatErrorMessage(string name)
        //{
        //    return String.Format(format, new object[] { name });
        //}
    }
}
