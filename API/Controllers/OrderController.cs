using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController:ControllerBase
    {
        private readonly BackendDbContext _backendDbContext;

        public OrderController(BackendDbContext backendDbContext)
        {
            _backendDbContext = backendDbContext;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return _backendDbContext.Orders.Include(p => p.Customer).Include(p=>p.Product).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrders(int id)
        {
            var order = _backendDbContext.Orders
      .Include(p => p.Customer)
      .Include(p => p.Product)
      .FirstOrDefault(p => p.OrderId == id);


            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var order = _backendDbContext.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _backendDbContext.Orders.Remove(order);
            _backendDbContext.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Order> PostOrder([FromBody] OrderDto orderDto)
        {
            var order = new Order
            {
                Description = orderDto.Description,
                createDate = orderDto.createDate,
                Address=orderDto.Address,
                CustomerId = orderDto.CustomerId,
                OrderId = orderDto.OrderId,
                ProductId = orderDto.ProductId,
                Status = orderDto.Status,
            };


            // Ürünü ekleme
            _backendDbContext.Orders.Add(order);

            try
            {
                _backendDbContext.SaveChanges(); // Veritabanına kaydet
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }

            // Başarılı bir şekilde eklenmişse, 201 Created döndür
            return CreatedAtAction(nameof(GetOrders), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public ActionResult PutOrder(int id, [FromBody] OrderUpdateDto orderUpdateDto)
        {
            // Mevcut ürünü veritabanından al
            var order = _backendDbContext.Orders.Find(id);

            if (order == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 döndür
            }

            // Güncelleme için mevcut ürünü güncelle
            order.Address = orderUpdateDto.Address;
            order.ProductId = orderUpdateDto.ProductId;
            order.Status = orderUpdateDto.Status;
            order.CustomerId = orderUpdateDto.CustomerId;
            order.createDate = orderUpdateDto.createDate;
            order.Description = orderUpdateDto.Description;

            

            try
            {
                _backendDbContext.SaveChanges(); // Değişiklikleri kaydet
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_backendDbContext.Orders.Any(p => p.OrderId == id))
                {
                    return NotFound(); // Eğer ürün yoksa 404 döndür
                }
                throw; // Başka bir hata varsa fırlat
            }

            return NoContent(); // Başarılı güncelleme için 204 döndür
        }
    }
}
