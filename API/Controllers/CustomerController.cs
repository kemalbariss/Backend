using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController:ControllerBase
    {
        private readonly BackendDbContext _backendDbContext;

        public CustomerController(BackendDbContext backendDbContext)
        {
            _backendDbContext = backendDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            return _backendDbContext.Customers.ToList();
        }


        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomers(int id)
        {
            var customer = _backendDbContext.Customers.FirstOrDefault(p => p.CustomerId == id);


            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            var customer = _backendDbContext.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            _backendDbContext.Customers.Remove(customer);
            _backendDbContext.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Customer> PostCustomer([FromBody] CustomerDto customerDto)
        {
            var customer = new Customer
            {
                CustomerId = customerDto.CustomerId,
                createDate = customerDto.createDate,
                Email = customerDto.Email,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                PhoneNumber = customerDto.PhoneNumber,
            };


            // Ürünü ekleme
            _backendDbContext.Customers.Add(customer);

            try
            {
                _backendDbContext.SaveChanges(); // Veritabanına kaydet
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }

            // Başarılı bir şekilde eklenmişse, 201 Created döndür
            return CreatedAtAction(nameof(GetCustomers), new { id = customer.CustomerId }, customer);
        }



        [HttpPut("{id}")]
        public ActionResult PutCustomer(int id, [FromBody] CustomerUpdateDto customerUpdateDto)
        {
            // Mevcut ürünü veritabanından al
            var customer = _backendDbContext.Customers.Find(id);

            if (customer == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 döndür
            }

            // Güncelleme için mevcut ürünü güncelle
            customer.FirstName = customerUpdateDto.FirstName;
            customer.LastName = customerUpdateDto.LastName;
            customer.PhoneNumber = customerUpdateDto.PhoneNumber;
            customer.Email = customerUpdateDto.Email;
           

            try
            {
                _backendDbContext.SaveChanges(); // Değişiklikleri kaydet
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_backendDbContext.Customers.Any(p => p.CustomerId == id))
                {
                    return NotFound(); // Eğer ürün yoksa 404 döndür
                }
                throw; // Başka bir hata varsa fırlat
            }

            return NoContent(); // Başarılı güncelleme için 204 döndür
        }
    }
}
