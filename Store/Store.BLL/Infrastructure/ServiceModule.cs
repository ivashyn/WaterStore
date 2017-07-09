using Ninject.Modules;
using Store.DAL.Interfaces;
using Store.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        string connectionString;
        public ServiceModule(string connectinon)
        {
            connectionString = connectinon;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
