using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly BackendDbContext _backendDbContext;

        public ProductController(BackendDbContext backendDbContext)
        {
            _backendDbContext = backendDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _backendDbContext.Products.ToList();
        }



        [HttpGet("{id}")]
        public ActionResult<Product> GetProducts(int id)
        {
            var product = _backendDbContext.Products
      .FirstOrDefault(p => p.ProductId == id);


            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _backendDbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _backendDbContext.Products.Remove(product);
            _backendDbContext.SaveChanges();

            return NoContent();
        }


        [HttpPost]
        public ActionResult<Product> PostProduct([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
               Description = productDto.Description,
               Price = productDto.Price,
            Quantity = productDto.Quantity,
            createDate = productDto.createDate,
            Image = productDto.Image
            
            };



            // Ürünü ekleme
            _backendDbContext.Products.Add(product);

            try
            {
                _backendDbContext.SaveChanges(); // Veritabanına kaydet
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }

            // Başarılı bir şekilde eklenmişse, 201 Created döndür
            return CreatedAtAction(nameof(GetProducts), new { id = product.ProductId }, product);
        }



        [HttpPut("{id}")]
        public ActionResult PutProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            // Mevcut ürünü veritabanından al
            var product = _backendDbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 döndür
            }

            // Güncelleme için mevcut ürünü güncelle
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.Quantity = productUpdateDto.Quantity;
            product.Image = productUpdateDto.Image;
            product.CategoryId = productUpdateDto.CategoryId;

            try
            {
                _backendDbContext.SaveChanges(); // Değişiklikleri kaydet
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_backendDbContext.Products.Any(p => p.ProductId == id))
                {
                    return NotFound(); // Eğer ürün yoksa 404 döndür
                }
                throw; // Başka bir hata varsa fırlat
            }

            return NoContent(); // Başarılı güncelleme için 204 döndür
        }






    }
}
