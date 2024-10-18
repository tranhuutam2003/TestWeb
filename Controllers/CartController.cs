
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWeb.Data;
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

        private string GetCurrentUserPhoneNumber()
        {
            return HttpContext.Session.GetString("PhoneNumber");
        }

        public async Task<IActionResult> Index()
        {
            var phoneNumber = GetCurrentUserPhoneNumber();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _context.ShoppingCart
                .Include(c => c.Book)
                .Where(c => c.PhoneNumber == phoneNumber)
                .ToListAsync();

            return View(cartItems);
        }

        public async Task<IActionResult> AddToCart(int bookId)
        {
            var phoneNumber = GetCurrentUserPhoneNumber();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.BookID == bookId);

            if (cartItem == null)
            {
                cartItem = new ShoppingCart
                {
                    PhoneNumber = phoneNumber,
                    BookID = bookId,
                    Quantity = 1
                };
                _context.ShoppingCart.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("BookList", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int bookId)
        {
            var phoneNumber = GetCurrentUserPhoneNumber();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.BookID == bookId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int bookId)
        {
            var phoneNumber = GetCurrentUserPhoneNumber();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.BookID == bookId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    _context.ShoppingCart.Remove(cartItem);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(int bookId)
        {
            var phoneNumber = GetCurrentUserPhoneNumber();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItem = await _context.ShoppingCart
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.BookID == bookId);

            if (cartItem != null)
            {
                _context.ShoppingCart.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
