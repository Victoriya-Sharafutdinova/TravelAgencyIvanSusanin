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
using TravelAgencyIvanSusaninDAL.BindingModel;

namespace TravelAgencyIvanSusaninMVC.Controllers
{

    public class ClientsController : Controller
    {
        public IClientService service = Globals.ClientService;

        public ActionResult Index()
        {
            return View(service.GetList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost()
        {
            var FIO = Request["FIO"];
            var Email = Request["Email"];
            var Login = Request["Login"];
            var Password = Request["Password"];
            var viewModel = service.GetList();
            if (!FIO.Equals(null) && !Email.Equals(null) && !Login.Equals(null) && !Password.Equals(null))
            {
                for (int i = 0; i < viewModel.Count; i++)
                {
                    if (FIO.Equals(viewModel[i].FIO))
                    {
                        ModelState.AddModelError("FIO", "Уже существует клиент с таким именем");
                    }
                    if (Login.Equals(viewModel[i].Login))
                    {
                        ModelState.AddModelError("Login", "Уже существует клиент с таким логином");
                    }
                }
                service.AddElement(new ClientBindingModel
                {
                    FIO = Request["FIO"],
                    Email = Request["Email"],
                    Login = Request["Login"],
                    Password = Request["Password"]
                });
                return RedirectToAction("Authorization");

            }
            return Redirect("/Exception/Index/3");

        }

        public ActionResult Edit(int id)
        {
            var viewModel = service.GetElement(id);
            var bindingModel = new ClientBindingModel
            {
                Id = id,
                FIO = viewModel.FIO,
                Login = viewModel.Login,
                Password = viewModel.Password
            };
            return View(bindingModel);

        }


        [HttpPost]
        public ActionResult EditPost()
        {
            service.UpdElement(new ClientBindingModel
            {
                Id = int.Parse(Request["Id"]),
                FIO = Request["FIO"],
                Login = Request["Login"],
                Password = Request["Password"]
            });
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            service.DelElement(id);
            return RedirectToAction("Index");
        }

        public ActionResult Authorization()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authorization([Bind(Include = "Login, Password")] Client client)
        {
            if (service.GetList().Any(rec => rec.Login == client.Login && rec.Password == client.Password))
            {
                var authClient = service.GetList().FirstOrDefault(cl => cl.Login == client.Login);
                Globals.AuthClient = authClient;
                return RedirectToAction("Index", "Travels");
            }
            else
            {
                ModelState.AddModelError("Password", "Проверьте правильность данных");
            }
            return View(client);
        }
        public ActionResult Exit()
        {
            Globals.AuthClient = null;
            return RedirectToAction("Authorization");
        }
    }
}
