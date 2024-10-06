using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistance.Context;
using System.Security.Cryptography.Xml;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly BackendDbContext _backendDbContext;
        private readonly IConfiguration _configuration;
        public TokenController(BackendDbContext backendDbContext, IConfiguration configuration)
        {
            _backendDbContext = backendDbContext;
            _configuration = configuration;
        }

     

        //public TokenController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        [HttpGet]
        public IActionResult Get()
        {
            Token token = TokenHandler.CreateToken(_configuration);
            return Ok(token);
        }

        [HttpPost]
        public IActionResult Login([FromBody] CustomerLoginDto loginDto)
        {
            var customer = _backendDbContext.Customers.FirstOrDefault(
                c => c.FirstName == loginDto.FirstName && 
                     c.LastName == loginDto.LastName &&
                     c.Email == loginDto.Email
                );
            if (customer == null) 
            {
                return Unauthorized();
            }
            Token token = TokenHandler.CreateToken(_configuration);
            return Ok(token);
        }
    }
}
