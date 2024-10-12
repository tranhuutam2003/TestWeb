using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWeb.Data;
using TestWeb.Models;
using TestWeb.ViewModel;
using X.PagedList;

namespace TestWeb.Controllers
{
    public class ProductController : Controller
    {
        BookContext db = new BookContext();
        private readonly BookContext _context; // Đảm bảo BookContext được định nghĩa
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Apple", Category = "Fruits", Price = 1.5m },
            new Product { Id = 2, Name = "Banana", Category = "Fruits", Price = 1.0m },
            new Product { Id = 3, Name = "Carrot", Category = "Vegetables", Price = 2.0m },
            new Product { Id = 4, Name = "Laptop", Category = "Electronics", Price = 500.0m },
        };

        // Constructor để khởi tạo _context
        public ProductController(BookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string query)
        {
            var products = string.IsNullOrEmpty(query) ?
                _products :
                _products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            var viewModel = new ProductSearchViewModel
            {
                Query = query,
                Products = products
            };

            return View(viewModel);
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page ?? 1;

            var lstsanpham = _context.Books.AsNoTracking().OrderBy(x => x.Title);
            var lst = lstsanpham.ToPagedList(pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult SanPhamTheoLoai(int maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page ?? 1;

            var lstsanpham = _context.Books.AsNoTracking().Where(x => x.BookID == maloai).OrderBy(x => x.Title);
            var lst = lstsanpham.ToPagedList(pageNumber, pageSize);
            ViewBag.maloai = maloai;
            return View(lst);
        }


        //public IActionResult ChiTietSanPham(String maSp)
        //{
        //    var SanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
        //    var AnhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
        //    ViewBag.AnhSanPham = AnhSanPham;
        //    return View(SanPham);
        //}



        public IActionResult BookList(int? page)
        {
            int pageSize = 8;
            int pageNumber = page ?? 1;

            var books = _context.Books.Include(b => b.Category).ToPagedList(pageNumber, pageSize);
            return View(books);
        }

        public IActionResult CreateBook()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult CreateBook(Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Sách đã được lưu thành công!";
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public IActionResult CreateCategory()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Sách đã được lưu thành công!";
                return View(category);
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc thông báo
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu sách."); // Thêm lỗi vào ModelState
            }
            return View(category);
        }

        public IActionResult CategoryList()
        {
            var categories = _context.Categories.ToList();
            return View(categories);  // Trả về view hiển thị danh sách danh mục
        }

    }
}