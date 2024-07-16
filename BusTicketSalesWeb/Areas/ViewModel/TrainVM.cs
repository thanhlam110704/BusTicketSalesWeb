using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Areas.ViewModel
{
    public class TrainVM
    {
        public string MaChuyen { get; set; }
        public string TenChuyen { get; set; }
        public string MaTuyen { get; set; }
        public DateTime? NgayGioKhoiHanh { get; set; }
        [Required]
        public string MaXe { get; set; }
        public IEnumerable<SelectListItem> TuyenXes { get; set; }
        public IEnumerable<SelectListItem> XeChuyenXes { get; set; }
    }
}