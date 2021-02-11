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
    public class OnderwerpenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Onderwerpen
        public ActionResult Index()
        {
            return View(db.Onderwerpen.ToList());
        }

        // GET: Onderwerpen/Details/5
        public ActionResult Details(string OnderwerpID)
        {
            if (OnderwerpID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);
            if (onderwerpen == null)
            {
                return HttpNotFound();
            }
            return View(onderwerpen);
        }

        // GET: Onderwerpen/Create
        public ActionResult Create()
        {
            return View(new Onderwerpen());
        }

        // POST: Onderwerpen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OnderwerpID,Omschrijving")] Onderwerpen onderwerpen)
        {
            if (ModelState.IsValid)
            {
                db.Onderwerpen.Add(onderwerpen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(onderwerpen);
        }

        // GET: Onderwerpen/Edit/5
        public ActionResult Edit(string OnderwerpID)
        {
            if (OnderwerpID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);
            if (onderwerpen == null)
            {
                return HttpNotFound();
            }
            return View(onderwerpen);
        }

        // POST: Onderwerpen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OnderwerpID,Omschrijving")] Onderwerpen onderwerpen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onderwerpen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(onderwerpen);
        }

        // GET: Onderwerpen/Delete/5
        public ActionResult Delete(string OnderwerpID)
        {
            if (OnderwerpID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);
            if (onderwerpen == null)
            {
                return HttpNotFound();
            }
            return View(onderwerpen);
        }

        // POST: Onderwerpen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string OnderwerpID)
        {
            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);
            db.Onderwerpen.Remove(onderwerpen);
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
