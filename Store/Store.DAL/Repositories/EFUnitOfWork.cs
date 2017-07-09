using Store.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.DAL.EFContext;
using Store.DAL.Entities;

namespace Store.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        StoreContext db;
        WaterRepository waterRepository;
        OrderRepository orderRepository;
        ManagerRepository managerRepository;
        UserRepository userRepository;

        public EFUnitOfWork()  //delete connectionString
        {
            db = new StoreContext();
        }

        public IRepository<Water> Water
        {
            get
            {
                if (waterRepository == null)
                    waterRepository = new WaterRepository(db);
                return waterRepository;
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }
        }

        public IRepository<Manager> Managers
        {
            get
            {
                if (managerRepository == null)
                    managerRepository = new ManagerRepository(db);
                return managerRepository;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
