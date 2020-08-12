using DatabaseConnection;
using DatabaseConnection.Interfaces;
using IntegrationApi.Interfaces;
using IntegrationApi.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationApi.Ninject
{
    public class IntegrationApiConfig
    {
        private IKernel _kernel;

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public IKernel GetKernel()
        {
            if (_kernel != null)
            {
                return _kernel;
            }
            _kernel = new StandardKernel();
            _kernel.Bind<IDataBase>().To<DataBase>();
            _kernel.Bind<IOccasion>().To<Occasion>();
            _kernel.Bind<IDataBaseService>().To<DataBaseService>();
            return _kernel;
        }
    }
}
