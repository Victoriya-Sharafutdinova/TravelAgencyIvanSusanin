using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelAgencyIvanSusaninDAL.Interfaces;

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    public class ReportController : Controller
    {
        public IReportService service = Globals.ReportService;

        // GET: Report
        public ActionResult Index()
        {
            return View(service.GetClientTravels());
        }
    }
}