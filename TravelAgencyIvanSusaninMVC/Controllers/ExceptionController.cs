using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    public class ExceptionController : Controller
    {
        // GET: Exception
        public ActionResult Index(int id)
        {
            string message = "Неизвестная ошибка";

            if (id == 0)
            {
                message = "Уже есть такой клиент.";
            }
            if (id == 1)
            {
                message = "Не верно указан логин или пароль";
            }
            if (id == 2)
            {
                message = "Пароль не верен";
            }
            if (id == 3)
            {
                message = "Уже есть пользователь с таким логином";
            }
            ViewBag.Message = message;
            return View();
        }
    }
}