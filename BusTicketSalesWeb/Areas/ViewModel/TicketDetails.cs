using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTicketSalesWeb.Areas.ViewModel
{
    public class TicketDetails
    {
        public string MaVe { get; set; }
        public string TenTuyen { get; set; }
        public DateTime? NgayGioKhoiHanh { get; set; }
        public string MaXe { get; set; }
        public byte? TrangThai { get; set; }
        public int? Gia { get; set; }
    }
}