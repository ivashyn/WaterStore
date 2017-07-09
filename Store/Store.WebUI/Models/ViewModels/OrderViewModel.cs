using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.WebUI.Models.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime OrderDate { get; set; }
        public string Annotation { get; set; }

        public int ManagerId { get; set; }
        public virtual ManagerViewModel ManagerViewModel { get; set; }
    }
}