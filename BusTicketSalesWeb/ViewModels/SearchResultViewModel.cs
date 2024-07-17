using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTicketSalesWeb.ViewModels
{
    public class SearchResultViewModel
    {
        public string MaChuyen { get; set; }
        public string TenChuyen { get; set; }
        public DateTime? NgayGioKhoiHanh { get; set; }
        public string NoiDi { get; set; }
        public string NoiDen { get; set; }
    }
}