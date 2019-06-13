﻿using System;
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
        // GET: User
        public ActionResult Index()
        {
            ViewBag.User = Globals.AuthClient;
            ViewBag.service = service;
            return View();
        }
    }
}