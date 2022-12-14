using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}
