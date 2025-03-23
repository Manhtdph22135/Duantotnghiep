using API.Context;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Products
{
    [Route("api/[controller]")]
    public class ProductDetailsController : Controller
    {
        private readonly DbContextShop _contextShop;

        public ProductDetailsController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetProductstDetails()
        {
            return await _contextShop.ProductDetails.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetail>> GetProductDetails(int id)
        {
            var productDetail = await _contextShop.ProductDetails.FindAsync(id);

            if (productDetail == null)
            {
                return NotFound();
            }

            return productDetail;
        }
        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductDetails(int id, ProductDetail productDetail)
        {
            if (id != productDetail.ProductId)
            {
                return BadRequest();
            }

            _contextShop.Entry(productDetail).State = EntityState.Modified;

            try
            {
                await _contextShop.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExit(id))
                {
                    _contextShop.ProductDetails.Update(productDetail);
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
        public async Task<ActionResult<ProductDetail>> PostProductDetails(ProductDetail productDetail)
        {
            _contextShop.ProductDetails.Add(productDetail);
            await _contextShop.SaveChangesAsync();

            return CreatedAtAction("GetProductDetails", new { id = productDetail.ProductDetailId }, productDetail);
        }
        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDetails(int id)
        {
            var productDetail = await _contextShop.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return NotFound("Không tìm thấy Sản phẩm chi tiết");
            }

            _contextShop.ProductDetails.Remove(productDetail);
            await _contextShop.SaveChangesAsync();
            return Ok("Xoá Thành Công");
        }
        private bool ProductExit(int id)
        {
            return _contextShop.ProductDetails.Any(e => e.ProductDetailId == id);
        }
    }
}
