using BelofteCheck.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BelofteCheck.Controllers
{
    [Authorize]
    public class OnderwerpenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Onderwerpen
        public ActionResult Index()
        {

            OnderwerpenListVM onderwerpenListVM = new OnderwerpenListVM();

            string msg = "Selecteer een bewerking op een onderwerp of voeg een onderwerp toe";
            string level = onderwerpenListVM.MessageSection.Info;
            string title = "Overzicht";
            var q = db.Onderwerpen.ToList();
            if (q.Count == 0)
            {
                level = onderwerpenListVM.MessageSection.Warning;
                msg = "Geen onderwerpen gevonden";
            }
            else
            {
                foreach (var entry in q)
                {
                    Onderwerp ond = new Onderwerp
                    {
                        Omschrijving = entry.Omschrijving,
                        OnderwerpID = entry.OnderwerpID.ToUpper()
                    };

                    onderwerpenListVM.OnderwerpenLijst.Add(ond);
                }
                onderwerpenListVM.MessageSection.SetMessage(title, level, msg);
            }
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }
            onderwerpenListVM.MessageSection.SetMessage(title, level, msg);
            return View(onderwerpenListVM);


        }

        // GET: Onderwerpen/Details/5
        public ActionResult Details(string OnderwerpID)
        {

            OnderwerpenVM onderwerpenVM = new OnderwerpenVM();
            string title = "Details";
            string level = onderwerpenVM.MessageSection.Info;
            string msg = "Politiek onderwerp met gerelateerde wetten";

            if (OnderwerpID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Onderwerp ID!";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            // Perform outer join from Onderwerpen via Wetscope to Wetten

            var query = from o in db.Onderwerpen
                        where o.OnderwerpID == OnderwerpID
                        join s in db.WetScope on o.OnderwerpID equals s.OnderwerpID into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty()
                        select new WetObject
                        {
                            OnderwerpID = o.OnderwerpID,
                            Omschrijving = o.Omschrijving,
                            Toelichting = lj1 == null ? "<geen>" : lj1.Toelichting,
                            WetID = lj1 == null ? "<geen>" : lj1.WetID,
                            WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                            WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                            WetLink = lj2 == null ? "nvt" : lj2.WetLink
                        }

                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Ondewerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }
            if (q[0].WetID == "<geen>")
            {
                msg = "Dit onderwerp heeft geen gekoppelde wetten en is dus ongebruikt. In gebruik name doe je via het bewerken van wetten";
                level = "W";
            }

            onderwerpenVM.Fill(q);

            onderwerpenVM.MessageSection.SetMessage(title, level, msg);

            return View(onderwerpenVM);

        }

        // GET: Onderwerpen/Create
        public ActionResult Create()
        {
            OnderwerpenVM onderwerpenVM = new OnderwerpenVM();
            string title = "Nieuw";
            string level = onderwerpenVM.MessageSection.Info;
            string msg = "Vul de gegevens voor dit nieuwe onderwerp in en selecteer AANMAKEN";
            onderwerpenVM.MessageSection.SetMessage(title, level, msg);
            return View(onderwerpenVM);
        }

        // POST: Onderwerpen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(OnderwerpenVM onderwerpenVM)
        {
            string title = "Nieuw";


            if (ModelState.IsValid)
            {
                Onderwerpen onderwerpen = new Onderwerpen();
                onderwerpen.OnderwerpID = onderwerpenVM.onderwerp.OnderwerpID;
                onderwerpen.Omschrijving = onderwerpenVM.onderwerp.Omschrijving;
                try
                {
                    db.Onderwerpen.Add(onderwerpen);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exnum = ex.Message;

                    string emsg = "Onderwerp '" + onderwerpenVM.onderwerp.OnderwerpID.Trim() + "' bestaat al? (" + exnum + ")";
                    string elevel = onderwerpenVM.MessageSection.Error;
                    onderwerpenVM.MessageSection.SetMessage(title, elevel, emsg);
                    return View(onderwerpenVM);
                }
                TempData["BCmessage"] = "Onderwerp " + onderwerpenVM.onderwerp.OnderwerpID.Trim() + " is nu aangemaakt";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Info;

                return RedirectToAction("Index");
            }

            string level = onderwerpenVM.MessageSection.Error;
            string msg = "ERROR - Onderwerp " + onderwerpenVM.onderwerp.OnderwerpID.Trim() + " is NIET aangemaakt";
            onderwerpenVM.MessageSection.SetMessage(title, level, msg);
            return View(onderwerpenVM);



        }

        // GET: Onderwerpen/Edit/5
        public ActionResult Edit(string OnderwerpID)
        {
            OnderwerpenVM onderwerpenVM = new OnderwerpenVM();
            string title = "Bewerken";
            string level = onderwerpenVM.MessageSection.Info;
            string msg = "Bewerk dit onderwerp en selecteer OPSLAAN";

            if (OnderwerpID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Onderwerp ID!";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            // Perform outer join from Onderwerpen via Wetscope to Wetten

            var query = from o in db.Onderwerpen
                        where o.OnderwerpID == OnderwerpID
                        join s in db.WetScope on o.OnderwerpID equals s.OnderwerpID into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty()
                        select new WetObject
                        {
                            OnderwerpID = o.OnderwerpID,
                            Omschrijving = o.Omschrijving,
                            Toelichting = lj1 == null ? "<geen>" : lj1.Toelichting,
                            WetID = lj1 == null ? "<geen>" : lj1.WetID,
                            WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                            WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                            WetLink = lj2 == null ? "nvt" : lj2.WetLink
                        }

                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Ondewerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }
            if (q[0].WetID == "<geen>")
            {
                msg = "Dit onderwerp heeft geen gekoppelde wetten en is dus ongebruikt. In gebruik name doe je via het bewerken van wetten";
                level = "W";
            }

            onderwerpenVM.Fill(q);
            onderwerpenVM.MessageSection.SetMessage(title, level, msg);

            return View(onderwerpenVM);

        }

        // POST: Onderwerpen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OnderwerpenVM onderwerpenVM)
        {
            string title = "Bewerken";

            if (ModelState.IsValid)
            {
                Onderwerpen onderwerpen = new Onderwerpen();
                onderwerpen.OnderwerpID = onderwerpenVM.onderwerp.OnderwerpID;
                onderwerpen.Omschrijving = onderwerpenVM.onderwerp.Omschrijving;
                db.Entry(onderwerpen).State = EntityState.Modified;
                db.SaveChanges();
                TempData["BCmessage"] = "Onderwerp " + onderwerpenVM.onderwerp.OnderwerpID.Trim() + " is gewijzigd";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Info;

                return RedirectToAction("Index");
            }

            string level = onderwerpenVM.MessageSection.Error;
            string msg = "ERROR - Onderwerp " + onderwerpenVM.onderwerp.OnderwerpID.Trim() + " is NIET gewijzigd";
            onderwerpenVM.MessageSection.SetMessage(title, level, msg);
            return View(onderwerpenVM);
        }

        // GET: Onderwerpen/Delete/5
        public ActionResult Delete(string OnderwerpID)
        {
            OnderwerpenVM onderwerpenVM = new OnderwerpenVM();
            string title = "Verwijderen";
            string level = onderwerpenVM.MessageSection.Info;
            string msg = "Selecteer VERWIJDEREN om dit onderwerp te verwijderen";

            if (OnderwerpID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Onderwerp ID!";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            // Perform outer join from Onderwerpen via Wetscope to Wetten

            var query = from o in db.Onderwerpen
                        where o.OnderwerpID == OnderwerpID
                        join s in db.WetScope on o.OnderwerpID equals s.OnderwerpID into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty()
                        select new WetObject
                        {
                            OnderwerpID = o.OnderwerpID,
                            Omschrijving = o.Omschrijving,
                            Toelichting = lj1 == null ? "<geen>" : lj1.Toelichting,
                            WetID = lj1 == null ? "<geen>" : lj1.WetID,
                            WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                            WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                            WetLink = lj2 == null ? "nvt" : lj2.WetLink
                        }

                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Ondewerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }
            if (q[0].WetID == "<geen>")
            {
                msg = "Dit onderwerp heeft geen gekoppelde wetten en kan dus worden gedelete";
                level = onderwerpenVM.MessageSection.Warning; ;
                onderwerpenVM.DeleteAllowed = true;
            }
            else
            {
                msg = "Er zijn nog wetten met dit onderwerp. Onderwerp kan niet verwijderd worden";
                level = onderwerpenVM.MessageSection.Warning;
                onderwerpenVM.DeleteAllowed = false;
            }

            onderwerpenVM.Fill(q);

            onderwerpenVM.MessageSection.SetMessage(title, level, msg);

            return View(onderwerpenVM);
        }

        // POST: Onderwerpen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string OnderwerpID)
        {
            OnderwerpenVM onderwerpenVM = new OnderwerpenVM();
            string title = "Verwijderen";

            if (OnderwerpID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Onderwerp ID!";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            // Check for related wetten
            var query = from o in db.Onderwerpen
                        where o.OnderwerpID == OnderwerpID
                        join s in db.WetScope on o.OnderwerpID equals s.OnderwerpID into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty()
                        select new WetObject
                        {
                            OnderwerpID = o.OnderwerpID,
                            Omschrijving = o.Omschrijving,
                            Toelichting = lj1 == null ? "<geen>" : lj1.Toelichting,
                            WetID = lj1 == null ? "<geen>" : lj1.WetID,
                            WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                            WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                            WetLink = lj2 == null ? "nvt" : lj2.WetLink
                        }

                    ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Ondewerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            onderwerpenVM.Fill(q);

            if (q[0].WetID != "<geen>")
            {
                string msg = "Er zijn nog wetten met dit onderwerp. Onderwerp kan niet verwijderd worden";
                string level = onderwerpenVM.MessageSection.Warning;
                onderwerpenVM.MessageSection.SetMessage(title, level, msg);
                return View(onderwerpenVM);
            }

            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);
            db.Onderwerpen.Remove(onderwerpen);
            db.SaveChanges();

            TempData["BCmessage"] = "Onderwerp '" + onderwerpen.OnderwerpID.Trim() + "' is succesvol verwijderd";
            TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Info;
            return RedirectToAction("Index");
        }
        public ActionResult Error()
        {
            OnderwerpenVM onderwerpenVM = new OnderwerpenVM();
            string title = "ERROR!";
            string msg = "";
            string level = "";
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            else
            {
                msg = "Unknown error";
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();
            }
            else
            {
                level = onderwerpenVM.MessageSection.Error;
            }
            onderwerpenVM.MessageSection.SetMessage(title, level, msg);
            return View(onderwerpenVM);
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
