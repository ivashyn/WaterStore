using Store.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Water> Water { get; }
        IRepository<Order> Orders { get; }
        IRepository<Manager> Managers { get; }
        IRepository<User> Users { get; }
        void Save();
    }
}
