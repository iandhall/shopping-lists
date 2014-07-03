//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using LightInject;
//using Microsoft.AspNet.SignalR;
//using LogForMe;

//namespace ShoppingLists.Web {
//    internal class LightInjectSignalrDependencyResolver : DefaultDependencyResolver {

//        private readonly IServiceFactory serviceFactory;

//        public LightInjectSignalrDependencyResolver(IServiceFactory serviceFactory) {
//            this.serviceFactory = serviceFactory;
//        }

//        public override object GetService(Type serviceType) {
//            //Logger.Debug("Type={0}", serviceType.Name);
//            return serviceFactory.TryGetInstance(serviceType) ?? base.GetService(serviceType);
//        }

//        public override IEnumerable<object> GetServices(Type serviceType) {
//            //Logger.Debug("Type={0}", serviceType.Name);
//            return serviceFactory.GetAllInstances(serviceType).Concat(base.GetServices(serviceType));
//        }
//    }
//}