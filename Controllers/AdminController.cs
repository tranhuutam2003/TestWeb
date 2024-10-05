using Microsoft.AspNetCore.Mvc;
using TestWeb.Models;
using TestWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestWeb.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace TestWeb.Controllers
{
    [Authentication(1)]
    public class AdminController : Controller
    {

        private readonly BookContext _context;

        // Sử dụng Dependency Injection để nhận BookContext
        public AdminController(BookContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var listBook = _context.Books.ToList();
            return View(listBook);
        }

        // Hiển thị toàn bộ danh sách sách
        public IActionResult AllBooks(int? page)
        {
            int pageSize = 10; // Số lượng sách hiển thị trên mỗi trang
            int pageNumber = page ?? 1; // Nếu page là null thì sẽ là trang 1

            var books = _context.Books.Include(b => b.Category).ToPagedList(pageNumber, pageSize);
            return View(books);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult AddBook(Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);  // Thêm sách mới vào DbContext
                _context.SaveChanges();    // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction("Index");  // Chuyển hướng về trang danh sách
            }
            return View(book);
        }

        // Sửa sản phẩm - Hiển thị form sửa
        [HttpGet]
        public IActionResult EditBook(int id)
        {
            var book = _context.Books.Find(id); // Tìm sách theo id
            if (book == null)
            {
                return NotFound(); // Nếu không tìm thấy sách, trả về NotFound
            }
            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View(book); // Hiển thị form chỉnh sửa với thông tin sách
        }

        // Sửa sản phẩm - Xử lý khi người dùng submit form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBook(Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Update(book); // Cập nhật thông tin sách
                _context.SaveChanges();      // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction("Index");
            }
            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View(book);  // Nếu không hợp lệ, hiển thị lại form với dữ liệu hiện tại
        }

        // Xóa sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.Find(id); // Tìm sách theo id
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book); // Xóa sách khỏi DbContext
            _context.SaveChanges();      // Lưu thay đổi vào cơ sở dữ liệu
            return RedirectToAction("Index");
        }

        // Xem doanh thu (bỏ comment nếu cần sử dụng)
        // public IActionResult Revenue()
        // {
        //     var revenues = _context.Revenues.ToList();
        //     return View(revenues);
        // }
    }
}
