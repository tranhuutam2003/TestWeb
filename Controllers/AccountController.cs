using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TestWeb.Models;
using TestWeb.Data; 
using System.Linq;

namespace TestWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookContext _context;

        public AccountController(BookContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            
            //return View();
            return PartialView();
        }

        [HttpPost]
        
        public IActionResult Login(User user)
        {
            // Kiểm tra user có tồn tại trong cơ sở dữ liệu không
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            if (existingUser != null)
            {
                // Lưu thông tin phiên đăng nhập
                HttpContext.Session.SetString("Username", existingUser.Email);
                HttpContext.Session.SetInt32("Role", existingUser.Role);


                // Kiểm tra phân quyền
                if (existingUser.Role == 1) // Admin
                {
                    return RedirectToAction("Index", "Admin"); // Đưa về trang quản lý của admin
                }
                else // Khách hàng
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Message = "Invalid login attempt";
            return View();
        }

        public IActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Password))
            {
                // Kiểm tra user đã tồn tại chưa
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser == null)
                {
                    // Mặc định Role = 0 (Khách hàng)
                    user.Role = 0;

                    // Lưu user mới vào cơ sở dữ liệu
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Message = "Username already exists";
                    return View();
                }
            }

            ViewBag.Message = "Please enter username and password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
