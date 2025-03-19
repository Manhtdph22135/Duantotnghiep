using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace PolyShopView.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string API_BASE_URL = "https://localhost:7055/api/ProductDetails";
        private const string CATEGORY_API_URL = "https://localhost:7055/api/ProductCategory";

        public ProductDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Lấy danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(API_BASE_URL);
            if (!response.IsSuccessStatusCode) return View(new List<ProductDetail>());

            var apiData = await response.Content.ReadAsStringAsync();
            var productDetails = JsonConvert.DeserializeObject<List<ProductDetail>>(apiData);
            return View(productDetails);
        }

        // Lấy chi tiết sản phẩm theo ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin sản phẩm từ API
            var productResponse = await _httpClient.GetAsync($"{API_BASE_URL}/{id}");
            if (!productResponse.IsSuccessStatusCode) return NotFound();

            var productData = await productResponse.Content.ReadAsStringAsync();
            var productDetail = JsonConvert.DeserializeObject<ProductDetail>(productData);
            if (productDetail == null) return NotFound();

            // Lấy danh mục sản phẩm từ API
            var categoryResponse = await _httpClient.GetAsync(CATEGORY_API_URL);
            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryData = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(categoryData);
            }

            return View(productDetail);
        }

        // Hiển thị form tạo sản phẩm
        public async Task<IActionResult> Create()
        {
            await LoadCategoryList();
            return View();
        }

        // Xử lý tạo sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDetail detail)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryList();
                return View(detail);
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(detail), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(API_BASE_URL, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi tạo sản phẩm.");
            await LoadCategoryList();
            return View(detail);
        }

        // Hiển thị form chỉnh sửa sản phẩm
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var response = await _httpClient.GetAsync($"{API_BASE_URL}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var productData = await response.Content.ReadAsStringAsync();
            var productDetail = JsonConvert.DeserializeObject<ProductDetail>(productData);
            if (productDetail == null) return NotFound();

            await LoadCategoryList(productDetail.ProductDetailId);
            return View(productDetail);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDetail detail)
        {
            if (id != detail.ProductId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadCategoryList(detail.ProductDetailId);
                return View(detail);
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(detail), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{API_BASE_URL}/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sản phẩm.");
            await LoadCategoryList(detail.ProductDetailId);
            return View(detail);
        }

        // Xóa sản phẩm
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var response = await _httpClient.GetAsync($"{API_BASE_URL}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var productData = await response.Content.ReadAsStringAsync();
            var productDetail = JsonConvert.DeserializeObject<ProductDetail>(productData);
            if (productDetail == null) return NotFound();

            return View(productDetail);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{API_BASE_URL}/{id}");
            return response.IsSuccessStatusCode ? RedirectToAction(nameof(Index)) : Problem("Không thể xóa sản phẩm.");
        }

        // Hàm load danh sách danh mục sản phẩm
        private async Task LoadCategoryList(int? selectedCategoryId = null)
        {
            var response = await _httpClient.GetAsync(CATEGORY_API_URL);
            if (response.IsSuccessStatusCode)
            {
                var categoryData = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(categoryData);
                ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", selectedCategoryId);
            }
            else
            {
                ViewBag.CategoryId = new SelectList(new List<ProductCategory>(), "CategoryId", "CategoryName");
            }
        }
    }
}

