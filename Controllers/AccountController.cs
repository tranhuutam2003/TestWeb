﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TestWeb.Models;
using TestWeb.Data; 
using System.Linq;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;

namespace TestWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookContext _context;
        private const string VerificationCodeKey = "_VerificationCode";
        private const string EmailForVerification = "_EmailForVerification";

        public AccountController(BookContext context)
        {
            _context = context;
        }

        // Trang hiển thị form Reset Password
        public IActionResult ResetPassword()
        {
            return View();
        }

        // Gửi mã xác nhận qua email
        [HttpPost]
        public IActionResult SendVerificationEmail(string email)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                return Json(new { success = false, message = "Email đã tồn tại trong hệ thống." });
            }

            string verificationCode = GenerateVerificationCode();

            // Lưu mã xác nhận và email vào TempData
            TempData[VerificationCodeKey] = verificationCode;
            TempData[EmailForVerification] = email;

            SendEmail(email, verificationCode);

            return Json(new { success = true, message = "Mã xác nhận đã được gửi thành công." });
        }

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        // Gửi email với mã xác nhận
        private void SendEmail(string email, string code)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("tam62533@gmail.com", "dotnnpjevidbdxjr"), // Sử dụng mật khẩu ứng dụng nếu cần
                    EnableSsl = true,
                };

                smtpClient.Send("tam62533@gmail.com", email, "Mã xác nhận", $"Mã xác nhận của bạn là: {code}");
            }
            catch (SmtpException smtpEx)
            {
                ViewBag.Message = $"Lỗi SMTP: {smtpEx.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Có lỗi xảy ra khi gửi email: {ex.Message}";
            }
        }

        // Form nhập mã xác nhận và mật khẩu mới
        public IActionResult ConfirmCode()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        // Xác nhận mã và cập nhật mật khẩu
        [HttpPost]
        public IActionResult ConfirmCode(string enteredCode, string newPassword, string confirmPassword)
        {
            string storedCode = TempData[VerificationCodeKey] as string;
            string email = TempData[EmailForVerification] as string;

            // Giữ lại giá trị trong TempData để có thể sử dụng lại nếu cần
            TempData.Keep(VerificationCodeKey);
            TempData.Keep(EmailForVerification);

            if (string.IsNullOrEmpty(storedCode) || storedCode != enteredCode)
            {
                ModelState.AddModelError("", "Mã xác nhận không đúng hoặc đã hết hạn.");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu không khớp.");
                return View();
            }

            UpdatePassword(email, newPassword);

            // Xóa mã xác nhận và email khỏi TempData sau khi sử dụng thành công
            TempData.Remove(VerificationCodeKey);
            TempData.Remove(EmailForVerification);

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            return RedirectToAction("Login");
        }

        // Cập nhật mật khẩu trong cơ sở dữ liệu
        private void UpdatePassword(string email, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Password = newPassword; // Lưu ý: Nên mã hóa mật khẩu trước khi lưu
                _context.SaveChanges();
            }
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
                HttpContext.Session.SetString("FullName", existingUser.FullName);
                HttpContext.Session.SetString("PhoneNumber", existingUser.PhoneNumber);

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
        public IActionResult Register(User user, string VerificationCode)
        {
            string storedCode = TempData[VerificationCodeKey] as string;
            string email = TempData[EmailForVerification] as string;

            if (string.IsNullOrEmpty(storedCode) || storedCode != VerificationCode)
            {
                ModelState.AddModelError("VerificationCode", "Mã xác nhận không đúng. Vui lòng thử lại.");
                // Giữ lại mã xác nhận trong TempData để có thể kiểm tra lại
                TempData.Keep(VerificationCodeKey);
                TempData.Keep(EmailForVerification);
                return View(user);
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra lại email một lần nữa để đảm bảo
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống.");
                    return View(user);
                }

                // Lưu người dùng vào database
                user.Role = 0; // Mặc định là khách hàng
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            return View(user);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
