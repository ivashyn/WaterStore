using Store.DAL.EFContext;
using Store.DAL.Entities;
using Store.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        StoreContext db;

        public OrderRepository(StoreContext context)
        {
            db = context;
        }
        public void Create(Order item)
        {
            db.Orders.Add(item);
        }

        public void Delete(int id)
        {
            var order = db.Orders.Find(id);
            if (order != null)
                db.Orders.Remove(order);
        }

        public IQueryable<Order> Find(System.Linq.Expressions.Expression<Func<Order, Boolean>> predicate)
        {
            return db.Orders.Where(predicate);
        }

        public Order Get(int id)
        {
            return db.Orders.Find(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return db.Orders;
        }

        public void Update(Order item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
