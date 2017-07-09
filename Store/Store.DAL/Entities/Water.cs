using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Entities
{
    public class Water
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public double Volume { get; set; }
        public string BottleType { get; set; }
        public double Price { get; set; }
        public string ImageName { get; set; }
    }
}
