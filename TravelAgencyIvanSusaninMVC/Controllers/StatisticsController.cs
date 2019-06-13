using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelAgencyIvanSusaninDAL.Interfaces;

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    public class StatisticsController : Controller
    {
        readonly IStatisticService service = Globals.StatisticService;
        // GET: Statistics
        public ActionResult Index()
        {
            ViewBag.service = service;
            return View();
        }
    }
}