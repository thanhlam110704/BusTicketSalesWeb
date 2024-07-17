using System;
using System.Collections.Generic;

namespace BusTicketSalesWeb.ViewModels
{
    public class CoachViewModel
    {
        public string MaXe { get; set; }
        public IEnumerable<SeatViewModel> BookedSeats { get; set; }
        public string NoiDi { get; set; }
        public string NoiDen { get; set; }
        public DateTime? ThoiGianKhoiHanh { get; set; }
        public byte? LoaiXe { get; set; }
    }
   
}