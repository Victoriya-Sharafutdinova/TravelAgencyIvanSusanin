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
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.BindingModel;

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    public class TravelsController : Controller
    {

        private ITravelService service = Globals.TravelService;
        private ITourService tourService = Globals.TourService;

        // GET: Travels
        public ActionResult Index()
        {
            return View(service.GetList());

        }

        public ActionResult Filter()
        {
            return View(service.GetFilteredList());
        }

        public ActionResult Create()
        {
            if (Session["Travels"] == null)
            {
                var travel = new TravelViewModel();
                travel.TourTravels = new List<TourTravelViewModel>();
                Session["Travels"] = travel;
            }
            return View((TravelViewModel)Session["Travels"]);
        }

        public ActionResult AddTour()
        {
            var tours = new SelectList(tourService.GetList(), "Id", "TourName");
            ViewBag.Tours = tours;
            return View();
        }

        [HttpPost]
        public ActionResult AddTourPost()
        {
            var travel = (TravelViewModel)Session["Travel"];
            var tour = new TourTravelViewModel
            {
                TourId = int.Parse(Request["Id"]),
                TourName = tourService.GetElement(int.Parse(Request["Id"])).Name,
                Count = int.Parse(Request["Count"]) 
            };
            travel.TourTravels.Add(tour);
            Session["Travel"] = travel;
            return RedirectToAction("Index");
        }

        public ActionResult CreateTravel()
        {
            var travel = new SelectList(service.GetList(), "Id");
            var tours = new SelectList(tourService.GetList(), "Id", "Name");
            ViewBag.Travels = travel;
            ViewBag.Tours = tours;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTravelPost()
        {
            var travel = (TravelViewModel)Session["Travel"];
            var tourTravels = new List<TourTravelBindingModel>();
            var total = 0;
            for (int i = 0; i < travel.TourTravels.Count; ++i)
            {
                tourTravels.Add(new TourTravelBindingModel
                {
                    Id = travel.TourTravels[i].Id,
                    TravelId = travel.TourTravels[i].TravelId,
                    TourId = travel.TourTravels[i].TourId,
                    Count = travel.TourTravels[i].Count
                });
                 total += CalcSum(travel.TourTravels[i].TourId, travel.TourTravels[i].Count);
            }
            
            service.CreateTravel(new TravelBindingModel
            {
                TotalCost = total,
                TourTravels = tourTravels
            });
            Session.Remove("Travel");
            return RedirectToAction("Index", "Travels");
        }

        private int CalcSum(int tourId, int tourCount)
        {
            TourViewModel tour = tourService.GetElement(tourId);
            return tourCount * tour.Cost;
        }

        public ActionResult SetStatus(int id, string status)
        {
            try
            {
                switch (status)
                {
                    case "Processing":
                        service.TakeTravelInWork(new TravelBindingModel { Id = id });
                        break;
                    case "Ready":
                        service.FinishTravel(new TravelBindingModel { Id = id });
                        break;
                    case "Paid":
                        service.PayTravel(new TravelBindingModel { Id = id });
                        break;
                    case "Reservation":
                        service.Reservation(new TourTravelBindingModel { Id = id });
                        break;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }


            return RedirectToAction("Index");
        }

        //private Context db = new Context();

        //// GET: Travels
        //public ActionResult Index()
        //{
        //    var travels = db.Travels.Include(t => t.Client);
        //    return View(travels.ToList());
        //}

        //// GET: Travels/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Travel travel = db.Travels.Find(id);
        //    if (travel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(travel);
        //}

        //// GET: Travels/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ClientId = new SelectList(db.Clients, "Id", "FIO");
        //    return View();
        //}

        //// POST: Travels/Create
        //// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        //// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ClientId,TotalCost,TravelStatus,DateCreate,DateImplement")] Travel travel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Travels.Add(travel);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ClientId = new SelectList(db.Clients, "Id", "FIO", travel.ClientId);
        //    return View(travel);
        //}

        //// GET: Travels/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Travel travel = db.Travels.Find(id);
        //    if (travel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ClientId = new SelectList(db.Clients, "Id", "FIO", travel.ClientId);
        //    return View(travel);
        //}

        //// POST: Travels/Edit/5
        //// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        //// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ClientId,TotalCost,TravelStatus,DateCreate,DateImplement")] Travel travel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(travel).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ClientId = new SelectList(db.Clients, "Id", "FIO", travel.ClientId);
        //    return View(travel);
        //}

        //// GET: Travels/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Travel travel = db.Travels.Find(id);
        //    if (travel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(travel);
        //}

        //// POST: Travels/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Travel travel = db.Travels.Find(id);
        //    db.Travels.Remove(travel);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
