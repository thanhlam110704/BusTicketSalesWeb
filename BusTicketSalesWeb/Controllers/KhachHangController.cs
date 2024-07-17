using BusTicketSalesWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Controllers
{
    public class KhachHangController : Controller
    {
        public QLBANVEXEEntities6 db = new QLBANVEXEEntities3();

        // Phương thức để hiển thị thông tin khách hàng
        [HttpGet]
        public async Task<ActionResult> InfomationCus(int account_Id)
        {
            var customer = await db.KhachHangs.Include(b => b.TaiKhoan).FirstOrDefaultAsync(b => b.Id == account_Id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.AccountId = account_Id;

            return View(customer);
        }


        [HttpPost]
        public async Task<ActionResult> InfomationCus(KhachHang model, int account_Id)
        {
            // Lấy thông tin khách hàng từ cơ sở dữ liệu
            var customer = await db.KhachHangs.Include(b => b.TaiKhoan).FirstOrDefaultAsync(b => b.Id == account_Id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // Cập nhật thông tin từ form vào đối tượng khách hàng từ database
                customer.TenKH = model.TenKH;
                customer.Gioi_Tinh = model.Gioi_Tinh;
                customer.Ngay_Sinh = model.Ngay_Sinh;

                // Kiểm tra và cập nhật số điện thoại (nếu có)
                if (customer.TaiKhoan != null)
                {
                    customer.TaiKhoan.Sdt = model.TaiKhoan?.Sdt;
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await db.SaveChangesAsync();

                // Thêm thông báo cập nhật thành công vào TempData
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";

                // Redirect lại đến action InfomationCus với thông tin tài khoản đã cập nhật
                return RedirectToAction("InfomationCus", new { account_Id = customer.Id });
            }

            // Nếu ModelState.IsValid không thành công, trả về lại View với model để hiển thị lỗi
            return View(model);
        }




    }

}