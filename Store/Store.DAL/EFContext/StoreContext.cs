using Store.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.EFContext
{
    public class StoreContext : DbContext
    {
        public DbSet<Water> Water { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<User> Users { get; set; }

        static StoreContext()
        {
            Database.SetInitializer(new StoreDbInitializer());
        }

        public StoreContext()
            : base("StoreDB")
        {

        }
    }

    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<StoreContext>//DropCreateDatabaseAlways<StoreContext>
    {
        protected override void Seed(StoreContext db)
        {
            db.Water.Add(new Water { Id = 1, Provider = "Vesna", Volume = 18.9, BottleType = "Glass", ImageName = "vesna.jpg", Price = 56 });
            db.Water.Add(new Water { Id = 2, Provider = "Oazys", Volume = 18.9, BottleType = "Polycarbonate", ImageName = "oasis.jpg", Price = 52 });
            db.Water.Add(new Water { Id = 3, Provider = "Berehynia", Volume = 18.9, BottleType = "Polycarbonate", ImageName = "berehynya.jpg", Price = 63 });
            db.Water.Add(new Water { Id = 4, Provider = "Sribna", Volume = 20, BottleType = "Polycarbonate", ImageName = "sribna.jpg", Price = 72 });
            db.Water.Add(new Water { Id = 5, Provider = "Effect", Volume = 18.9, BottleType = "Polycarbonate", ImageName = "effekt.jpg", Price = 60 });
            db.Water.Add(new Water { Id = 6, Provider = "Iceberg", Volume = 19, BottleType = "Polycarbonate", ImageName = "iceberg.jpg", Price = 49 });
            db.Water.Add(new Water { Id = 7, Provider = "Arkhyz", Volume = 19, BottleType = "Polycarbonate", ImageName = "arkhyz19.jpg", Price = 280 });
            db.Water.Add(new Water { Id = 8, Provider = "Ecobutel", Volume = 11.3, BottleType = "Glass", ImageName = "eco-bottle.jpg", Price = 109 });
            db.Water.Add(new Water { Id = 9, Provider = "Etalon", Volume = 20, BottleType = "Glass", ImageName = "etalon.jpg", Price = 97 });
            db.Water.Add(new Water { Id = 10, Provider = "Ukrainochka", Volume = 19, BottleType = "Glass", ImageName = "ukrainochka.jpg", Price = 350 });
            db.Water.Add(new Water { Id = 11, Provider = "Tayana", Volume = 18.9, BottleType = "Polycarbonate", ImageName = "tayana.jpg", Price = 68 });

            db.Managers.Add(new Manager { Id = 1, Name = "Ivanov", Age = 25 });
            db.Managers.Add(new Manager { Id = 2, Name = "Petrov", Age = 20 });
            db.Managers.Add(new Manager { Id = 3, Name = "Sidorov", Age = 30 });

            #region Orders
            var rand = new Random();
            var userId = 0;
            var waterId = 0;
            var managerId = 0;
            var day = 0;
            var month = 0;
            var records = 500;
            for (int i = 1; i < records; i++)
            {
                userId = rand.Next(1, 3);
                waterId = rand.Next(1, 12);
                managerId = rand.Next(1, 4);
                day = rand.Next(1, 28);
                month = rand.Next(1, 13);
                db.Orders.Add(new Order { Id = i, Number = "N"+i, UserId = userId, WaterId = waterId, OrderDate = new DateTime(2017, month, day), ManagerId = managerId, Annotation = i+" order" });
            }

            #endregion

            db.Users.Add(new User { Id = 1, Email = "ivashyn.vadym@gmail.com" });
            db.Users.Add(new User { Id = 2, Email = "user@gmail.com" });
            db.SaveChanges();
        }

    }
}
