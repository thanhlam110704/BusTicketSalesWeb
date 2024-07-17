using BusTicketSalesWeb.Models;
using BusTicketSalesWeb.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Controllers
{
    public class SearchController : Controller
    {
        private readonly QLBANVEXEEntities5 _context;

        public SearchController()
        {
            _context = new QLBANVEXEEntities5(); // Đảm bảo tên context phù hợp với tên bạn đã đặt trong Entity Framework
        }

        [HttpPost]
        public ActionResult Search(string noiDi, string noiDen, DateTime? ngayKhoiHanh)
        {
            var viewModel = new
            {
                noiDi = noiDi,
                noiDen = noiDen,
                ngayKhoiHanh = ngayKhoiHanh
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("SearchResults", viewModel);
            }
            else
            {
                return View(viewModel);
            }
        }

        //[HttpGet]
        //public ActionResult SearchTrain(string noiDi, string noiDen, DateTime? ngayKhoiHanh, int? page)
        //{
        //    if (!(page.HasValue && page.Value != 0))
        //        page = 1;

        //    var chuyenXeQuery = _context.ChuyenXes.Include(cx => cx.TuyenXe).AsQueryable();

        //    if (!string.IsNullOrEmpty(noiDi))
        //    {
        //        chuyenXeQuery = chuyenXeQuery.Where(cx => cx.TuyenXe.NoiDi.Contains(noiDi));
        //    }

        //    if (!string.IsNullOrEmpty(noiDen))
        //    {
        //        chuyenXeQuery = chuyenXeQuery.Where(cx => cx.TuyenXe.NoiDen.Contains(noiDen));
        //    }

        //    if (ngayKhoiHanh.HasValue)
        //    {
        //        DateTime ngayKhoiHanhDate = ngayKhoiHanh.Value.Date;
        //        chuyenXeQuery = chuyenXeQuery.Where(cx => DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == ngayKhoiHanhDate);
        //    }

        //    // Phân trang
        //    int pageSize = 1; // Số lượng chuyến xe trên mỗi trang
        //    int totalItems = chuyenXeQuery.Count();

        //    var chuyenXeList = chuyenXeQuery
        //     .OrderBy(cx => cx.MaChuyen)
        //     .Skip((int)((page - 1) * pageSize))
        //     .Take(pageSize)
        //     .ToList();

        //    int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        //    var viewModel = new SearchViewModel
        //    {
        //        ChuyenXes = chuyenXeList,
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = (int)page,
        //            ItemsPerPage = pageSize,
        //            TotalItems = totalItems,
        //            TotalPages = totalPages

        //        },
        //        NoiDi = noiDi,
        //        NoiDen = noiDen,
        //        NgayKhoiHanh = ngayKhoiHanh,
        //        IsNotFound = !chuyenXeList.Any() // Kiểm tra nếu không có kết quả
        //    };

        //    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    {
        //        return PartialView("SearchResults", viewModel);
        //    }

        //    return View(viewModel);
        //}

        //[HttpGet]
        //public ActionResult SortAndFilter(string noiDi, string noiDen, DateTime? ngayKhoiHanh, string sort, int? page)
        //{
        //    if (!page.HasValue || page.Value == 0)
        //        page = 1;

        //    var chuyenXeQuery = _context.ChuyenXes.Include(cx => cx.TuyenXe).AsQueryable();

        //    // Apply filters
        //    if (!string.IsNullOrEmpty(noiDi))
        //    {
        //        chuyenXeQuery = chuyenXeQuery.Where(cx => cx.TuyenXe.NoiDi.Contains(noiDi));
        //    }

        //    if (!string.IsNullOrEmpty(noiDen))
        //    {
        //        chuyenXeQuery = chuyenXeQuery.Where(cx => cx.TuyenXe.NoiDen.Contains(noiDen));
        //    }

        //    if (ngayKhoiHanh.HasValue)
        //    {
        //        DateTime ngayKhoiHanhDate = ngayKhoiHanh.Value.Date;
        //        chuyenXeQuery = chuyenXeQuery.Where(cx => DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == ngayKhoiHanhDate);
        //    }

        //    // Sorting logic
        //    switch (sort)
        //    {
        //        case "time:asc":
        //            chuyenXeQuery = chuyenXeQuery.OrderBy(cx => cx.NgayGioKhoiHanh);
        //            break;
        //        case "time:desc":
        //            chuyenXeQuery = chuyenXeQuery.OrderByDescending(cx => cx.NgayGioKhoiHanh);
        //            break;
        //        default:
        //            chuyenXeQuery = chuyenXeQuery.OrderBy(cx => cx.MaChuyen); // Default sorting
        //            break;
        //    }

        //    // Pagination
        //    int pageSize = 1; // Number of items per page
        //    int totalItems = chuyenXeQuery.Count();

        //            var chuyenXeList = chuyenXeQuery
        //     .OrderBy(cx => cx.MaChuyen)
        //     .Skip((int)((page - 1) * pageSize))
        //     .Take(pageSize)
        //     .ToList();


        //    // Calculate total pages
        //    int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //    var viewModel = new SearchViewModel
        //    {
        //        ChuyenXes = chuyenXeList,
        //        NoiDi = noiDi,
        //        NoiDen = noiDen,
        //        NgayKhoiHanh = ngayKhoiHanh,
        //        IsNotFound = !chuyenXeList.Any(),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = (int)page,
        //            ItemsPerPage = pageSize,
        //            TotalItems = totalItems,
        //            TotalPages = totalPages
        //        }
        //    };

        //    return PartialView("SearchResultsSort", viewModel);
        //}



        [HttpGet]
        public ActionResult SearchTrain(string noiDi, string noiDen, DateTime? ngayKhoiHanh, string sort, int? page)
        {
            if (!page.HasValue || page.Value == 0)
                page = 1;

            var chuyenXeQuery = _context.ChuyenXes.Include(cx => cx.TuyenXe).AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(noiDi))
            {
                chuyenXeQuery = chuyenXeQuery.Where(cx => cx.TuyenXe.NoiDi.Contains(noiDi));
            }

            if (!string.IsNullOrEmpty(noiDen))
            {
                chuyenXeQuery = chuyenXeQuery.Where(cx => cx.TuyenXe.NoiDen.Contains(noiDen));
            }

            if (ngayKhoiHanh.HasValue)
            {
                DateTime ngayKhoiHanhDate = ngayKhoiHanh.Value.Date;
                chuyenXeQuery = chuyenXeQuery.Where(cx => DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == ngayKhoiHanhDate);
            }

            // Sorting logic
            switch (sort)
            {
                case "time:asc":
                    chuyenXeQuery = chuyenXeQuery.OrderBy(cx => cx.NgayGioKhoiHanh);
                    break;
                case "time:desc":
                    chuyenXeQuery = chuyenXeQuery.OrderByDescending(cx => cx.NgayGioKhoiHanh);
                    break;
                default:
                    chuyenXeQuery = chuyenXeQuery.OrderBy(cx => cx.MaChuyen); // Default sorting
                    break;
            }

            // Pagination
            int pageSize = 1; // Number of items per page
            int totalItems = chuyenXeQuery.Count();

            var chuyenXeList = chuyenXeQuery
                 .Skip((int)((page - 1) * pageSize))
                 .Take(pageSize)
                 .ToList();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var viewModel = new SearchViewModel
            {
                ChuyenXes = chuyenXeList,
                NoiDi = noiDi,
                NoiDen = noiDen,
                NgayKhoiHanh = ngayKhoiHanh,
                IsNotFound = !chuyenXeList.Any(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = (int)page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                }
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("SearchResults", viewModel);
            }

            return View("SearchTrain", viewModel);
        }

    }
}
