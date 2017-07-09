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
    public class ManagerRepository : IRepository<Manager>
    {
        StoreContext db;

        public ManagerRepository(StoreContext context)
        {
            db = context;
        }

        public void Create(Manager item)
        {
            db.Managers.Add(item);
        }

        public void Delete(int id)
        {
            var manager = db.Managers.Find(id);
            if (manager != null)
                db.Managers.Remove(manager);
        }

        public IQueryable<Manager> Find(System.Linq.Expressions.Expression<Func<Manager, Boolean>> predicate)
        {
            return db.Managers.Where(predicate);
        }

        public Manager Get(int id)
        {
            return db.Managers.Find(id);
        }

        public IEnumerable<Manager> GetAll()
        {
            return db.Managers;
        }

        public void Update(Manager item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
