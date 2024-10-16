using Microsoft.AspNetCore.Mvc;
using TestWeb.Data;
using TestWeb.Extensions;
using TestWeb.Models;

namespace TestWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly BookContext _context;

        public CartController(BookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy giỏ hàng từ session
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            return View(cart);
        }

        public IActionResult AddToCart(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            cart.AddItem(book, 1);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("BookList", "Product");
        }
        //public IActionResult IncreaseQuantity(int bookId)
        //{
        //    // Lấy giỏ hàng từ session
        //    var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();

        //    // Tìm sản phẩm
        //    var book = _context.Books.Find(bookId);
        //    if (book != null)
        //    {
        //        cart.AddItem(book, 1); // Tăng số lượng
        //        HttpContext.Session.SetObjectAsJson("Cart", cart); // Lưu lại giỏ hàng
        //    }

        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public IActionResult IncreaseQuantity(int bookId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();

            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                cart.AddItem(book, 1);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int bookId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();

            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                var item = cart.Items.Find(i => i.Book.BookID == bookId);
                if (item != null && item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    cart.RemoveItem(bookId);
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        //public IActionResult DecreaseQuantity(int bookId)
        //{
        //    // Lấy giỏ hàng từ session
        //    var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();

        //    // Tìm sản phẩm
        //    var book = _context.Books.Find(bookId);
        //    if (book != null)
        //    {
        //        // Giảm số lượng nếu lớn hơn 1
        //        var item = cart.Items.Find(i => i.Book.BookID == bookId);
        //        if (item != null && item.Quantity > 1)
        //        {
        //            item.Quantity--;
        //        }
        //        else
        //        {
        //            cart.RemoveItem(bookId); // Nếu số lượng = 1 thì xóa sản phẩm
        //        }

        //        HttpContext.Session.SetObjectAsJson("Cart", cart); // Lưu lại giỏ hàng
        //    }

        //    return RedirectToAction("Index");
        //}

        public IActionResult RemoveFromCart(int bookId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            cart.RemoveItem(bookId);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }
    }
}
