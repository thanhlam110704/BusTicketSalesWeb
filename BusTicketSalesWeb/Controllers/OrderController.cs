using BusTicketSalesWeb.ViewModels;
using BusTicketSalesWeb.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusTicketSalesWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly QLBANVEXEEntities5 _context;

        public OrderController()
        {
            _context = new QLBANVEXEEntities5();
        }
        // GET: Order
        [HttpGet]
        public ActionResult Index(string idCoach)
        {



            if (Session["Customer"] == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }


            // Tìm xe theo idCoach
            var coach = _context.Xes.FirstOrDefault(x => x.MaXe == idCoach);

            if (coach == null)
            {
                return HttpNotFound(); // Trả về 404 nếu không tìm thấy xe
            }

            // Lấy các ghế của xe đó và trạng thái ghế đã đặt
            var bookedSeats = _context.Ghes
                 .Where(g => g.MaXe == coach.MaXe)
                 .SelectMany(g => g.VeXes)
                 .Select(v => new
                 {
                     MaGhe = v.Ghe.MaGhe,
                     TenGhe = v.Ghe.TenGhe,
                     TrangThai = v.Ghe.TrangThai == 1 // Trạng thái ghế đã đặt
                 })
                 .ToList();

            // Lấy thông tin chuyến xe
            var viewModel = new CoachViewModel
            {
                MaXe = coach.MaXe,
                BookedSeats = bookedSeats.Select(s => new SeatViewModel
                {
                    MaGhe = s.MaGhe,
                    TenGhe = s.TenGhe,
                    TrangThai = s.TrangThai
                }),
                NoiDi = coach.XE_CHUYENXE.FirstOrDefault().ChuyenXe.TuyenXe.NoiDi, // Lấy Nơi đi từ XE_CHUYENXE
                NoiDen = coach.XE_CHUYENXE.FirstOrDefault().ChuyenXe.TuyenXe.NoiDen, // Lấy Nơi đến từ XE_CHUYENXE
                ThoiGianKhoiHanh = coach.XE_CHUYENXE.FirstOrDefault().ChuyenXe.NgayGioKhoiHanh, // Lấy Thời gian khởi hành từ XE_CHUYENXE
                LoaiXe = coach.LoaiXe
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult BookingTicket(BookingViewModel model)
        {
            if (Session["Customer"] == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            var customer = Session["Customer"] as TaiKhoan;
            var khach = Session["KhachHang"] as KhachHang;
            if (customer != null)
            {
                using (var db = new QLBANVEXEEntities5())
                {
                    khach = db.KhachHangs.FirstOrDefault(k => k.Id == customer.Id);
                }
            }

            var maKH = khach.MaKH;

            if (!ModelState.IsValid)
            {
                return View(model); // Trả về lại trang đặt vé với các lỗi
            }

            // Lấy mã hóa đơn cuối cùng từ cơ sở dữ liệu
            var lastInvoice = _context.HoaDons
                .OrderByDescending(h => h.MaHoaDon)
                .FirstOrDefault();

            // Tăng phần số của mã hóa đơn cuối cùng lên 1
            int nextInvoiceNumber = 1;
            if (lastInvoice != null)
            {
                string lastInvoiceNumber = lastInvoice.MaHoaDon.Substring(2);
                nextInvoiceNumber = int.Parse(lastInvoiceNumber) + 1;
            }

            // Tạo mã hóa đơn mới
            string newInvoiceId = "HD" + nextInvoiceNumber.ToString("D3");

            // Tạo hóa đơn mới
            var invoice = new HoaDon
            {
                MaHoaDon = newInvoiceId,
                NgayGioTaoHoaDon = DateTime.Now,
                MaKH = maKH,
                MaNV = "NV002",
                TongTien = (int)model.giaVe, // Tổng tiền của tất cả vé
            };

            // Lưu hóa đơn vào cơ sở dữ liệu trước
            _context.HoaDons.Add(invoice);
            _context.SaveChanges();

            // Lấy mã vé cuối cùng từ cơ sở dữ liệu
            var lastTicket = _context.VeXes
                .OrderByDescending(v => v.MaVe)
                .FirstOrDefault();

            int nextTicketNumber = 1;
            if (lastTicket != null)
            {
                string lastTicketNumber = lastTicket.MaVe.Substring(2);
                nextTicketNumber = int.Parse(lastTicketNumber) + 1;
            }

            var maChuyen = _context.XE_CHUYENXE
              .Where(x => x.MaXe == model.MaXe)
              .Select(x => x.MaChuyen)
              .FirstOrDefault();
            List<string> seats = JsonConvert.DeserializeObject<List<string>>(model.gheDat);

            // Duyệt qua danh sách các ghế và tạo vé tương ứng
            foreach (var tenghe in seats)
            {
                var seat = _context.Ghes.FirstOrDefault(g => g.TenGhe == tenghe && g.MaXe == model.MaXe);
                if (seat == null || seat.TrangThai == 1)
                {
                    return new HttpStatusCodeResult(400, "Ghế đã được đặt hoặc không tồn tại");
                }

                // Tạo mã vé mới
                string newTicketId = "VE" + nextTicketNumber.ToString("D3");
                nextTicketNumber++;

                var ticket = new VeXe
                {

                    MaVe = newTicketId,
                    Gia = 100000,
                    QuyDinh = "Qui Dinh 1",
                    NgayXuatVe = DateTime.Now,
                    MaHoaDon = invoice.MaHoaDon,
                    MaGhe = seat.MaGhe,
                    MaChuyen = maChuyen

                };

                seat.TrangThai = 1;

                _context.VeXes.Add(ticket);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        // Ghi chi tiết lỗi vào log hoặc hiển thị lỗi
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                throw;
            }

            return RedirectToAction("PaySuccess");
        }





        public ActionResult PayFail()
        {
            return View();
        }
        public ActionResult PaySuccess()
        {
            return View();
        }
    }
}