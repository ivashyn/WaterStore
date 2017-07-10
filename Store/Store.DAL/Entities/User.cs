using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
