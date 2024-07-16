using BusTicketSalesWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Areas.admin.Controllers
{
    public class ReportsController : Controller
    {
        private QLBANVEXEEntities db = new QLBANVEXEEntities();

        public ActionResult Index()
        {
            var revenueData = db.HoaDons
            .Where(h => h.NgayGioTaoHoaDon.HasValue)
            .GroupBy(h => new { h.NgayGioTaoHoaDon.Value.Year, h.NgayGioTaoHoaDon.Value.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalRevenue = g.Sum(h => h.TongTien ?? 0)
            })
            .ToList();

            return View(revenueData);
        }
    }
}