using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public DateTime createDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> _products { get; set; }
    }
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public DateTime createDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class CategoryUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
