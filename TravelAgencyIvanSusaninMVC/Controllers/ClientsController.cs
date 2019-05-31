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
                        return Redirect("/Exception/Index/0");
                    }
                    if (Login.Equals(viewModel[i].Login))
                    {
                        return Redirect("/Exception/Index/3");
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

            /*if (!client.FIO.Equals(null) && !client.Email.Equals(null) && !client.Login.Equals(null) && !client.Password.Equals(null))
            {
                for (int i = 1; i < service.Clients.ToList().Count + 1; i++)
                {
                    if (client.FIO.Equals(service.Clients.Find(i).FIO))
                    {
                        return Redirect("/Exception/Index/0");
                    }
                    if (client.Login.Equals(service.Clients.Find(i).Login))
                    {
                        return Redirect("/Exception/Index/3");
                    }
                }
                service.Add(client);
                service.SaveChanges();
                return RedirectToAction("Authorization");
                   
            }
            else
            {
                return Redirect("/Exception/Index/3");
            }         */
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
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Client client = service.Clients.Find(id);
            //if (client == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(client);
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

            //if (ModelState.IsValid)
            //{
            //    service.Entry(client).State = EntityState.Modified;
            //    service.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(client);
        }

        public ActionResult Delete(int id)
        {
            service.DelElement(id);
            return RedirectToAction("Index");


            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Client client = service.Clients.Find(id);
            //if (client == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(client);
        }

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Client client = service.Clients.Find(id);
        //    service.Clients.Remove(client);
        //    service.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Authorization()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorizationPost()
        {
            var Login = Request["Login"];
            var Password = Request["Password"];
            var viewModel = service.GetList();
            for (int i = 0; i < viewModel.Count; i++)
            {
                if (viewModel[i].Login.Equals(Login))
                {
                    if (viewModel[i].Password.Equals(Password))
                    {
                        return RedirectToAction("Index", "Travels");
                    }
                    else
                    {
                        return Redirect("/Exception/Index/2");
                    }
                }
            }
            return Redirect("/Exception/Index/1");


            //for (int i = 1; i < service.Clients.ToList().Count+1; i++)
            //    {
            //        if (client.Login.Equals(service.Clients.Find(i).Login))
            //        {
            //            if (client.Password.Equals(service.Clients.Find(i).Password))
            //            {
            //             return  RedirectToAction("Index", "Travels");
            //            }
            //            else
            //            {
            //                return Redirect("/Exception/Index/2");
            //            }
            //        }

            //    }
            //    return Redirect("/Exception/Index/1");



        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        service.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
