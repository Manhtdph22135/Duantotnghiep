﻿using API.Context;
using API.DOT;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class TrangchuController : Controller
    {
        private readonly DbContextShop _contextShop;

        public TrangchuController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
        }
        [HttpGet("trang-chu")]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetProductDetailsHome()
        {
            var query = from p in _contextShop.Products
                join pd in _contextShop.ProductDetails on p.ProductId equals pd.ProductId
                select new HomeDOT()
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = pd.Image,
                };
            return Ok(await query.ToListAsync());
        }
        [HttpGet("trang-chu/{category}")]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetProductDetailsHomeWithCategory(string category)
        {
            var query = from p in _contextShop.Products
                join pd in _contextShop.ProductDetails on p.ProductId equals pd.ProductId
                        join pc in _contextShop.ProductCategories on p.CategoryId equals pc.CategoryId
                        where pc.CategoryName == category
                        select new HomeDOT()
                        {
                            ProductID = p.ProductId,
                            ProductName = p.ProductName,
                            Price = p.Price,
                            Image = pd.Image,
                        };
            return Ok(await query.ToListAsync());
        }
    }
}
