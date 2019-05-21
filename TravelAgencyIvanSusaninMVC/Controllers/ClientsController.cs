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

namespace TravelAgencyIvanSusaninMVC.Controllers
{
    
    public class ClientsController : Controller
    {
        private Context db = new Context();

        // GET: Clients
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FIO,Email,Login,Password")] Client client)
        {
            if (ModelState.IsValid)
            {
                if (!client.FIO.Equals(null) && !client.Email.Equals(null) && !client.Login.Equals(null) && !client.Password.Equals(null))
                {
                    for (int i = 1; i < db.Clients.ToList().Count + 1; i++)
                    {
                        if (client.FIO.Equals(db.Clients.Find(i).FIO))
                        {
                            return Redirect("/Exception/Index/0");
                        }
                        if (client.Login.Equals(db.Clients.Find(i).Login))
                        {
                            return Redirect("/Exception/Index/3");
                        }
                    }
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Authorization");
                   
                }
                else
                {
                    return Redirect("/Exception/Index/3");
                }
            }
            return Redirect("/Exception/Index/3");
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FIO,Email,Login,Password")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Clients/Authorization
        public ActionResult Authorization()
        {
            return View();
        }

        // POST: Clients/Authorization
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authorization([Bind(Include = "Login,Password")] Client client)
        {
            
                for (int i = 1; i < db.Clients.ToList().Count+1; i++)
                {
                    if (client.Login.Equals(db.Clients.Find(i).Login))
                    {
                        if (client.Password.Equals(db.Clients.Find(i).Password))
                        {
                         return  RedirectToAction("Index", "Travels");
                        }
                        else
                        {
                            return Redirect("/Exception/Index/2");
                        }
                    }
                    
                }
                return Redirect("/Exception/Index/1");
            

           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
