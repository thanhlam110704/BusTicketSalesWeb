using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTicketSalesWeb.Areas.ViewModel
{
    public class KhachHangListViewModel
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public byte? Gioi_Tinh { get; set; }
        public DateTime? Ngay_Sinh { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }
        public string MatKhau { get; set; }
    }
}