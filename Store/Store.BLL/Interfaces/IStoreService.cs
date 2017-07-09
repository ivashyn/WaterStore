using Store.BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BLL.Interfaces
{
    public interface IStoreService : IDisposable
    {
        void MakeOrder(OrderDTO orderDTO);
        void AddUser(UserDTO userDTO);
        void EditOrder(OrderDTO orderDTO);
        int GetTotalUsersOrdersCount(int userId);
        string GetLastOrderNumber();
        OrderDTO GetOrderById(int orderId);
        WaterDTO GetWaterById(int id);
        UserDTO GetUserByEmail(string email);
        IEnumerable<WaterDTO> GetAllWater();
        IEnumerable<string> GetAllWatersProvider();
        IEnumerable<ManagerDTO> GetAllManagers();
        IEnumerable<OrderDTO> GetAllOrders();
        IEnumerable<OrderDTO> GetAllUsersOrders(int userId);
        IEnumerable<OrderDTO> GetUsersOrdersByWater(int userId, int waterId);
        IEnumerable<OrderDTO> GetUsersOrdersByManager(int userId, int managerId);
        IEnumerable<OrderDTO> GetUsersOrders(int userId, int waterId, int managerId);

        IEnumerable<OrderDTO> GetNUsersOrders(int userId, int amountToTake, int amountToSkip);
    }
}
