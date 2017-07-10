using Store.BLL.Interfaces;
using Store.BLL.ModelsDTO;
using Store.WebUI.Models;
using Store.WebUI.Models.NavigationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IStoreService _storeService;
        public HomeController(IStoreService orderService)
        {
            _storeService = orderService;
        }


        public ActionResult Index()
        {
            var water = _storeService.GetAllWater();
            return View(water);
        }


        public ActionResult MakeOrder(int waterId)
        {
            ViewBag.WaterId = waterId;
            var water = _storeService.GetWaterById(waterId);
            ViewBag.WaterProvider = water.Provider;
            ViewBag.WaterImageName = water.ImageName;
            ViewBag.Managers = new SelectList(_storeService.GetAllManagers(), "Id", "Name");
            return PartialView(new OrderDTO());
        }

        [HttpPost]
        [Authorize]
        public ActionResult MakeOrder(OrderDTO order)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                order.UserId = userId;
                order.Number = CreateOrderNumber();
                try
                {
                    _storeService.MakeOrder(order);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home", new { @errorText = ex.Message });
                }
                return RedirectToAction("MyOrders");
            }
            return View(order);
        }

        private string CreateOrderNumber()
        {
            var userId = GetUserId();

            var lastOrdersNumber = _storeService.GetLastOrderNumber();
            var numbers = Convert.ToInt32(lastOrdersNumber.Substring(1));
            numbers++;
            var orderNumber = "N" + numbers;
            return orderNumber;
        }

        [Authorize]
        public ActionResult EditOrder(int orderId)
        {
            var userId = GetUserId();

            ViewBag.OrderId = orderId;
            var order = new OrderDTO();
            try
            {
                order = _storeService.GetOrderById(orderId);
                if (order.UserId != userId)
                    return RedirectToAction("Error", "Home", new { @errorText = "Sorry, but this is not your order" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { @errorText = ex.Message });
            }

            ViewBag.WaterImageName = order.WaterDTO.ImageName;

            ViewBag.WaterProvider = new SelectList(_storeService.GetAllWater(), "Id", "Provider");
            ViewBag.Managers = new SelectList(_storeService.GetAllManagers(), "Id", "Name");
            return PartialView(order);
        }

        [HttpPost]
        public ActionResult EditOrder(OrderDTO order)
        {
            if (ModelState.IsValid)
            {
                _storeService.EditOrder(order);
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
            var userId = GetUserId();

            int totalOrders = 0;
            bool recorderOrdersOnPageAndTotalOrders = false;
            IEnumerable<OrderDTO> allUsersOrders = new List<OrderDTO>();
            IEnumerable<OrderDTO> ordersOnPage = new List<OrderDTO>();

            Filter(ref allUsersOrders, ref ordersOnPage, userId, ref totalOrders, ref recorderOrdersOnPageAndTotalOrders, waterId, managerId, page, ordersPerPage);
            var ordersViewModel = Pagination(ordersOnPage, allUsersOrders, totalOrders, recorderOrdersOnPageAndTotalOrders, page, ordersPerPage);
            return ordersViewModel;
        }

        private void Filter(ref IEnumerable<OrderDTO> allUsersOrders, ref IEnumerable<OrderDTO> ordersOnPage, int userId, ref int totalOrders, ref bool recorderOrdersOnPageAndTotalOrders, int? waterId, int? managerId, int page = 1, int ordersPerPage = 10)
        {
            if (waterId != null && waterId != 0)
            {
                //waterId == 7, managerId == 2
                if (managerId != null && managerId != 0)
                {
                    allUsersOrders = _storeService.GetUsersOrders(userId, Convert.ToInt32(waterId), Convert.ToInt32(managerId));
                }
                //waterId == 7
                else
                {
                    allUsersOrders = _storeService.GetUsersOrdersByWater(userId, Convert.ToInt32(waterId));
                }
            }
            //managerId == 2
            else if (managerId != null && managerId != 0)
            {
                allUsersOrders = _storeService.GetUsersOrdersByManager(userId, Convert.ToInt32(managerId));
            }
            //waterId == 0, managerId == 0 
            else
            {
                allUsersOrders = _storeService.GetNUsersOrders(userId, ordersPerPage, (page - 1) * ordersPerPage);
                ordersOnPage = allUsersOrders;
                totalOrders = _storeService.GetTotalUsersOrdersCount(userId);
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

            List<ManagerDTO> managers = _storeService.GetAllManagers().ToList();
            managers.Insert(0, new ManagerDTO { Name = "All", Id = 0 });

            List<WaterDTO> water = _storeService.GetAllWater().ToList();
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

        [Authorize]
        public ActionResult GetGoogleChart()
        {
            var waterOrdersCount = GetWaterOrdersCount();
            return PartialView(waterOrdersCount);
        }

        public Dictionary<string, int> GetWaterOrdersCount()
        {
            var userId = GetUserId();

            var waterOrdersCount = new Dictionary<string, int>();
            var water = _storeService.GetAllWater();
            var amount = 0;
            foreach (var item in water)
            {
                amount = _storeService.GetUsersOrdersByWater(userId, item.Id).Count();
                waterOrdersCount.Add(item.Provider, amount);
            }
            return waterOrdersCount;
        }

        private int GetUserId()
        {
            var userEmail = User.Identity.Name;
            var userId = _storeService.GetUserByEmail(userEmail).Id;
            return userId;
        }

        public ActionResult Error(string errorText = "You Requested the page that is no longer There.")
        {
            ViewBag.ErrorText = errorText;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

    }
}