using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PolyShopView.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string API_BASE_URL = "https://localhost:7055/api/Product";

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Lấy danh sách sản phẩm từ API
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(API_BASE_URL);
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Product>());
            }

            var apiData = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(apiData);
            return View(products);
        }

        // Lấy chi tiết sản phẩm theo ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin sản phẩm từ API
            var productResponse = await _httpClient.GetAsync($"{API_BASE_URL}/{id}");
            if (!productResponse.IsSuccessStatusCode) return NotFound();

            var productData = await productResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(productData);
            if (product == null) return NotFound();

            // Lấy danh mục sản phẩm từ API
            string categoryUrl = "https://localhost:7055/api/ProductCategory";
            var categoryResponse = await _httpClient.GetAsync(categoryUrl);

            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryData = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(categoryData);

                // Tìm danh mục của sản phẩm
                var productCategory = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
                product.Category = productCategory; // Gán category vào Product
            }

            return View(product);
        }



        // Hiển thị form tạo sản phẩm
        public async Task<IActionResult> Create()
        {
            string categoryUrl = "https://localhost:7055/api/ProductCategory"; // API lấy danh mục
            var response = await _httpClient.GetAsync(categoryUrl);

            if (response.IsSuccessStatusCode)
            {
                var apiData = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(apiData);
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(new List<ProductCategory>(), "CategoryId", "CategoryName");
            }

            return View();
        }


        // Gửi yêu cầu API để tạo sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, DateTime dateTime)
        {
            if (!ModelState.IsValid)
            {
                // Debug: In ra lỗi nếu có
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
                return View(product);
            }

            product.CreatedAt = DateTime.Now;
            product.UpdateAt = DateTime.Now;
            var jsonContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(API_BASE_URL, jsonContent);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(product);
        }


        // Hiển thị form chỉnh sửa sản phẩm
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin sản phẩm từ API
            var productResponse = await _httpClient.GetAsync($"{API_BASE_URL}/{id}");
            if (!productResponse.IsSuccessStatusCode) return NotFound();

            var productData = await productResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(productData);

            if (product == null) return NotFound();

            // Lấy danh mục sản phẩm từ API
            string categoryUrl = "https://localhost:7055/api/ProductCategory";
            var categoryResponse = await _httpClient.GetAsync(categoryUrl);

            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryData = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(categoryData);
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(new List<ProductCategory>(), "CategoryId", "CategoryName");
            }

            return View(product);
        }


        // Gửi yêu cầu API để cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId) return NotFound();

            if (ModelState.IsValid)
            {
                // Lấy dữ liệu sản phẩm hiện tại từ API để giữ nguyên CreatedAt
                var existingResponse = await _httpClient.GetAsync($"{API_BASE_URL}/{id}");
                if (!existingResponse.IsSuccessStatusCode) return NotFound();

                var existingData = await existingResponse.Content.ReadAsStringAsync();
                var existingProduct = JsonConvert.DeserializeObject<Product>(existingData);
                if (existingProduct == null) return NotFound();

                // Giữ nguyên CreatedAt, chỉ cập nhật UpdateAt
                product.CreatedAt = existingProduct.CreatedAt;
                product.UpdateAt = DateTime.Now;

                var jsonContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{API_BASE_URL}/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Hiển thị xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin sản phẩm từ API
            var productResponse = await _httpClient.GetAsync($"{API_BASE_URL}/{id}");
            if (!productResponse.IsSuccessStatusCode) return NotFound();

            var productData = await productResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(productData);
            if (product == null) return NotFound();

            // Lấy danh mục sản phẩm từ API
            string categoryUrl = "https://localhost:7055/api/ProductCategory";
            var categoryResponse = await _httpClient.GetAsync(categoryUrl);

            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryData = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(categoryData);

                // Tìm danh mục của sản phẩm
                var productCategory = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
                product.Category = productCategory; // Gán category vào Product
            }

            return View(product);
        }

        // Gửi yêu cầu API để xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{API_BASE_URL}/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return BadRequest();
        }
    }
}
