using BelofteCheck.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BelofteCheck.Controllers
{
    [Authorize]
    public class WettenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Wetten
        public ActionResult Index()
        {
            WettenListVM wettenListVM = new WettenListVM();

            string msg = "Selecteer een bewerking op een wet of voeg een wet toe";
            string level = wettenListVM.MessageSection.Info;
            string title = "Overzicht W E T T E N";

            try
            {
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
                        wo.WetID = entry.WetID.ToUpper();
                        wo.WetLink = entry.WetLink;
                        wo.WetNaam = entry.WetNaam;
                        wo.WetType = entry.WetType.ToUpper();
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
            }
            catch (Exception ex)
            {
                msg = ex.ToString();

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
            string msg = "Wetgegevens en gekoppelde stemmingen en onderwerpen";

            if (WetID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet ID!";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

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
                            WetType = w.WetType,
                            OnderwerpID = lj1 == null ? "<geen>" : lj1.OnderwerpID,
                            Toelichting = lj1 == null ? "nvt" : lj1.Toelichting,
                            Omschrijving = lj2 == null ? "nvt" : lj2.Omschrijving
                        }
                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }


            if (q[0].OnderwerpID == "<geen>")
            {
                msg = "Deze wet heeft geen gekoppelde onderwerpen. Gebruik BEWERK om minstens één onderwerp te koppelen";
                level = "W";
            }

            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }
            // Get Stemmingen for current Wet
            var st = from s in db.Stemmingen
                     where s.WetID == WetID
                     group s by new { s.WetID, s.StemDatum } into grp
                     select new Stemming
                        {
                            WetID = grp.Key.WetID,
                            StemDatum = grp.Key.StemDatum,
                            Voor = grp.Sum(t => t.Voor),
                            Tegen = grp.Sum(t => t.Tegen),
                            Blanco = grp.Sum(t => t.Blanco)

                        } 
                        ;

            wettenVM.Stemmingen = st.ToList();            

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
            var q = db.Onderwerpen.ToList();
            foreach (var entry in q)
            {
                Onderwerp o = new Onderwerp();
                o.Geselecteerd = false;
                o.Omschrijving = entry.Omschrijving;
                o.OnderwerpID = entry.OnderwerpID;
                o.Toelichting = "";
                o.Geselecteerd = false;
                wettenVM.OnderwerpenLijst.Add(o);
            }

            wettenVM.MessageSection.SetMessage(title, level, msg);
            return View(wettenVM);
        }

        // POST: Wetten/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WettenVM wettenVM)
        {
            string title = "Nieuw";

            if (ModelState.IsValid)
            {
                Wetten wetten = new Wetten();
                wetten.Fill(wettenVM);
                try
                {
                    db.Wetten.Add(wetten);

                    foreach (Onderwerp o in wettenVM.OnderwerpenLijst)
                    {
                        if (o.Geselecteerd)
                        {
                            WetScope wetscope = new WetScope();
                            wetscope.Fill(wettenVM.wet.WetID, o);
                            db.WetScope.Add(wetscope);
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exnum = ex.Message;

                    string emsg = "Wet '" + wettenVM.wet.WetID.Trim() + "' bestaat al";
                    string elevel = wettenVM.MessageSection.Error;
                    wettenVM.MessageSection.SetMessage(title, elevel, emsg);
                    return View(wettenVM);
                }
                TempData["BCmessage"] = "Wet " + wettenVM.wet.WetNaam.Trim() + " is nu aangemaakt";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Info;

                return RedirectToAction("Index");
            }

            string level = wettenVM.MessageSection.Error;
            string msg = "ERROR - Wet " + wettenVM.wet.WetNaam.Trim() + " is NIET aangemaakt";
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
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }
            // Perform outer join form Wetten, Wetsccope, Onderwerpen

            var query = from w in db.Wetten
                        where w.WetID == WetID
                        join o in db.Onderwerpen on 1 equals 1 into ljoin2
                        from lj2 in ljoin2.DefaultIfEmpty()
                        join s in db.WetScope on
                        new { wi = w.WetID, ond = lj2.OnderwerpID } equals
                        new { wi = s.WetID, ond = s.OnderwerpID } into ljoin1
                        from lj1 in ljoin1.DefaultIfEmpty()
                        select new WetObject
                        {
                            WetID = w.WetID,
                            WetNaam = w.WetNaam,
                            WetOmschrijving = w.WetOmschrijving,
                            WetLink = w.WetLink,
                            WetType = w.WetType,
                            OnderwerpID = lj2.OnderwerpID,
                            Toelichting = lj1 == null ? "" : lj1.Toelichting,
                            Omschrijving = lj2.Omschrijving
                        }
                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }

            // Check of minimaal gekoppeld aan 1 onderwerp
            bool gekoppeld = false;
            foreach (WetObject wo in q)
            {
                if (!String.IsNullOrEmpty(wo.Toelichting))
                {
                    gekoppeld = true;
                }
            }
            if (!gekoppeld)
            {
                msg = "Deze wet heeft geen gekoppelde onderwerpen. Vink er minimaal één aan!";
                level = "W";
            }

            // Get Stemmingen for current Wet
            var st = from s in db.Stemmingen
                     where s.WetID == WetID
                     group s by new { s.WetID, s.StemDatum } into grp
                     select new Stemming
                     {
                         WetID = grp.Key.WetID,
                         StemDatum = grp.Key.StemDatum,
                         Voor = grp.Sum(t => t.Voor),
                         Tegen = grp.Sum(t => t.Tegen),
                         Blanco = grp.Sum(t => t.Blanco)

                     }
                        ;

            wettenVM.Stemmingen = st.ToList();

            wettenVM.Fill(q);
            wettenVM.MessageSection.SetMessage(title, level, msg);

            return View(wettenVM);
        }


        // POST: Wetten/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WettenVM wettenVM)
        {
            string title = "Bewerken";

            if (ModelState.IsValid)
            {
                Wetten wetten = new Wetten();
                wetten.Fill(wettenVM);
                try
                {
                    db.Entry(wetten).State = EntityState.Modified;
                    db.SaveChanges();

                    foreach (Onderwerp o in wettenVM.OnderwerpenLijst)
                    {
                        WetScope searchdb = db.WetScope.Find(wettenVM.wet.WetID, o.OnderwerpID);
                        if (o.Geselecteerd)
                        {
                            if (searchdb == null)
                            {
                                WetScope ws = new WetScope();
                                ws.Fill(wettenVM.wet.WetID, o);
                                db.WetScope.Add(ws);
                            }
                            else
                            {
                                WetScope ws1 = new WetScope();
                                ws1.Fill(wettenVM.wet.WetID, o);
                                db.WetScope.Attach(ws1);
                                db.Entry(ws1).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            if (searchdb != null)
                            {
                                db.WetScope.Remove(searchdb);
                                db.SaveChanges();
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    string exnum = ex.Message;

                    string emsg = "Wet '" + wettenVM.wet.WetID.Trim() + "' bestaat al? (" + exnum + ")";
                    string elevel = wettenVM.MessageSection.Error;
                    wettenVM.MessageSection.SetMessage(title, elevel, emsg);
                    return View(wettenVM);
                }
                TempData["BCmessage"] = "Wet '" + wettenVM.wet.WetNaam.Trim() + "' is gewijzigd";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Info;

                return RedirectToAction("Details", new { WetID = wettenVM.wet.WetID });

            }

            string level = wettenVM.MessageSection.Error;
            string msg = "ERROR - Wet '" + wettenVM.wet.WetNaam.Trim() + "' is NIET gewijzigd";
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
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

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
                            WetType = w.WetType,
                            OnderwerpID = lj1 == null ? "<geen>" : lj1.OnderwerpID,
                            Toelichting = lj1 == null ? "nvt" : lj1.Toelichting,
                            Omschrijving = lj2 == null ? "nvt" : lj2.Omschrijving
                        }
                        ;

            List<WetObject> q = query.ToList();

            if (q == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }


            if (q[0].OnderwerpID == "<geen>")
            {
                msg = "Deze wet heeft geen gekoppelde onderwerpen. Gebruik BEWERK om minstens één onderwerp te koppelen";
                level = "W";
            }

            // Get Stemmingen for current Wet
            var st = from s in db.Stemmingen
                     where s.WetID == WetID
                     group s by new { s.WetID, s.StemDatum } into grp
                     select new Stemming
                     {
                         WetID = grp.Key.WetID,
                         StemDatum = grp.Key.StemDatum,
                         Voor = grp.Sum(t => t.Voor),
                         Tegen = grp.Sum(t => t.Tegen),
                         Blanco = grp.Sum(t => t.Blanco)

                     }
                        ;

            wettenVM.Stemmingen = st.ToList();

            if (wettenVM.Stemmingen.Count != 0)
            {
                msg += " (hiermee verwijder je ook de getoonde stemmingen)";
                level = wettenVM.MessageSection.Warning;
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
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }

            Wetten wetten = db.Wetten.Find(WetID);

            if (wetten == null)
            {
                TempData["BCmessage"] = "Wet ID " + WetID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = wettenVM.MessageSection.Error;

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
