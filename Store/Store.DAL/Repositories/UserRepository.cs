using Store.DAL.Entities;
using Store.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Store.DAL.EFContext;
using System.Data.Entity;

namespace Store.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        StoreContext db;
        public UserRepository(StoreContext context)
        {
            db = context;
        }
        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Delete(int id)
        {
            var Water = db.Water.Find(id);
            if (Water != null)
                db.Water.Remove(Water);
        }

        public IQueryable<User> Find(System.Linq.Expressions.Expression<Func<User, Boolean>> predicate)
        {
            return db.Users.Where(predicate);
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
