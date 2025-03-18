using System.Text;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PolyShopView.Controllers
{
    public class BillsController : Controller
    {
        private readonly HttpClient _httpClient;

        public BillsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            string requestURL = "https://localhost:7055/api/Bills";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var bill = JsonConvert.DeserializeObject<List<Bill>>(apiData);
            return View(bill); // truyền list vừa nhận được sang view
        }

        public async Task<IActionResult> Details(int id)
        {
            string requestURL = $"https://localhost:7055/api/Bills/{id}";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var bill = JsonConvert.DeserializeObject<Bill>(apiData);
            return View(bill); // truyền bill vừa nhận được sang view
        }

        public async Task<IActionResult> Create(Bill bill)
        {
            string requestURL = "https://localhost:7055/api/Bills";
            var content = new StringContent(JsonConvert.SerializeObject(bill), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }

        public async Task<IActionResult> Edit(int id, Bill bill)
        {
            string requestURL = $"https://localhost:7055/api/Bills/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(bill), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }

        public async Task<IActionResult> Delete(int id)
        {
            string requestURL = $"https://localhost:7055/api/Bills/{id}";
            var response = await _httpClient.DeleteAsync(requestURL);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
