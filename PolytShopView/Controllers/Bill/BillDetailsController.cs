using System.Text;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PolyShopView.Controllers.Bill
{
    public class BillDetailsController : Controller
    {
        private readonly HttpClient _httpClient;

        public BillDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            string requestURL = "https://localhost:7055/api/BillDetails";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var billDetail = JsonConvert.DeserializeObject<List<BillDetail>>(apiData);
            return View(billDetail); // truyền list vừa nhận được sang view
        }

        public async Task<IActionResult> Details(int id)
        {
            string requestURL = $"https://localhost:7055/api/BillDetails/{id}";
            var response = await _httpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var billDetail = JsonConvert.DeserializeObject<BillDetail>(apiData);
            return View(billDetail); // truyền billDetail vừa nhận được sang view
        }

        public IActionResult CreateBillDetail(int billId)
        {
            ViewBag.BillId = billId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBillDetail(BillDetail billDetail)
        {
            string requestURL = "https://localhost:7055/api/BillDetails/create-bill-details";
            var content = new StringContent(JsonConvert.SerializeObject(billDetail), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestURL, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetBillDOT", "Bills");
            }
            return View(billDetail);
        }


        public async Task<IActionResult> Edit(int id, BillDetail billDetail)
        {
            string requestURL = $"https://localhost:7055/api/BillDetails/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(billDetail), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(billDetail);
        }

        public async Task<IActionResult> Delete(int id)
        {
            string requestURL = $"https://localhost:7055/api/BillDetails/{id}";
            var response = await _httpClient.DeleteAsync(requestURL);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
