using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Corum.Models;
using Corum.Models.Interfaces;
using Corum.RestReports;
using Corum.DAL.Entity;

namespace Corum.Common
{
    public class CorumNinjectResolver : IDependencyResolver
    {
        private readonly IKernel kernel;
        public CorumNinjectResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {            
            kernel.Bind<Entities>()
                .ToSelf()
                .InTransientScope();

            kernel.Bind<Func<Entities>>()
                .ToMethod(context => () => context.Kernel.Get<Entities>())
                .InSingletonScope();

            kernel.Bind<ICorumDataProvider>().To<DAL.EFCorumDataProvider>();
            kernel.Bind<IReportRenderer>().To<RestReportRenderer>().InSingletonScope();
        }
    }
}