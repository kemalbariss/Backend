using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } //miktar demek
        public double Price { get; set; }
        public string Image { get; set; }
        [Key]
        public int ProductId { get; set; }
        public DateTime createDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Order> _orders { get; set; }
    }

    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } //miktar demek
        public double Price { get; set; }
        public string Image { get; set; }
        public int ProductId { get; set; }
        public DateTime createDate { get; set; }
        public int CategoryId { get; set; }
    }

    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }

    }
}
