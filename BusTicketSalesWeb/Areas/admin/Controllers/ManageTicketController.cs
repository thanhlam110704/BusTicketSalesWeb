using BusTicketSalesWeb.Areas.ViewModel;
using BusTicketSalesWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Areas.admin.Controllers
{
    public class ManageTicketController : Controller
    {
        // GET: admin/ManageTicket
        QLBANVEXEEntities db = new QLBANVEXEEntities();
        public ActionResult Index()
        {
            // Lấy dữ liệu từ cơ sở dữ liệu
            var ticketDetails = db.VeXes
                .Select(v => new TicketDetails
                {
                    MaVe = v.MaVe,
                    TenTuyen = v.ChuyenXe.TuyenXe.TenTuyen,
                    NgayGioKhoiHanh = v.ChuyenXe.NgayGioKhoiHanh,
                    MaXe = v.ChuyenXe.XE_CHUYENXE.FirstOrDefault(x => x.MaChuyen == v.MaChuyen).MaXe,
                    TrangThai = v.ChuyenXe.XE_CHUYENXE.FirstOrDefault(x => x.MaChuyen == v.MaChuyen).Xe.LoaiXe,  // Giả sử bạn dùng `LoaiXe` để chỉ trạng thái vé
                    Gia = v.Gia
                })
                .ToList();

            // Trả về View với dữ liệu
            return View(ticketDetails);
        }
    }
}