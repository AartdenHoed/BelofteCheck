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
    public class WettenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Wetten
        public ActionResult Index()
        {
            return View(db.Wetten.ToList());
        }

        // GET: Wetten/Details/5
        public ActionResult Details(string WetID)
        {
            if (WetID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wetten wetten = db.Wetten.Find(WetID);
            if (wetten == null)
            {
                return HttpNotFound();
            }
            return View(wetten);
        }

        // GET: Wetten/Create
        public ActionResult Create()
        {
            return View(new Wetten());
        }

        // POST: Wetten/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WetID,WetNaam,WetOmschrijving,WetLink")] Wetten wetten)
        {
            if (ModelState.IsValid)
            {
                db.Wetten.Add(wetten);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wetten);
        }

        // GET: Wetten/Edit/5
        public ActionResult Edit(string WetID)
        {
            if (WetID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wetten wetten = db.Wetten.Find(WetID);
            if (wetten == null)
            {
                return HttpNotFound();
            }
            return View(wetten);
        }

        // POST: Wetten/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WetID,WetNaam,WetOmschrijving,WetLink")] Wetten wetten)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wetten).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wetten);
        }

        // GET: Wetten/Delete/5
        public ActionResult Delete(string WetID)
        {
            if (WetID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wetten wetten = db.Wetten.Find(WetID);
            if (wetten == null)
            {
                return HttpNotFound();
            }
            return View(wetten);
        }

        // POST: Wetten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string WetID)
        {
            Wetten wetten = db.Wetten.Find(WetID);
            db.Wetten.Remove(wetten);
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
