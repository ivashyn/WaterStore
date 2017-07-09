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
    public class WaterRepository : IRepository<Water>
    {
        StoreContext db;

        public WaterRepository(StoreContext context)
        {
            db = context;
        }
        public void Create(Water item)
        {
            db.Water.Add(item);
        }

        public void Delete(int id)
        {
            var Water = db.Water.Find(id);
            if (Water != null)
                db.Water.Remove(Water);
        }

        public IQueryable<Water> Find(System.Linq.Expressions.Expression<Func<Water, Boolean>> predicate)
        {
            return db.Water.Where(predicate);
        }

        public Water Get(int id)
        {
            return db.Water.Find(id);
        }

        public IEnumerable<Water> GetAll()
        {
            return db.Water;
        }

        public void Update(Water item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
