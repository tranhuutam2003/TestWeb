using Microsoft.AspNetCore.Mvc;
using TestWeb.Models;
using TestWeb.Data;


namespace TestWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookContext _context;

        public AdminController(BookContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        // Thêm sản phẩm
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // Sửa sản phẩm
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Update(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // Xóa sản phẩm
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Xem doanh thu
        //public IActionResult Revenue()
        //{
        //    var revenues = _context.Revenues.ToList();
        //    return View(revenues);
        //}
    }

}
