using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelAgencyIvanSusaninMVC.Models;
using TravelAgencyIvanSusaninModel;

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    public class TourTravelsController : Controller
    {
        private Context db = new Context();

        // GET: TourTravels
        public ActionResult Index()
        {
            var tourTravels = db.TourTravels.Include(t => t.Tour).Include(t => t.Travel);
            return View(tourTravels.ToList());
        }

        // GET: TourTravels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourTravel tourTravel = db.TourTravels.Find(id);
            if (tourTravel == null)
            {
                return HttpNotFound();
            }
            return View(tourTravel);
        }

        // GET: TourTravels/Create
        public ActionResult Create()
        {
            ViewBag.TourId = new SelectList(db.Tours, "Id", "Name");
            ViewBag.TravelId = new SelectList(db.Travels, "Id", "Id");
            return View();
        }

        // POST: TourTravels/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TravelId,TourId,DateReservation,DateBegin,DateEnd")] TourTravel tourTravel)
        {
            if (ModelState.IsValid)
            {
                db.TourTravels.Add(tourTravel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TourId = new SelectList(db.Tours, "Id", "Name", tourTravel.TourId);
            ViewBag.TravelId = new SelectList(db.Travels, "Id", "Id", tourTravel.TravelId);
            return View(tourTravel);
        }

        // GET: TourTravels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourTravel tourTravel = db.TourTravels.Find(id);
            if (tourTravel == null)
            {
                return HttpNotFound();
            }
            ViewBag.TourId = new SelectList(db.Tours, "Id", "Name", tourTravel.TourId);
            ViewBag.TravelId = new SelectList(db.Travels, "Id", "Id", tourTravel.TravelId);
            return View(tourTravel);
        }

        // POST: TourTravels/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TravelId,TourId,DateReservation,DateBegin,DateEnd")] TourTravel tourTravel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tourTravel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TourId = new SelectList(db.Tours, "Id", "Name", tourTravel.TourId);
            ViewBag.TravelId = new SelectList(db.Travels, "Id", "Id", tourTravel.TravelId);
            return View(tourTravel);
        }

        // GET: TourTravels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourTravel tourTravel = db.TourTravels.Find(id);
            if (tourTravel == null)
            {
                return HttpNotFound();
            }
            return View(tourTravel);
        }

        // POST: TourTravels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TourTravel tourTravel = db.TourTravels.Find(id);
            db.TourTravels.Remove(tourTravel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TourTravels/Reservation/5
        public ActionResult Reservation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourTravel tourTravel = db.TourTravels.Find(id);
            if (tourTravel == null)
            {
                return HttpNotFound();
            }
            return View(tourTravel);
        }

        // POST: TourTravels/Reservation/5
        [HttpPost, ActionName("Reservation")]
        [ValidateAntiForgeryToken]
        public ActionResult Reservation(int id)
        {
            db.TourTravels.Find(id).DateReservation = DateTime.Today;
            
            db.SaveChanges();
            return RedirectToAction("Index");
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
