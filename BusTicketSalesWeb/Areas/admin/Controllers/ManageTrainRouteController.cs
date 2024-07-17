using BusTicketSalesWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Areas.admin.Controllers
{
    public class ManageTrainRouteController : Controller
    {
        QLBANVEXEEntities db = new QLBANVEXEEntities();
        public ActionResult Index()
        {
            var tuyen_xe = db.TuyenXes.ToList();
            return View(tuyen_xe);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTuyen,TenTuyen")] TuyenXe tuyenXe)
        {
            if (ModelState.IsValid)
            {
                db.TuyenXes.Add(tuyenXe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tuyenXe);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuyenXe tuyenXe = db.TuyenXes.Find(id);
            if (tuyenXe == null)
            {
                return HttpNotFound();
            }
            return View(tuyenXe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTuyen,TenTuyen")] TuyenXe tuyenXe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuyenXe).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuyenXe);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuyenXe tuyenXe = db.TuyenXes.Find(id);
            if (tuyenXe == null)
            {
                return HttpNotFound();
            }
            return View(tuyenXe);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TuyenXe tuyenXe = db.TuyenXes.Find(id);
            if (tuyenXe == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra xem tuyến xe có tồn tại trong bảng ChuyenXe hay không
            bool hasChuyenXe = db.ChuyenXes.Any(cx => cx.MaTuyen == id);
            if (hasChuyenXe)
            {
                // Hiển thị thông báo lỗi
                ModelState.AddModelError("", "Không thể xóa tuyến xe vì đã tồn tại trong bảng Chuyến Xe.");
                return View(tuyenXe);
            }

            db.TuyenXes.Remove(tuyenXe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}