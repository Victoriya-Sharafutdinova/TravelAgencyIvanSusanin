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
using TravelAgencyIvanSusaninDAL.Interfaces;

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    public class ToursController : Controller
    {
        private ITourService service = Globals.TourService;

        // GET: Tours
        public ActionResult Index()
        {
            return View(service.GetList());
        }

        public ActionResult Filter()
        {
            return View(service.GetFilteredList());
        }

    }
}
