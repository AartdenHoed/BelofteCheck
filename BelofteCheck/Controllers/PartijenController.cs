using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BelofteCheck;

namespace BelofteCheck.Controllers
{
    [Authorize]
    public class PartijenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Partijen
        public ActionResult Index()
        {
            return View(db.Partijen.ToList());
        }

        // GET: Partijen/Details/5
        public ActionResult Details(string PartijID)
        {
            if (PartijID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partijen partijen = db.Partijen.Find(PartijID);
            if (partijen == null)
            {
                return HttpNotFound();
            }
            return View(partijen);
        }

        // GET: Partijen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Partijen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PartijID,PartijNaam,AantalZetels,VanDatum,TotDatum")] Partijen partijen)
        {
            if (ModelState.IsValid)
            {
                db.Partijen.Add(partijen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(partijen);
        }

        // GET: Partijen/Edit/5
        public ActionResult Edit(string PartijID)
        {
            if (PartijID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partijen partijen = db.Partijen.Find(PartijID);
            if (partijen == null)
            {
                return HttpNotFound();
            }
            return View(partijen);
        }

        // POST: Partijen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PartijID,PartijNaam,AantalZetels,VanDatum,TotDatum")] Partijen partijen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partijen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partijen);
        }

        // GET: Partijen/Delete/5
        public ActionResult Delete(string PartijID)
        {
            if (PartijID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partijen partijen = db.Partijen.Find(PartijID);
            if (partijen == null)
            {
                return HttpNotFound();
            }
            return View(partijen);
        }

        // POST: Partijen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string PartijID)
        {
            Partijen partijen = db.Partijen.Find(PartijID);
            db.Partijen.Remove(partijen);
            db.SaveChanges();
            return RedirectToAction("Index");
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
