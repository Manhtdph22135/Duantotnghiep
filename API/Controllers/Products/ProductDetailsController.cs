using API.Context;
using API.Models;
using Humanizer;
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
        public async Task<IActionResult> PutProductDetails(int id, [FromForm] ProductDetail productDetail, IFormFile? file)
        {
            if (id != productDetail.ProductId)
            {
                return BadRequest("ID không khớp.");
            }

            var existingProduct = await _contextShop.ProductDetails.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }


            if (file != null && file.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picture");

                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingProduct.Image))
                {
                    var oldFilePath = Path.Combine(folderPath, existingProduct.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Lưu ảnh mới
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                existingProduct.Image = fileName; // Cập nhật tên ảnh mới
            }

            try
            {
                _contextShop.Update(existingProduct);
                await _contextShop.SaveChangesAsync();
                return Ok("Cập nhật thành công.");
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Lỗi cập nhật dữ liệu.");
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<ProductDetail>> PostProductDetails(ProductDetail productDetail, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picture");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Chỉ lưu tên file vào UrlHinhanh
                productDetail.Image = fileName;
            }
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

            // Xóa ảnh nếu tồn tại
            if (!string.IsNullOrEmpty(productDetail.Image))
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picture");
                var filePath = Path.Combine(folderPath, productDetail.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _contextShop.ProductDetails.Remove(productDetail);
            await _contextShop.SaveChangesAsync();
            return Ok("Xoá Thành Công");
        }

        [HttpGet("GetImageById/{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var SanPhamCT = await _contextShop.ProductDetails.FindAsync(id);
            if (SanPhamCT == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picture", SanPhamCT.Image);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "Tệp hình ảnh không tồn tại" });
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var mimeType = "image/jpeg"; // Giả định file là JPEG, có thể kiểm tra kiểu tệp thực tế

            return File(fileStream, mimeType);
        }

    }
}
