using Microsoft.AspNetCore.Mvc;
using TestWeb.Models;
using TestWeb.Services;

namespace TestWeb.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("items")]
        public IActionResult GetCartItems()
        {
            var items = _cartService.GetCartItems();
            return View("Index", items);  // Trả về view Index.cshtml
        }

        [HttpPost("add")]
        public IActionResult AddToCart(Product product, int quantity)
        {
            _cartService.AddToCart(product, quantity);
            return RedirectToAction("GetCartItems");  // Redirect đến giỏ hàng sau khi thêm sản phẩm
        }

        [HttpPost("remove/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("GetCartItems");
        }

        [HttpGet("total")]
        public IActionResult GetTotalPrice()
        {
            var total = _cartService.GetTotalPrice();
            return View("Total", total);  // Trả về view Total.cshtml
        }


    }
}
