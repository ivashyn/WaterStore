using Store.BLL.Interfaces;
using Store.BLL.ModelsDTO;
using Store.WebUI.Models;
using Store.WebUI.Models.NavigationModels;
using Store.WebUI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IOrderService _orderService;
        public HomeController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        public ActionResult Index()
        {
            var water = _orderService.GetAllWater();
            return View(water);
        }

        public ActionResult MakeOrder(int waterId)
        {
            ViewBag.WaterId = waterId;
            ViewBag.WaterProvider = _orderService.GetWaterById(waterId).Provider;
            ViewBag.Managers = new SelectList(_orderService.GetAllManagers(), "Id", "Name");
            return PartialView(new OrderDTO());  //OrderViewModel
        }

        [HttpPost]
        public ActionResult MakeOrder(OrderDTO order) //OrderViewModel
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var userId = _orderService.GetUserByEmail(userEmail).Id;
                order.UserId = userId;
                order.Number = CreateOrderNumber();
                _orderService.MakeOrder(order);
                return RedirectToAction("MyOrders");
            }
            return View(order);
        }

        private string CreateOrderNumber()
        {
            var userEmail = User.Identity.Name;
            var userId = _orderService.GetUserByEmail(userEmail).Id;

            var lastOrdersNumber = _orderService.GetLastOrderNumber();
            var numbers = Convert.ToInt32(lastOrdersNumber.Substring(1));
            numbers++;
            var orderNumber = "N" + numbers;
            return orderNumber;
        }

        public ActionResult EditOrder(int orderId)
        {
            ViewBag.OrderId = orderId;
            var order = _orderService.GetOrderById(orderId);
            ViewBag.WaterProvider = new SelectList(_orderService.GetAllWater(), "Id","Provider");
            ViewBag.Managers = new SelectList(_orderService.GetAllManagers(), "Id", "Name");
            return PartialView(order);
        }

        [HttpPost]
        public ActionResult EditOrder(OrderDTO order)
        {
            if (ModelState.IsValid)
            {
                _orderService.EditOrder(order);
                return RedirectToAction("MyOrders");
            }
            return View(order);
        }

        [Authorize]
        public ActionResult GetOrdersPartial(int? waterId, int? managerId, int page = 1, int ordersPerPage = 10)
        {
            var ordersViewModel = GetMyOrders(waterId, managerId, page, ordersPerPage);
            return PartialView(ordersViewModel);
        }

        [Authorize]
        public ActionResult MyOrders(int? waterId, int? managerId, int page = 1, int ordersPerPage = 10)
        {
            var ordersViewModel = GetMyOrders(waterId, managerId, page, ordersPerPage);
            return View(ordersViewModel);
        }

        public OrdersListViewModel GetMyOrders(int? waterId, int? managerId, int page = 1, int ordersPerPage = 10)
        {
            int totalOrders = 0;
            bool recorderOrdersOnPageAndTotalOrders = false;
            var userEmail = User.Identity.Name;
            var userId = _orderService.GetUserByEmail(userEmail).Id;
            IEnumerable<OrderDTO> allUsersOrders = new List<OrderDTO>();
            IEnumerable<OrderDTO> ordersOnPage = new List<OrderDTO>();

            Filter(ref allUsersOrders, ref ordersOnPage, userId, ref totalOrders, ref recorderOrdersOnPageAndTotalOrders, waterId, managerId, page, ordersPerPage);
            var ordersViewModel = Pagination(ordersOnPage, allUsersOrders, totalOrders, recorderOrdersOnPageAndTotalOrders, page, ordersPerPage);
            return ordersViewModel;
        }

        private void Filter(ref IEnumerable<OrderDTO> allUsersOrders,ref IEnumerable<OrderDTO> ordersOnPage, int userId, ref int totalOrders, ref bool recorderOrdersOnPageAndTotalOrders, int? waterId, int? managerId, int page = 1, int ordersPerPage = 10)
        {
            if (waterId != null && waterId != 0)
            {
                //waterId == 7, managerId == 2
                if (managerId != null && managerId != 0)
                {
                    allUsersOrders = _orderService.GetUsersOrders(userId, Convert.ToInt32(waterId), Convert.ToInt32(managerId));//orders.Where(p => p.ManagerId == managerId);
                }
                //waterId == 7
                else
                {
                    allUsersOrders = _orderService.GetUsersOrdersByWater(userId, Convert.ToInt32(waterId)); // orders.Where(p => p.WaterId == waterId);
                }
            }
                //managerId == 2
            else if (managerId != null && managerId != 0)
            {
                allUsersOrders = _orderService.GetUsersOrdersByManager(userId, Convert.ToInt32(managerId));//orders.Where(p => p.ManagerId == managerId);
            }
            //waterId == 0, managerId == 0 
            else
            {
                allUsersOrders = _orderService.GetNUsersOrders(userId, ordersPerPage, (page - 1) * ordersPerPage);
                ordersOnPage = allUsersOrders;
                totalOrders = _orderService.GetTotalUsersOrdersCount(userId);
                recorderOrdersOnPageAndTotalOrders = true;
            }
        }


        private OrdersListViewModel Pagination(IEnumerable<OrderDTO> ordersOnPage, IEnumerable<OrderDTO> allUsersOrders, int totalOrders, bool recorderOrdersOnPageAndTotalOrders, int page = 1, int ordersPerPage = 10)
        {
            if (!recorderOrdersOnPageAndTotalOrders)
            {
                ordersOnPage = allUsersOrders.Skip((page - 1) * ordersPerPage).Take(ordersPerPage);
                totalOrders = allUsersOrders.Count();
            }
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = ordersPerPage, TotalItems = totalOrders };

            List<ManagerDTO> managers = _orderService.GetAllManagers().ToList();
            managers.Insert(0, new ManagerDTO { Name = "All", Id = 0 });

            List<WaterDTO> water = _orderService.GetAllWater().ToList();
            water.Insert(0, new WaterDTO { Provider = "All", Id = 0 });

            var ordersViewModel = new OrdersListViewModel
            {
                Orders = ordersOnPage.ToList(),
                Managers = new SelectList(managers, "Id", "Name"),
                Water = new SelectList(water, "Id", "Provider"),
                ObjectsPerPage = new SelectList(new List<int>() { 10, 15, 20 }),
                PageInfo = pageInfo
            };
            return ordersViewModel;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

    }
}