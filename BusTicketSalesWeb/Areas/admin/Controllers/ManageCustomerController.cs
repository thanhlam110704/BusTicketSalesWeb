using BusTicketSalesWeb.Areas.ViewModel;
using BusTicketSalesWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Areas.admin.Controllers
{
    public class ManageCustomerController : Controller
    {
        private QLBANVEXEEntities db = new QLBANVEXEEntities();
        // GET: admin/ManageCustomer
        public ActionResult Customers()
        {
            var khachHangs = db.KhachHangs.Include("TaiKhoan").ToList();

            // Chuyển đổi dữ liệu thành ViewModel
            var viewModel = khachHangs.Select(k => new KhachHangListViewModel
            {
                MaKH = k.MaKH,
                TenKH = k.TenKH,
                Gioi_Tinh = k.Gioi_Tinh,
                Ngay_Sinh = k.Ngay_Sinh,
                Email = k.TaiKhoan.Email,
                Sdt = k.TaiKhoan.Sdt,
                MatKhau = k.TaiKhoan.MatKhau
            }). ToList();
            return View(viewModel);
            //var cus = db.KhachHangs.ToList();

            //return View(cus);
        }
        public ActionResult EditCustomer(int id)
        {
            var customer = db.KhachHangs.Find(id);
            ViewBag.Gioi_Tinh = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Nam" },
                new SelectListItem { Value = "0", Text = "Nữ" }
            };
            ViewBag.Ngay_Sinh = customer.Ngay_Sinh?.ToString("dd/MM/yyyy");
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(KhachHangListViewModel customer)
        {
            
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.Include("TaiKhoan").FirstOrDefault(k => k.MaKH == customer.MaKH);

                if (khachHang == null)
                {
                    return HttpNotFound();
                }

                khachHang.TenKH = customer.TenKH;
                khachHang.Gioi_Tinh = customer  .Gioi_Tinh;
                khachHang.Ngay_Sinh = customer.Ngay_Sinh;
                //khachHang.TaiKhoan.Email = customer.Email;
                //khachHang.TaiKhoan.Sdt = customer.Sdt;
                db.Entry(khachHang).State = EntityState.Modified;
                db.Entry(khachHang.TaiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Customers"); // Hoặc tên của view mà bạn muốn điều hướng đến
            }
            return View(customer);
        }
       
    }
}