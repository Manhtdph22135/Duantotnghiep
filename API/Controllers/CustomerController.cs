
using API.Context;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DbContextShop _contextShop;

        public CustomerController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _contextShop.Customers.ToListAsync();
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _contextShop.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _contextShop.Entry(customer).State = EntityState.Modified;

            try
            {
                await _contextShop.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    _contextShop.Customers.Update(customer);
                    _contextShop.SaveChanges();
                    return Ok("Cập Nhật Thành Công");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _contextShop.Customers.Add(customer);
            await _contextShop.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _contextShop.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("Không tìm thấy id khách hàng");
            }

            _contextShop.Customers.Remove(customer);
            await _contextShop.SaveChangesAsync();

            return Ok("Xoá Thành Công");
        }

        private bool CustomerExists(int id)
        {
            return _contextShop.Customers.Any(e => e.CustomerId == id);
        }
    }
}
