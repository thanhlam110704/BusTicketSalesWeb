using BusTicketSalesWeb.Areas.ViewModel;
using BusTicketSalesWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Areas.admin.Controllers
{
    public class ManageTrainController : Controller
    {
        // GET: admin/ManageTrain
        QLBANVEXEEntities db = new QLBANVEXEEntities();
        public ActionResult Index()
        {
            var train = db.ChuyenXes
              .Select(v => new TrainVM
              {
                  MaChuyen = v.MaChuyen,
                  MaTuyen = v.MaTuyen,
                  TenChuyen = v.TenChuyen,
                  NgayGioKhoiHanh = v.NgayGioKhoiHanh,
                  MaXe = v.XE_CHUYENXE.Select(x => x.Xe.MaXe).FirstOrDefault()

              })
              .ToList();

            return View(train);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenXe chuyenXe = db.ChuyenXes.Find(id);
            if (chuyenXe == null)
            {
                return HttpNotFound();
            }
            return View(chuyenXe);
        }

        public ActionResult Create()
        {
            ViewBag.MaTuyen = new SelectList(db.TuyenXes, "MaTuyen", "TenTuyen");
            ViewBag.MaXe = new SelectList(db.Xes, "MaXe", "TenXe");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrainVM model)
        {
            if (ModelState.IsValid)
            {
                var tuyenXe = db.TuyenXes.FirstOrDefault(t => t.MaTuyen == model.MaTuyen);
                if (tuyenXe == null)
                {
                    ModelState.AddModelError("", "Tuyến xe không tồn tại.");
                    ViewBag.MaTuyen = new SelectList(db.TuyenXes, "MaTuyen", "TenTuyen");
                    ViewBag.MaXe = new SelectList(db.Xes, "MaXe", "TenXe");
                    return RedirectToAction("Index");
                }
                var chuyenXe = new ChuyenXe
                {
                    MaChuyen = model.MaChuyen,
                    TenChuyen = model.TenChuyen, 
                    NgayGioKhoiHanh = model.NgayGioKhoiHanh,
                    MaTuyen = model.MaTuyen 
                };
                db.ChuyenXes.Add(chuyenXe);
                var xeChuyenXe = db.Xes.FirstOrDefault(x => x.MaXe == model.MaXe);
                //var xeChuyenXe = db.XE_CHUYENXE.FirstOrDefault(x => x.MaChuyen == model.MaChuyen);
                if (xeChuyenXe != null)
                {
                     var xe = new XE_CHUYENXE
                    {
                        MaChuyen = model.MaChuyen,                   
                        MaXe = model.MaXe
                    };
                    db.XE_CHUYENXE.Add(xe);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTuyen = new SelectList(db.TuyenXes, "MaTuyen", "TenTuyen");
            ViewBag.MaXe = new SelectList(db.Xes, "MaXe", "TenXe");
            return View(model);
        }
        //public string GetMaXe(string maChuyen)
        //{
        //    using (var context = new QLBANVEXEEntities())
        //    {
        //        var maXe = context.XE_CHUYENXE
        //                          .Where(x => x.MaChuyen == maChuyen)
        //                          .Select(x => x.MaXe)
        //                          .FirstOrDefault();
        //        return maXe;
        //    }
        //}
        //public void UpdateTrain(TrainVM train)
        //{
        //    using (var context = new QLBANVEXEEntities())
        //    {
        //        var chuyenXe = context.ChuyenXes
        //                              .FirstOrDefault(x => x.MaChuyen == train.MaChuyen);
        //        var xe_cx = context.XE_CHUYENXE.FirstOrDefault(c => c.MaChuyen == train.MaChuyen);
        //        if (chuyenXe != null)
        //        {
        //            chuyenXe.TenChuyen = train.TenChuyen;
        //            chuyenXe.MaTuyen = train.MaTuyen;
        //            chuyenXe.NgayGioKhoiHanh = train.NgayGioKhoiHanh;
        //            context.SaveChanges();
        //            if (xe_cx == null)
        //            {
        //                XE_CHUYENXE cx = new XE_CHUYENXE
        //                {
        //                    MaChuyen = train.MaChuyen,
        //                    MaXe = train.MaXe
        //                };
        //            }
        //            else
        //            {
        //                xe_cx.MaXe = train.MaXe;
        //            }
        //            context.SaveChanges();
        //        }
        //    }
        //}
        //public ActionResult Edit(string id)
        //{
        //    var maXe = GetMaXe(id);
        //    var chuyen_xe = db.ChuyenXes.Find(id);
        //    var tuyen_xe = db.ChuyenXes.Include("TuyenXe").FirstOrDefault(c => c.MaChuyen == id);
        //    var train = new TrainVM
        //    {
        //        MaChuyen = chuyen_xe.MaChuyen,
        //        TenChuyen = chuyen_xe.TenChuyen,
        //        MaTuyen = chuyen_xe.MaTuyen,
        //        MaXe = maXe,
        //    };
        //    ViewBag.SelectedMT =;
        //    ViewBag.SelectedMX = maXe;
        //    ViewBag.MaTuyen = new SelectList(db.TuyenXes, "MaTuyen", "TenTuyen");
        //    ViewBag.MaXe = new SelectList(db.Xes, "MaXe", "TenXe");
        //    return View(train);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(TrainVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UpdateTrain(model);
        //        return RedirectToAction("Index"); // Chuyển hướng sau khi cập nhật thành công
        //    }

        //    return RedirectToAction("Index");
        //}
        public ActionResult Edit(string id)
        {
            // Lấy dữ liệu từ cơ sở dữ liệu
            var chuyendi = db.ChuyenXes.Find(id);
            if (chuyendi == null)
            {
                return HttpNotFound();
            }
            var maXe = db.XE_CHUYENXE.Where(x => x.MaChuyen == id).Select(x => x.MaXe).FirstOrDefault();
            var viewModel = new TrainVM
            {
                MaChuyen = chuyendi.MaChuyen,
                TenChuyen = chuyendi.TenChuyen,
                MaTuyen = chuyendi.MaTuyen,
                NgayGioKhoiHanh = chuyendi.NgayGioKhoiHanh,
                MaXe = maXe,
                TuyenXes = db.TuyenXes.Select(t => new SelectListItem
                {
                    Value = t.MaTuyen,
                    Text = t.TenTuyen,
                    Selected = t.MaTuyen == chuyendi.MaTuyen
                }).ToList(),
                XeChuyenXes = db.Xes.Select(x => new SelectListItem
                {
                    Value = x.MaXe,
                    Text = x.MaXe,
                    Selected = x.MaXe == maXe
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrainVM model)
        {
            if (ModelState.IsValid)
            {
                var chuyendi = db.ChuyenXes.Find(model.MaChuyen);
                if (chuyendi == null)
                {
                    return HttpNotFound();
                }

                chuyendi.TenChuyen = model.TenChuyen;
                chuyendi.MaTuyen = model.MaTuyen;
                chuyendi.NgayGioKhoiHanh = model.NgayGioKhoiHanh;
                var xeChuyenXe = db.XE_CHUYENXE.FirstOrDefault(x => x.MaChuyen == model.MaChuyen);
                if (xeChuyenXe != null)
                {
                    xeChuyenXe.MaXe = model.MaXe;
                }
                else
                {
                    // Nếu chưa tồn tại, thêm mới
                    db.XE_CHUYENXE.Add(new XE_CHUYENXE { MaChuyen = model.MaChuyen, MaXe = model.MaXe });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            model.TuyenXes = db.TuyenXes.Select(t => new SelectListItem
            {
                Value = t.MaTuyen,
                Text = t.TenTuyen,
                Selected = t.MaTuyen == model.MaTuyen
            }).ToList();

            model.XeChuyenXes = db.Xes.Select(x => new SelectListItem
            {
                Value = x.MaXe,
                Text = x.MaXe,
                Selected = x.MaXe == model.MaXe
            }).ToList();

            return View(model);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenXe chuyenXe = db.ChuyenXes.Find(id);
            if (chuyenXe == null)
            {
                return HttpNotFound();
            }
            return View(chuyenXe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var xeTicket = db.VeXes.FirstOrDefault(c => c.MaChuyen ==  id);
            ChuyenXe chuyenXe = db.ChuyenXes.Find(id);
            if (chuyenXe == null)
            {
                return HttpNotFound();
            }
            if (xeTicket != null) 
            {
                db.VeXes.Remove(xeTicket);
            }
            db.ChuyenXes.Remove(chuyenXe);
            db.SaveChanges();
            return RedirectToAction("Index"); // Điều hướng sau khi xóa thành công
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}