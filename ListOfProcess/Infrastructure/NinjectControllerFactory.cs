using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ListOfProcess.Services;
using Ninject;
using ListOfProcess.ConnectionLogic;


namespace ListOfProcess.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {

        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            ninjectKernel.Bind<IConnection>().To<ListOfProcess.ConnectionLogic.ConnectionLogic>();
        }
    }
}
