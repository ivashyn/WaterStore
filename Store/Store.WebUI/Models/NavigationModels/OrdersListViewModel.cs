using Store.BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.WebUI.Models.NavigationModels
{
    public class OrdersListViewModel
    {
        public IEnumerable<OrderDTO> Orders { get; set; }
        public SelectList Managers { get; set; }
        public SelectList Water { get; set; }
        public SelectList ObjectsPerPage { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}