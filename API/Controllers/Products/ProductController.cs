using API.Context;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace API.Controllers.Products
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly DbContextShop _contextShop;
        public ProductController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _contextShop.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var products = await _contextShop.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }
        //[HttpGet("{productName}")]
        //public async Task<ActionResult<Product>> GetProductByName(string productName)
        //{
        //    var products = await _contextShop.Products.FirstOrDefaultAsync(x => x.ProductName == productName);
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }
        //    return products;
        //}
        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _contextShop.Entry(product).State = EntityState.Modified;

            try
            {
                await _contextShop.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExit(id))
                {
                    _contextShop.Products.Update(product);
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
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            _contextShop.Products.Add(product);
            await _contextShop.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }
        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var customer = await _contextShop.Products.FindAsync(id);
            if (customer == null)
            {
                return NotFound("Không tìm thấy id khách hàng");
            }

            _contextShop.Products.Remove(customer);
            await _contextShop.SaveChangesAsync();
            return Ok("Xoá Thành Công");
        }
        private bool ProductExit(int id)
        {
            return _contextShop.Products.Any(e => e.ProductId == id);
        }
    }
}
