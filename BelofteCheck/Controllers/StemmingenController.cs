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
    public class StemmingenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Stemmingen
        public ActionResult Index()
        {
            var stemmingen = db.Stemmingen.Include(s => s.Partijen).Include(s => s.Wetten);
            return View(stemmingen.ToList());
        }

        // GET: Stemmingen/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stemmingen stemmingen = db.Stemmingen.Find(id);
            if (stemmingen == null)
            {
                return HttpNotFound();
            }
            return View(stemmingen);
        }

        // GET: Stemmingen/Create
        public ActionResult Create()
        {
            ViewBag.PartijID = new SelectList(db.Partijen, "PartijID", "PartijNaam");
            ViewBag.WetID = new SelectList(db.Wetten, "WetID", "WetNaam");
            return View();
        }

        // POST: Stemmingen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WetID,PartijID,Voor,Tegen,Blanco,StemDatum")] Stemmingen stemmingen)
        {
            if (ModelState.IsValid)
            {
                db.Stemmingen.Add(stemmingen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PartijID = new SelectList(db.Partijen, "PartijID", "PartijNaam", stemmingen.PartijID);
            ViewBag.WetID = new SelectList(db.Wetten, "WetID", "WetNaam", stemmingen.WetID);
            return View(stemmingen);
        }

        // GET: Stemmingen/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stemmingen stemmingen = db.Stemmingen.Find(id);
            if (stemmingen == null)
            {
                return HttpNotFound();
            }
            ViewBag.PartijID = new SelectList(db.Partijen, "PartijID", "PartijNaam", stemmingen.PartijID);
            ViewBag.WetID = new SelectList(db.Wetten, "WetID", "WetNaam", stemmingen.WetID);
            return View(stemmingen);
        }

        // POST: Stemmingen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WetID,PartijID,Voor,Tegen,Blanco,StemDatum")] Stemmingen stemmingen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stemmingen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PartijID = new SelectList(db.Partijen, "PartijID", "PartijNaam", stemmingen.PartijID);
            ViewBag.WetID = new SelectList(db.Wetten, "WetID", "WetNaam", stemmingen.WetID);
            return View(stemmingen);
        }

        // GET: Stemmingen/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stemmingen stemmingen = db.Stemmingen.Find(id);
            if (stemmingen == null)
            {
                return HttpNotFound();
            }
            return View(stemmingen);
        }

        // POST: Stemmingen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Stemmingen stemmingen = db.Stemmingen.Find(id);
            db.Stemmingen.Remove(stemmingen);
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
