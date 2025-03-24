using API.Context;
using API.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace PolyShopView.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            string requestURL = "https://localhost:7055/api/Customer";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<List<Customer>>(apiData);
            return View(customer); // truyền list vừa nhận được sang view
        }

        public async Task<IActionResult> Details(int id)
        {
            string requestURL = $"https://localhost:7055/api/Customer/{id}";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(apiData);
            return View(customer); // truyền customer vừa nhận được sang view
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            string requestURL = "https://localhost:7055/api/Customer";
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(customer); // truyền customer vừa nhận được sang view
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            string requestURL = $"https://localhost:7055/api/Customer/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(customer); // truyền customer vừa nhận được sang view
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string requestURL = $"https://localhost:7055/api/Customer/{id}";
            var response = await _httpClient.DeleteAsync(requestURL);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(); // truyền customer vừa nhận được sang view
        }
    }
}
