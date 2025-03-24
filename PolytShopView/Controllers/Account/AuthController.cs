using API.Context;
using Microsoft.AspNetCore.Mvc;

namespace PolyShopView.Controllers.Account
{
    public class AuthController : Controller
    {
        private readonly DbContextShop _contextShop;

        public AuthController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
        }

        
    }
}
