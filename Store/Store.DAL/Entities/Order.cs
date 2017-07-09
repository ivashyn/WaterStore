using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Entities
{
    public class Order
    {
        
        public int Id { get; set; }
        [StringLength(50)]
        public string Number { get; set; }
        public DateTime OrderDate { get; set; }
        [StringLength(20)]
        public string Annotation { get; set; }
        public int UserId { get; set; }

        [Required]
        public int WaterId { get; set; }
        public virtual Water Water { get; set; }
        [Required]
        public int ManagerId { get; set; }
        public virtual Manager Manager { get; set; }
    }
}
