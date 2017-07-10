
using Store.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.BLL.ModelsDTO;
using Store.DAL.Interfaces;
using AutoMapper;
using Store.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Store.BLL.Services
{
    public class StoreService : IStoreService
    {
        IUnitOfWork db;
        IMapper _mapper;

        public StoreService(IUnitOfWork uow)
        {
            db = uow;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>()
                .ForMember(bll => bll.ManagerDTO, dal => dal.MapFrom(b => b.Manager))
                .ForMember(bll => bll.WaterDTO, dal => dal.MapFrom(b => b.Water));
                cfg.CreateMap<OrderDTO, Order>()
                .ForMember(bll => bll.Manager, dal => dal.MapFrom(b => b.ManagerDTO))
                .ForMember(bll => bll.Water, dal => dal.MapFrom(b => b.WaterDTO));
                cfg.CreateMap<Water, WaterDTO>();
                cfg.CreateMap<WaterDTO, Water>();
                cfg.CreateMap<Manager, ManagerDTO>();
                cfg.CreateMap<ManagerDTO, Manager>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<User, UserDTO>();

            });
            _mapper = config.CreateMapper();
        }

        public void MakeOrder(OrderDTO orderDTO)
        {
            orderDTO.OrderDate = DateTime.Now;
            var orderToAdd = _mapper.Map<OrderDTO, Order>(orderDTO);
            var orderFromDb = db.Orders.Find(o => o.Number == orderToAdd.Number).FirstOrDefault();
            if (orderFromDb != null)
                throw new ValidationException("Sorry, but the order with the same Number is already exsist");
            db.Orders.Create(orderToAdd);
            db.Save();
        }

        public void AddUser(UserDTO userDTO)
        {
            var user = _mapper.Map<UserDTO, User>(userDTO);
            var userFromDb = db.Users.Find(u => u.Email == userDTO.Email).FirstOrDefault();
            if (userFromDb != null)
                throw new ValidationException("Sorry, but the user with the same Email is already exsist");
            db.Users.Create(user);
            db.Save();
        }

        public void EditOrder(OrderDTO orderDTO)
        {
            orderDTO.OrderDate = DateTime.Now;
            var order = _mapper.Map<OrderDTO, Order>(orderDTO);
            db.Orders.Update(order);
            db.Save();
        }

        public int GetTotalUsersOrdersCount(int userId)
        {
            var ordersAmount = db.Orders.Find(o => o.UserId == userId).Count();
            return ordersAmount;
        }

        public string GetLastOrderNumber()
        {
            var order = db.Orders.GetAll().OrderByDescending(o=>o.Id).FirstOrDefault();
            if (order == null)
                return "N0";
            var number = "N" + order.Id;
            return number;
        }

        public OrderDTO GetOrderById(int orderId)
        {
            var order = db.Orders.Get(orderId);
            if (order == null)
                throw new Exception("Sorry, but the order is not exsist");
            return _mapper.Map<Order, OrderDTO>(order);
        }

        public WaterDTO GetWaterById(int id)
        {
            var water = db.Water.Find(h => h.Id == id).FirstOrDefault();
            if (water == null)
                throw new Exception("Sorry, but the water is not exsist");
            return _mapper.Map<Water, WaterDTO>(water);
        }

        public UserDTO GetUserByEmail(string email)
        {
            var user = db.Users.Find(u => u.Email == email).FirstOrDefault();
            if (user == null)
                throw new Exception("Sorry, but the user is not exsist");
            return _mapper.Map<User, UserDTO>(user);
        }

        public IEnumerable<WaterDTO> GetAllWater()
        {
            var Waters = db.Water.GetAll();
            return _mapper.Map<IEnumerable<Water>, IEnumerable<WaterDTO>>(Waters);
        }

        public IEnumerable<OrderDTO> GetAllOrders()
        {
            var orders = db.Orders.GetAll();
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
        }

        public IEnumerable<OrderDTO> GetNUsersOrders(int userId, int amountToTake, int amountToSkip)
        {
            var orders = db.Orders.Find(o => o.UserId == userId)
                .ToList()
                .Skip(amountToSkip)
                .Take(amountToTake);// GetAllUsersOrders(userId).Skip(amountToSkip).Take(amountToTake);
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
        }

        public IEnumerable<OrderDTO> GetAllUsersOrders(int userId)
        {
            var orders = db.Orders.Find(o => o.UserId == userId).ToList();
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
        }

        public IEnumerable<ManagerDTO> GetAllManagers()
        {
            var managers = db.Managers.GetAll();
            return _mapper.Map<IEnumerable<Manager>, IEnumerable<ManagerDTO>>(managers);
        }

        public IEnumerable<OrderDTO> GetUsersOrdersByWater(int userId, int waterId)
        {
            var orders = db.Orders.Find(o => o.UserId == userId && o.WaterId == waterId).ToList();
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
        }

        public IEnumerable<OrderDTO> GetUsersOrdersByManager(int userId, int managerId)
        {
            var orders = db.Orders.Find(o => o.UserId == userId && o.ManagerId == managerId).ToList();
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
        }

        public IEnumerable<OrderDTO> GetUsersOrders(int userId, int waterId, int managerId)
        {
            var orders = db.Orders.Find(o => o.UserId == userId && o.WaterId == waterId && o.ManagerId == managerId).ToList();
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
        }

        public IEnumerable<string> GetAllWatersProvider()
        {
            var providers = new List<string>();
            var allWater = GetAllWater();
            foreach (var water in allWater)
            {
                providers.Add(water.Provider);
            }
            return providers;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
