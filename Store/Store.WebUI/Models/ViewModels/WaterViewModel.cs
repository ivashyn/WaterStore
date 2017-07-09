using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.WebUI.Models.ViewModels
{
    public class WaterViewModel
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public double Volume { get; set; }
        public string BottleType { get; set; }
        public double Price { get; set; }
    }
}