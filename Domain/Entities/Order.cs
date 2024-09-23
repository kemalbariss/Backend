using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        [Key]
        public int OrderId { get; set; }
        public DateTime createDate { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }


    }

    public class OrderDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        [Key]
        public int OrderId { get; set; }
        public DateTime createDate { get; set; }
    }
    public class OrderUpdateDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
