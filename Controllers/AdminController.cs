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
        public async Task<IActionResult> AddBook(Books book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageFile != null)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "products");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    // Tạo tên file duy nhất
                    var fileName = Path.GetFileNameWithoutExtension(book.ImageFile.FileName);
                    var extension = Path.GetExtension(book.ImageFile.FileName);
                    book.ImageURL = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    var filePath = Path.Combine(imagePath, book.ImageURL);

                    try
                    {
                        // Lưu ảnh vào thư mục
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await book.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TempData["ErrorMessage"] = $"Error saving the image: {ex.Message}";
                        return RedirectToAction("AddBook"); // Hoặc trang bạn muốn quay lại
                    }
                }
                else
                {
                    //TempData["ErrorMessage"] = "No image file was uploaded.";
                    return RedirectToAction("AddBook"); // Hoặc trang bạn muốn quay lại
                }

                try
                {
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //TempData["ErrorMessage"] = $"Error saving the book: {ex.Message}";
                    return RedirectToAction("AddBook"); // Hoặc trang bạn muốn quay lại
                }
            }

            ViewBag.CategoryList = new SelectList(_context.Categories, "CategoryID", "CategoryName");
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
        public async Task<IActionResult> EditBook(Books book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageFile != null)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "products");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    // Tạo tên file duy nhất
                    var fileName = Path.GetFileNameWithoutExtension(book.ImageFile.FileName);
                    var extension = Path.GetExtension(book.ImageFile.FileName);
                    book.ImageURL = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    var filePath = Path.Combine(imagePath, book.ImageURL);

                    try
                    {
                        // Lưu ảnh vào thư mục
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await book.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TempData["ErrorMessage"] = $"Error saving the image: {ex.Message}";
                        return RedirectToAction("EditBook", new { id = book.BookID }); // Hoặc trang bạn muốn quay lại
                    }
                }

                // Cập nhật thông tin sách
                _context.Books.Update(book); // Cập nhật thông tin sách
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
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

            // Xóa sách khỏi DbContext
            _context.Books.Remove(book);
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            return RedirectToAction("AllBooks");
        }

        // Xem doanh thu (bỏ comment nếu cần sử dụng)
        // public IActionResult Revenue()
        // {
        //     var revenues = _context.Revenues.ToList();
        //     return View(revenues);
        // }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
