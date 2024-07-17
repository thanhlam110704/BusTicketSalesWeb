using System;
using System.Collections.Generic;
using BusTicketSalesWeb.Models;

namespace BusTicketSalesWeb.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<ChuyenXe> ChuyenXes { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string NoiDi { get; set; }
        public string NoiDen { get; set; }
        public DateTime? NgayKhoiHanh { get; set; }
        public bool IsNotFound { get; set; } // Thuộc tính mới để chỉ thị nếu không có kết quả
    }
}