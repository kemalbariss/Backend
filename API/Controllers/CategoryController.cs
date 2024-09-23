using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController:ControllerBase
    {
        private readonly BackendDbContext _backendDbContext;
        public CategoryController(BackendDbContext backendDbContext)
        {
            _backendDbContext = backendDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return _backendDbContext.Categories.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategories(int id)
        {
            var category = _backendDbContext.Categories.FirstOrDefault(p => p.CategoryId == id);


            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            var category = _backendDbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _backendDbContext.Categories.Remove(category);
            _backendDbContext.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Category> PostCategory([FromBody] CategoryDto categoryDto)
        {
            var category = new Category
            {
                CategoryId = categoryDto.CategoryId,
                createDate=categoryDto.createDate,
                Description = categoryDto.Description,
                Name = categoryDto.Name,
            };



            // Ürünü ekleme
            _backendDbContext.Categories.Add(category);

            try
            {
                _backendDbContext.SaveChanges(); // Veritabanına kaydet
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }

            // Başarılı bir şekilde eklenmişse, 201 Created döndür
            return CreatedAtAction(nameof(GetCategories), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public ActionResult PutCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            // Mevcut ürünü veritabanından al
            var category = _backendDbContext.Categories.Find(id);

            if (category == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 döndür
            }

            // Güncelleme için mevcut ürünü güncelle
            category.Name = categoryUpdateDto.Name;
            category.Description = categoryUpdateDto.Description;

            try
            {
                _backendDbContext.SaveChanges(); // Değişiklikleri kaydet
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_backendDbContext.Categories.Any(p => p.CategoryId == id))
                {
                    return NotFound(); // Eğer ürün yoksa 404 döndür
                }
                throw; // Başka bir hata varsa fırlat
            }

            return NoContent(); // Başarılı güncelleme için 204 döndür
        }
    }
}
