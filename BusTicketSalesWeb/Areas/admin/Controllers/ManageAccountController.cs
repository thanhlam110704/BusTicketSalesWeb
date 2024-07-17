using BusTicketSalesWeb.Areas.ViewModel;
using BusTicketSalesWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Areas.admin.Controllers
{
    public class ManageAccountController : Controller
    {
        private QLBANVEXEEntities db = new QLBANVEXEEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Accounts()
        {
            var account = db.TaiKhoans.ToList();
            return View(account);
        }
        public ActionResult EditAccounts(int id)
        {
            var account = db.TaiKhoans.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccounts(TaiKhoan account)
        {
            var tk = db.TaiKhoans.Find(account.Id);
            if (ModelState.IsValid)
            {
                tk.Email = account.Email;
                tk.MatKhau = account.MatKhau;
                tk.Sdt = account.Sdt;
                tk.VaiTro = account.VaiTro;
                db.Entry(tk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Accounts"); 
            }
            return View(account);
        }
    }
}