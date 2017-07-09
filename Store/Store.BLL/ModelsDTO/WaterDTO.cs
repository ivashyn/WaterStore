using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BLL.ModelsDTO
{
    public class WaterDTO
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public double Volume { get; set; }
        public string BottleType { get; set; }
        public double Price { get; set; }
    }
}
