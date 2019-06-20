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

        private readonly ITravelService service = Globals.TravelService;
        private readonly ITourService tourService = Globals.TourService;
        private readonly IStatisticService statistic = Globals.StatisticService;
        List<SelectListItem> types = new List<SelectListItem>();

        public ActionResult Index()
        {
            return View(service.GetClientTravels(Globals.AuthClient.Id));
        }

        public ActionResult Details(int id)
        {
            var order = service.GetElement(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        } 

        public ActionResult ReserveDoc(int id)
        {
            var travels = (TravelViewModel)Session["Travels"];

            var travel = service.GetElement(id);
            var travelTours = new List<TourTravelBindingModel>();
            for (int i = 0; i < travel.TourTravels.Count; ++i)
            {
                travelTours.Add(new TourTravelBindingModel
                {
                    Id = travel.TourTravels[i].Id,
                    TravelId = travel.TourTravels[i].TravelId,
                    TourId = travel.TourTravels[i].TourId,
                    Count = travel.TourTravels[i].Count,
                    DateBegin = travel.TourTravels[i].DateBegin,
                    DateEnd = travel.TourTravels[i].DateEnd
                });
            }
           
            service.Reservation(travel.Id, "doc");
            Session.Remove("Travels");
            return RedirectToAction("Index", "Travels");
        }

        public ActionResult ReserveXls(int id)
        {
            var travels = (TravelViewModel)Session["Travels"];
            var travel = service.GetElement(id);
            var travelTours = new List<TourTravelBindingModel>();
            for (int i = 0; i < travel.TourTravels.Count; ++i)
            {
                travelTours.Add(new TourTravelBindingModel
                {
                    Id = travel.TourTravels[i].Id,
                    TravelId = travel.TourTravels[i].TravelId,
                    TourId = travel.TourTravels[i].TourId,
                    Count = travel.TourTravels[i].Count,
                    DateBegin = travel.TourTravels[i].DateBegin,
                    DateEnd = travel.TourTravels[i].DateEnd
                });
            }
            service.Reservation(travel.Id, "xls");
            Session.Remove("Travels");
            return RedirectToAction("Index", "Travels");
        }

        public ActionResult Pay(int id)
        {
            service.PayTravel(new TravelBindingModel { ClientId = Globals.AuthClient.Id, Id = id });
            return View();
        }

        public ActionResult Create()
        {
            if (Session["Travels"] == null)
            {
                var travel = new TravelViewModel();
                travel.TourTravels = new List<TourTravelViewModel>();
                Session["Travels"] = travel;
            }
            ViewBag.Service = statistic;
            return View((TravelViewModel)Session["Travels"]);
        }

        public ActionResult AddTour()
        {
            var tours = new SelectList(tourService.GetList(), "Id", "Name");
            ViewBag.Tours = tours;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTourPost()
        {

            var travel = (TravelViewModel)Session["Travels"];
            bool date = true;
            var dateBegin2 = DateTime.Parse(Request["DateBegin"]);
            var dateEnd2 = DateTime.Parse(Request["DateEnd"]);
            for (int i = 0; i < travel.TourTravels.Count; ++i)
            {
                var dateBegin1 = travel.TourTravels[i].DateBegin;
                var dateEnd1 = travel.TourTravels[i].DateEnd;               
               
                if ((dateBegin2 >= dateBegin1 && dateBegin2 <= dateEnd1) || (dateBegin2 < dateBegin1 && dateBegin1 <= dateEnd2) )
                {
                    date = false;
                    ModelState.AddModelError("Count", "Некорректная дата");
                    ModelState.AddModelError("DateBegin", "Некорректная дата");
                    ModelState.AddModelError("DateEnd", "Некорректная дата");
                }
                else
                {
                    date = true;
                }                
            }
            if ((dateBegin2 > dateEnd2) || (dateBegin2 < DateTime.Now) || (dateEnd2 < DateTime.Now))
            {
                date = false;
                ModelState.AddModelError("Count", "Некорректная дата");
                ModelState.AddModelError("DateBegin", "Некорректная дата");
                ModelState.AddModelError("DateEnd", "Некорректная дата");
            }
            if (date)
            {
                var tour = new TourTravelViewModel
                {
                    TourId = int.Parse(Request["Id"]),
                    TourName = tourService.GetElement(int.Parse(Request["Id"])).Name,
                    Count = int.Parse(Request["Count"]),
                    DateBegin = DateTime.Parse(Request["DateBegin"]),
                    DateEnd = DateTime.Parse(Request["DateEnd"])
                };
                travel.TourTravels.Add(tour);
                Session["Travels"] = travel;
                return RedirectToAction("Create");
            }
            ModelState.AddModelError("Count", "Некорректная дата");
            ModelState.AddModelError("DateBegin", "Некорректная дата");
            ModelState.AddModelError("DateEnd", "Некорректная дата");
            return RedirectToAction("AddTour");
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
            var travel = (TravelViewModel)Session["Travels"];
            var tourTravels = new List<TourTravelBindingModel>();
            var total = 0;

            for (int i = 0; i < travel.TourTravels.Count; ++i)
            {
                tourTravels.Add(new TourTravelBindingModel
                {
                    Id = travel.TourTravels[i].Id,
                    TravelId = travel.TourTravels[i].TravelId,
                    TourId = travel.TourTravels[i].TourId,
                    Count = travel.TourTravels[i].Count,
                    DateBegin = travel.TourTravels[i].DateBegin,
                    DateEnd = travel.TourTravels[i].DateEnd
                });
                total += CalcSum(travel.TourTravels[i].TourId, travel.TourTravels[i].Count);
            }

            service.CreateTravel(new TravelBindingModel
            {
                ClientId = Globals.AuthClient.Id,
                TotalCost = tourTravels.Sum(rec => rec.Count * tourService.GetElement(rec.TourId).Cost),
                TourTravels = tourTravels
            });
            Session.Remove("Travels");
           
            
            return RedirectToAction("Index", "Travels");
        }

        private int CalcSum(int tourId, int tourCount)
        {
            TourViewModel tour = tourService.GetElement(tourId);
            return tourCount * tour.Cost;
        }

        //public ActionResult SetStatus(int id, string status)
        //{
        //    try
        //    {
        //        switch (status)
        //        {
        //            case "Processing":
        //                service.TakeTravelInWork(new TravelBindingModel { Id = id });
        //                break;
        //            case "Ready":
        //                service.FinishTravel(new TravelBindingModel { Id = id });
        //                break;
        //            case "Paid":
        //                service.PayTravel(new TravelBindingModel { Id = id });
        //                break;
        //            case "Reservation":
        //                service.Reservation(new TravelBindingModel { Id = id });
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("Error", ex.Message);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}

