using API.Context;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly DbContextShop _contextShop;
        public ProductCategoryController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategory()
        {
            return await _contextShop.ProductCategories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategorys(int id)
        {
            var products = await _contextShop.ProductCategories.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }
        // PUT: api/PruductCattegory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _contextShop.Entry(category).State = EntityState.Modified;

            try
            {
                await _contextShop.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExit(id))
                {
                    _contextShop.ProductCategories.Update(category);
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
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory category)
        {
            _contextShop.ProductCategories.Add(category);
            await _contextShop.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = category.CategoryId }, category);
        }
        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            var Pc = await _contextShop.ProductCategories.FindAsync(id);
            if (Pc == null)
            {
                return NotFound("Không tìm thấy id ");
            }

            _contextShop.ProductCategories.Remove(Pc);
            await _contextShop.SaveChangesAsync();
            return Ok("Xoá Thành Công");
        }
        private bool ProductCategoryExit(int id)
        {
            return _contextShop.ProductCategories.Any(e => e.CategoryId == id);
        }
    }
}

