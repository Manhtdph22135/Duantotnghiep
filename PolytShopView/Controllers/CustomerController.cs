using API.Context;
using API.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var HttpClient = new HttpClient();
            var response = await HttpClient.GetAsync(requestURL);
            string apiData = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<List<Customer>>(apiData);
            return View(customer); // truyền list vừa nhận được sang view
        }
    }
}
