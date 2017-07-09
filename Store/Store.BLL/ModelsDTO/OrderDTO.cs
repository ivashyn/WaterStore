using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BLL.ModelsDTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime OrderDate { get; set; }
        public string Annotation { get; set; }
        public int UserId { get; set; }


        public int WaterId { get; set; }
        public virtual WaterDTO WaterDTO { get; set; }
        public int ManagerId { get; set; }
        public virtual ManagerDTO ManagerDTO { get; set; }

    }
}
