using System.Text;
using API.DOT;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PolyShopView.Controllers.Bill
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
            var bill = JsonConvert.DeserializeObject<List<API.Models.Bill>>(apiData);
            return View(bill); // truyền list vừa nhận được sang view
        }

        public async Task<IActionResult> GetBillDOT()
        {
            string requestURL = "https://localhost:7055/api/Bills/hoadon";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var billDot = JsonConvert.DeserializeObject<List<BillDOT>>(apiData);

            return View("HoadonView", billDot); // Ensure "HoadonView.cshtml" exists
        }

        public async Task<IActionResult> Details(int id)
        {
            string requestURL = $"https://localhost:7055/api/Bills/{id}";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var bill = JsonConvert.DeserializeObject<API.Models.Bill>(apiData);
            return View(bill); // truyền bill vừa nhận được sang view
        }

        public async Task<IActionResult> Create(API.Models.Bill bill)
        {
            string requestURL = "https://localhost:7055/api/Bills";
            var content = new StringContent(JsonConvert.SerializeObject(bill), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(GetBillDOT));
            }
            return View(bill);
        }

        public async Task<IActionResult> CreateBillDot(OrderViewModel billDot)
        {
            string requestURL = "https://localhost:7055/api/Bills/create-bill";
            var content = new StringContent(JsonConvert.SerializeObject(billDot), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestURL, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                try
                {
                    var createdBill = JsonConvert.DeserializeObject<API.Models.Bill>(responseBody);

                    if (createdBill != null && createdBill.BillId > 0)
                    {
                        return RedirectToAction("CreateBillDetail", "BillDetails", new { billId = createdBill.BillId });
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("JSON Parsing Error: " + ex.Message);
                }
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Error: " + errorMessage);
            }

            //ModelState.AddModelError(string.Empty, "Failed to create the bill.");
            return View(billDot);
        }



        public async Task<IActionResult> Edit(int id, API.Models.Bill bill)
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
