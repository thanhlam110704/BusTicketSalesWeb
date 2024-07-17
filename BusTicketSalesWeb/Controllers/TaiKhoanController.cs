using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Principal;
using System.Web.Helpers;
using BusTicketSalesWeb.Models;

namespace BusTicketSalesWeb.Controllers
{
    public class TaiKhoanController : Controller
    {
       
        public QLBANVEXEEntities3 db = new QLBANVEXEEntities3();
       
       

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register( string Email, string Sdt, string MatKhau, string confirmPassword)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Sdt)|| string.IsNullOrEmpty(MatKhau) || string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("", "Thông tin đăng ký không hợp lệ! Vui lòng thử lại.");
                return View();
            }
            else if (IsExistedEmail(Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại! Vui lòng sử dụng một Email khác.");
                return View();
            }
            else if (MatKhau.Length < 6)
            {
                ModelState.AddModelError("", "Mật khẩu quá ngắn! Vui lòng sử dụng mật khẩu dài hơn 6 ký tự.");
                return View();
            }
            else if (!MatKhau.Equals(confirmPassword))
            {
                ModelState.AddModelError("", "Mật khẩu nhập lại không trùng khớp! Vui lòng nhập lại.");
                return View();
            }
            else if (Sdt.Length !=10 && ContainsSpecialCharacters(Sdt))
            {
                ModelState.AddModelError("", "Số điện thoại phải có đủ 10 ký tự và không chưa ký tự đặc biệt.");
                return View();
            }
            else if (!IsValidEmail(Email))
            {
                ModelState.AddModelError("", "Email không hợp lệ! Vui lòng sử dụng một Email hợp lệ.");
                return View();
            }
            else
            {
                TaiKhoan account = new TaiKhoan();
                account.Email = Email;
                account.MatKhau = MatKhau;
                account.VaiTro = 0;
                account.Sdt = Sdt;
              
                string token = Guid.NewGuid().ToString();
                Session["NewAccount"] = account;
                Session["ValidatedToken"] = token;

                string subject = "Xác thực tài khoản";
                string url = Url.Action("VerifyAccount", "TaiKhoan", new {token = token }, Request.Url.Scheme);
                string body = $"<p><i>BusTicketsSales xin chào,</i></p> <p><i>Vui lòng nhấp vào liên kết dưới đây để xác thực tài khoản:</i></p> <p><i>{url}</i></p>";


                SendEmail(Email, subject, body);

                TempData["Message"] = "Chúng tôi đã gửi mã xác nhận đến email của bạn. Vui lòng kiểm tra hộp thư để tiếp tục đăng ký tài khoản";
                TempData["css"] = "text-success";

                return RedirectToAction("Login", "TaiKhoan");
            }
        }
      
        // Kiểm tra xem email có hợp lệ không
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        // Kiểm tra xem email có tồn tại không

        private bool IsExistedEmail(string email)
        {
            var account = db.TaiKhoans.FirstOrDefault(a => a.Email == email);

            return (account != null) ? true : false;
        }
        // Kiểm tra các ký tự đặc biệt
        private static bool ContainsSpecialCharacters(string text)
        {
            // Danh sách các ký tự đặc biệt
            const string specialCharacters = "!@#$%^&*()_+{}|:\"<>?,./";

            // Kiểm tra xem text có chứa các ký tự đặc biệt hay không
            for (int i = 0; i < text.Length; i++)
            {
                if (specialCharacters.Contains(text[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public ActionResult Login()
        {

            return View();
        }
        // Khi login thành công nếu như account là custommer thì layout signin/ signup sẽ thay đổi thành tên người dùng và sẽ có hiển thị các chức năng của custommer
        [HttpPost]
        public ActionResult Login(string Email, string MatKhau)
        {
            TaiKhoan customer = IsValidCustomer(Email, MatKhau);
            if (customer != null)
            {
                Session["Customer"] = customer;
                return RedirectToAction("Index", "Home");
            }
            else if (IsValidAdmin(Email, MatKhau))
            {
                FormsAuthentication.SetAuthCookie(Email, false);

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                ModelState.AddModelError("", "Thông tin đăng nhập không hợp lệ! Vui lòng thử lại.");
                return View();
            }
        }
        public ActionResult VerifyAccount(string token)
        {
            TaiKhoan account = (TaiKhoan)Session["NewAccount"];
            string validatedToken = (string)Session["ValidatedToken"];
            if (account != null && token == validatedToken)
            {
                string hashedPassword = BCryptNet.HashPassword(account.MatKhau);

                account.MatKhau = hashedPassword;
                db.TaiKhoans.Add(account);
                db.SaveChanges();

                Session.Remove("NewAccount");
                Session.Remove("ValidatedToken");

                ViewBag.Message = "Xác thực thành công!";
                ViewBag.Success = true;
                KhachHang kh = new KhachHang();
                return View();
            }
            else
            {
                ViewBag.Message = "Xác thực không thành công!";
                ViewBag.Success = false;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "TaiKhoan");
        }
        private TaiKhoan IsValidCustomer(string email, string password)
        {
            var customer = db.TaiKhoans.FirstOrDefault(a => a.Email == email && a.VaiTro == 0);

            if (customer != null && BCryptNet.Verify(password, customer.MatKhau))
            {
                return customer;
            }

            return null;
        }
       

        private bool IsValidAdmin(string email, string password)
        {
            var admin = db.TaiKhoans.FirstOrDefault(a => a.Email == email && a.VaiTro == 1);

            return (admin != null && BCryptNet.Verify(password, admin.MatKhau));
        }

        public ActionResult InformationCustomer()
        { 
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        private void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "ltthanh4104@gmail.com";
            var fromPassword = "eckk eiwv lkqc oqpn";
            var fromDisplayName = "BusTicketSales";

            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var fromAddress = new MailAddress(fromEmail, fromDisplayName);
            var toAddress = new MailAddress(toEmail);

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtpClient.Send(mailMessage);
        }
    }
}