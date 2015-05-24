using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using CCP.WebApi.Models;

namespace CCP.WebApi.Helpers
{
    public static class ErrorsModelBuilder
    {
        public static ErrorsModel GetErrorsModel(string logicError, ModelStateDictionary errorsDictionary)
        {
            var errorsModel = new ErrorsModel();
            if (!String.IsNullOrEmpty(logicError))
            {
                errorsModel.LogicError = logicError;
            }
            if (errorsDictionary != null)
            {
                errorsModel.DataErrors = errorsDictionary.Where(x => x.Value.Errors.Any())
                .ToDictionary( kvp => kvp.Key.Replace("model.",""),kvp => kvp.Value.Errors.Where(e=>!String.IsNullOrEmpty(e.ErrorMessage)).Select(e => e.ErrorMessage));
            }
            return errorsModel;
        }

        public static ErrorsModel GetErrorsModel(string logicError)
        {
            var errorsModel = new ErrorsModel();
            if (!String.IsNullOrEmpty(logicError))
            {
                errorsModel.LogicError = logicError;
            }
            return errorsModel;
        }

        public static ErrorsModel GetErrorsModel(ModelStateDictionary errorsDictionary)
        {
            var errorsModel = new ErrorsModel();
            if (errorsDictionary != null)
            {
                errorsModel.DataErrors = errorsDictionary.Where(x => x.Value.Errors.Any())
                .ToDictionary(kvp => kvp.Key.Replace("model.", ""), kvp => kvp.Value.Errors.Where(e=>!String.IsNullOrEmpty(e.ErrorMessage)).Select(e => e.ErrorMessage));
            }
            return errorsModel;
        }
    }
}