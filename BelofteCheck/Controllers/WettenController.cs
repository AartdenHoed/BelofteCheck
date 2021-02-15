using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BelofteCheck.ViewModels;

namespace BelofteCheck.Controllers
{
    public class WettenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Wetten
        public ActionResult Index()
        {
            WettenListVM wettenListVM = new WettenListVM();

            string msg = "Selecteer een bewerking op een wet of voeg een wet toe";
            string level = wettenListVM.MessageSection.Info;
            string title = "Overzicht";
            var q = db.Wetten.ToList();
            if (q.Count == 0)
            {
                level = wettenListVM.MessageSection.Warning;
                msg = "Geen wetten gevonden";
            }
            else
            {
                foreach (var entry in q)
                {
                    WetObject wo = new WetObject();
                    wo.WetOmschrijving = entry.WetOmschrijving;
                    wo.WetID = entry.WetID;
                    wo.WetLink = entry.WetLink;
                    wo.WetNaam = entry.WetNaam;
                    wettenListVM.WettenLijst.Add(wo);
                }
                wettenListVM.MessageSection.SetMessage(title, level, msg);
            }
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }
            wettenListVM.MessageSection.SetMessage(title, level, msg);
            return View(wettenListVM);
        }

        // GET: Wetten/Details/5
        public ActionResult Details(string WetID)
        {
            WettenVM wettenVM = new WettenVM();
            string title = "Details";
            string level = wettenVM.MessageSection.Info;
            string msg = "Wetgegevens en gekoppelde onderwerpen";

            if (WetID == null) {
                TempData["BCmessage"] = "Specificeer een geldige Wet ID!";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;
                
                return RedirectToAction("Error");
            }
            // Perform outer join form Wetten, Wetsccope, Onderwerpen
    
            var query = from w in db.Wetten
                        where w.WetID == WetID
                        join s in db.WetScope on w.WetID equals s.WetID into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        join o in db.Onderwerpen on lj1.OnderwerpID equals o.OnderwerpID into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty() 
                        select new WetObject
                        {
                            WetID = w.WetID,
                            WetNaam = w.WetNaam,
                            WetOmschrijving = w.WetOmschrijving,
                            WetLink = w.WetLink,
                            OnderwerpID = lj1 == null ? "<geen>" : lj1.OnderwerpID,
                            Toelichting = lj1 == null ? "nvt" : lj1.Toelichting,
                            Omschrijving = lj2 == null ? "nvt" : lj2.Omschrijving
                        } 
                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }


            if (q[0].OnderwerpID == "<geen>")
            {                  
                msg = "Deze wet heeft geen gekoppelde onderwerpen. Gebruik BEWERK om minstens één onderwerp te koppelen";
                level = "W"; 
            }
           
            wettenVM.Fill(q);
            wettenVM.MessageSection.SetMessage(title, level, msg);

            return View(wettenVM);
        }

        // GET: Wetten/Create
        public ActionResult Create()
        {
            WettenVM wettenVM = new WettenVM();
            string title = "Nieuw";
            string level = wettenVM.MessageSection.Info;
            string msg = "Vul de gegevens voor de nieuwe wet in en selecteer AANMAKEN";
            wettenVM.MessageSection.SetMessage(title, level, msg);
            return View(wettenVM);
        }

        // POST: Wetten/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WetID,WetNaam,WetOmschrijving,WetLink")] WettenVM wettenVM)
        {
            string title = "Nieuw";

            if (ModelState.IsValid)
            {
                Wetten wetten = new Wetten();
                wetten.Fill(wettenVM); 
                db.Wetten.Add(wetten);
                db.SaveChanges();
                TempData["BCmessage"] = "Wet " + wettenVM.WetNaam.Trim() + " is nu aangemaakt";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Info;
                
                return RedirectToAction("Index");
            }

            string level = wettenVM.MessageSection.Error;
            string msg = "ERROR - Wet " + wettenVM.WetNaam.Trim() + " is NIET aangemaakt";
            wettenVM.MessageSection.SetMessage(title, level, msg);
            return View(wettenVM);
            
        }

        // GET: Wetten/Edit/5
        public ActionResult Edit(string WetID)
        {
            WettenVM wettenVM = new WettenVM();
            string title = "Bewerken";
            string level = wettenVM.MessageSection.Info;
            string msg = "Bewerk deze wet en/of de gekoppelde onderwerpen en selecteer OPSLAAN";

            if (WetID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet ID!";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }
            // Perform outer join form Wetten, Wetsccope, Onderwerpen

            var query = from w in db.Wetten
                        where w.WetID == WetID
                        join s in db.WetScope on w.WetID equals s.WetID into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        join o in db.Onderwerpen on lj1.OnderwerpID equals o.OnderwerpID into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty()
                        select new WetObject
                        {
                            WetID = w.WetID,
                            WetNaam = w.WetNaam,
                            WetOmschrijving = w.WetOmschrijving,
                            WetLink = w.WetLink,
                            OnderwerpID = lj1 == null ? "<geen>" : lj1.OnderwerpID,
                            Toelichting = lj1 == null ? "nvt" : lj1.Toelichting,
                            Omschrijving = lj2 == null ? "nvt" : lj2.Omschrijving
                        }
                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }


            if (q[0].OnderwerpID == "<geen>")
            {
                msg = "Deze wet heeft geen gekoppelde onderwerpen. Vink er minimaal één aan!";
                level = "W";
            }

            wettenVM.Fill(q);
            wettenVM.MessageSection.SetMessage(title, level, msg);

            return View(wettenVM);
        }
    

        // POST: Wetten/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WetID,WetNaam,WetOmschrijving,WetLink")] WettenVM wettenVM)
        {
            string title = "Bewerken";

            if (ModelState.IsValid)
            {
                Wetten wetten = new Wetten();
                wetten.Fill(wettenVM);
                db.Entry(wetten).State = EntityState.Modified;
                db.SaveChanges();
                TempData["BCmessage"] = "Wet " + wettenVM.WetNaam.Trim() + " is gewijzigd";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Info;

                return RedirectToAction("Index");
            }

            string level = wettenVM.MessageSection.Error;
            string msg = "ERROR - Wet " + wettenVM.WetNaam.Trim() + " is NIET gewijzigd";
            wettenVM.MessageSection.SetMessage(title, level, msg);
            return View(wettenVM);
            
        }

        // GET: Wetten/Delete/5
        public ActionResult Delete(string WetID)
        {
            WettenVM wettenVM = new WettenVM();
            string title = "Verwijder";
            string level = wettenVM.MessageSection.Info;
            string msg = "Selecteer VERWIJDEREN om deze wet te verwijderen";

            if (WetID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet ID!";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }
            // Perform outer join form Wetten, Wetsccope, Onderwerpen

            var query = from w in db.Wetten
                        where w.WetID == WetID
                        join s in db.WetScope on w.WetID equals s.WetID into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        join o in db.Onderwerpen on lj1.OnderwerpID equals o.OnderwerpID into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty()
                        select new WetObject
                        {
                            WetID = w.WetID,
                            WetNaam = w.WetNaam,
                            WetOmschrijving = w.WetOmschrijving,
                            WetLink = w.WetLink,
                            OnderwerpID = lj1 == null ? "<geen>" : lj1.OnderwerpID,
                            Toelichting = lj1 == null ? "nvt" : lj1.Toelichting,
                            Omschrijving = lj2 == null ? "nvt" : lj2.Omschrijving
                        }
                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }


            if (q[0].OnderwerpID == "<geen>")
            {
                msg = "Deze wet heeft geen gekoppelde onderwerpen. Gebruik BEWERK om minstens één onderwerp te koppelen";
                level = "W";
            }

            wettenVM.Fill(q);
            wettenVM.MessageSection.SetMessage(title, level, msg);

            return View(wettenVM);
        }

        // POST: Wetten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string WetID)
        {
            WettenVM wettenVM = new WettenVM();
            if (WetID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet ID!";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }
            
            Wetten wetten = db.Wetten.Find(WetID);

            if (wetten == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            db.Wetten.Remove(wetten);
            db.SaveChanges();

            TempData["BCmessage"] = "Wet '" + wetten.WetNaam.Trim() + "' is succesvol verwijderd";
            TempData["BCerrorlevel"] = wettenVM.MessageSection.Info;
            return RedirectToAction("Index");
        }
        public ActionResult Error()
        {
            WettenVM wettenVM = new WettenVM();
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
                level = wettenVM.MessageSection.Error;
            }
            wettenVM.MessageSection.SetMessage(title, level, msg);
            return View(wettenVM);
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
