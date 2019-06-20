using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelAgencyIvanSusaninDAL.Interfaces;

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    public class UserController : Controller
    {
        readonly IStatisticService service = Globals.StatisticService;
        readonly ITravelService serviceTravel = Globals.TravelService;
        // GET: User
        public ActionResult Index()
        {
            ViewBag.User = Globals.AuthClient;
            ViewBag.service = service;
            return View();
        }
        public ActionResult BackUp()
        {
            serviceTravel.SaveDataBaseClient();
            return RedirectToAction("Index", "User");
        }
    }
}