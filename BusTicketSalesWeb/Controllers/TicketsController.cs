using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusTicketSalesWeb.Controllers
{
    public class TicketsController : Controller
    {
        // GET: Tickets
        public ActionResult Index()
        {
            return View();
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

       
    }
}
