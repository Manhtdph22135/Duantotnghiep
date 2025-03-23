using Microsoft.AspNetCore.Mvc;

namespace PolyShopView.Controllers.Product
{
    public class ProductDetailsController : Controller
    {
        public readonly HttpClient _httpClient;

        public ProductDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
