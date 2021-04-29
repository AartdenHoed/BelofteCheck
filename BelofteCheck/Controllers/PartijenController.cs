using BelofteCheck.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BelofteCheck.Controllers
{
    [Authorize]
    public class PartijenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Partijen
        public ActionResult Index()
        {

            PartijenListVM partijenListVM = new PartijenListVM();

            string msg = "Selecteer een bewerking op een partij of voeg een partij toe";
            string level = partijenListVM.MessageSection.Info;
            string title = "Overzicht P A R T I J E N";

            var q = db.Partijen.ToList();

            if (q.Count == 0)
            {
                level = partijenListVM.MessageSection.Warning;
                msg = "Geen partijen gevonden";
            }
            else
            {
                foreach (var entry in q)
                {
                    Partij part = new Partij
                    {
                        PartijID = entry.PartijID.ToUpper(),
                        PartijNaam = entry.PartijNaam
                    };

                    partijenListVM.PartijenLijst.Add(part);
                }
                partijenListVM.MessageSection.SetMessage(title, level, msg);
            }
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }
            partijenListVM.MessageSection.SetMessage(title, level, msg);
            return View(partijenListVM);


        }

        // GET: Partijen/Details/5
        public ActionResult Details(string PartijID)
        {

            PartijenVM partijenVM = new PartijenVM();
            string title = "Details";
            string level = partijenVM.MessageSection.Info;
            string msg = "Politieke partij met gerelateerde stemmingen";

            if (PartijID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Partij ID!";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }

            // Fill the View Model

            Partijen partij = db.Partijen.Find(PartijID);
            if (partij == null)
            {
                TempData["BCmessage"] = "Partij ID " + PartijID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }
            var query1 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join s in db.Stemmingen on p.PartijID equals s.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()
                         join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                         from lj2 in ljoin2.DefaultIfEmpty()
                         select new StemObject
                         {
                             PartijID = p.PartijID,
                             Voor = lj1 == null ? 0 : lj1.Voor,
                             Tegen = lj1 == null ? 0 : lj1.Tegen,
                             Blanco = lj1 == null ? 0 : lj1.Blanco,
                             WetID = lj1 == null ? "<geen>" : lj1.WetID,
                             WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                             WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                             WetLink = lj2 == null ? "nvt" : lj2.WetLink
                         }

                        ;

            List<StemObject> stemlijst = query1.ToList();

            if (stemlijst[0].WetID == "<geen>")
            {
                msg = "Deze partij heeft geen gekoppelde stemmingen en is dus niet actief. Koppel één of meer stemmingen aan deze partij";
                level = "W";
            }

            var query2 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join z in db.PartijZetels on p.PartijID equals z.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()

                         select new ZetelObject
                         {
                             PartijID = p.PartijID,
                             AantalZetels = lj1 == null ? 0 : lj1.AantalZetels,
                             VanDatum = lj1 == null ? DateTime.MinValue : lj1.VanDatum,
                             TotDatum = lj1 == null ? DateTime.MinValue : lj1.TotDatum,
                             IncludeIfSelected = true,
                             InError = " ",
                             ErrorMsg = " "
                         }

                        ;

            List<ZetelObject> zetellijst = query2.ToList();

            partijenVM.Fill(partij, stemlijst, zetellijst);
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }

            partijenVM.MessageSection.SetMessage(title, level, msg);

            return View(partijenVM);

        }

        // GET: Partijen/Create
        public ActionResult Create()
        {
            PartijenVM partijenVM = new PartijenVM();
            string title = "Nieuw";
            string level = partijenVM.MessageSection.Info;
            string msg = "Vul de gegevens voor deze nieuwe partij in en selecteer AANMAKEN";
            partijenVM.MessageSection.SetMessage(title, level, msg);
            return View(partijenVM);
        }

        // POST: Partijen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(PartijenVM partijenVM)
        {
            string title = "Nieuw";


            if (ModelState.IsValid)
            {
                Partijen partijen = new Partijen();
                partijen.Fill(partijenVM);
                try
                {
                    db.Partijen.Add(partijen);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string exnum = ex.Message;

                    string emsg = "Partij '" + partijenVM.partij.PartijID.Trim() + "' bestaat al? (" + exnum + ")";
                    string elevel = partijenVM.MessageSection.Error;
                    partijenVM.MessageSection.SetMessage(title, elevel, emsg);
                    return View(partijenVM);
                }
                TempData["BCmessage"] = "Partij " + partijenVM.partij.PartijID.Trim() + " is nu aangemaakt";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Info;

                return RedirectToAction("Index");
            }

            string level = partijenVM.MessageSection.Error;
            string msg = "ERROR - Partij " + partijenVM.partij.PartijID.Trim() + " is NIET aangemaakt";
            partijenVM.MessageSection.SetMessage(title, level, msg);
            return View(partijenVM);

        }

        // GET: Partijen/Edit/5
        public ActionResult Edit(string PartijID)
        {
            PartijenVM partijenVM = new PartijenVM();
            string title = "Bewerken";
            string level = partijenVM.MessageSection.Info;
            string msg = "Bewerk deze partij en selecteer OPSLAAN";

            if (PartijID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Onderwerp ID!";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }

            Partijen partij = db.Partijen.Find(PartijID);
            if (partij == null)
            {
                TempData["BCmessage"] = "Partij ID " + PartijID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }
            var query1 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join s in db.Stemmingen on p.PartijID equals s.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()
                         join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                         from lj2 in ljoin2.DefaultIfEmpty()
                         select new StemObject
                         {
                             PartijID = p.PartijID,
                             Voor = lj1 == null ? 0 : lj1.Voor,
                             Tegen = lj1 == null ? 0 : lj1.Tegen,
                             Blanco = lj1 == null ? 0 : lj1.Blanco,
                             WetID = lj1 == null ? "<geen>" : lj1.WetID,
                             WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                             WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                             WetLink = lj2 == null ? "nvt" : lj2.WetLink

                         }

                        ;

            List<StemObject> stemlijst = query1.ToList();

            var query2 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join z in db.PartijZetels on p.PartijID equals z.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()
                         select new ZetelObject
                         {
                             PartijID = p.PartijID,
                             AantalZetels = lj1 == null ? 0 : lj1.AantalZetels,
                             VanDatum = lj1 == null ? DateTime.MinValue : lj1.VanDatum,
                             TotDatum = lj1 == null ? DateTime.MinValue : lj1.TotDatum,
                             IncludeIfSelected = true,
                             InError = lj1 == null ? ZetelObject.Na : " ",
                             ErrorMsg = lj1 == null ? "Na aanvinken kan deze entry worden toegevoegd" : " "
                         }

                        ;

            List<ZetelObject> zetellijst = query2.ToList();
            if (!((zetellijst[0].VanDatum == DateTime.MinValue) && (zetellijst[0].TotDatum == DateTime.MinValue)))
            {
                // add null entry
                ZetelObject extra = new ZetelObject
                {
                    AantalZetels = 0,
                    VanDatum = DateTime.MinValue,
                    TotDatum = DateTime.MinValue,
                    IncludeIfSelected = true,
                    ErrorMsg = "Na aanvinken kan deze entry worden toegevoegd",
                    InError = ZetelObject.Na,
                    PartijID = PartijID
                };
                zetellijst.Add(extra);
            }

            partijenVM.Fill(partij, stemlijst, zetellijst);

            partijenVM.MessageSection.SetMessage(title, level, msg);

            return View(partijenVM);

        }

        // POST: Partijen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartijenVM partijenVM)
        {
            string title = "Bewerken";
            string level = "";
            string msg = "";

            if (ModelState.IsValid)
            {
                partijenVM.CheckZetels();

                if (partijenVM.ZetelError)
                {
                    level = partijenVM.MessageSection.Error;
                    msg = "ERROR - periodes overlappen of periodes hebben einddatum die voor de begindatum ligt";
                    partijenVM.MessageSection.SetMessage(title, level, msg);
                    if (!((partijenVM.ZetelLijst[0].VanDatum == DateTime.MinValue) && (partijenVM.ZetelLijst[0].TotDatum == DateTime.MinValue)))
                    {
                        // add null entry
                        ZetelObject extra = new ZetelObject { AantalZetels = 0, VanDatum = DateTime.MinValue, TotDatum = DateTime.MinValue, IncludeIfSelected = false };
                        partijenVM.ZetelLijst.Add(extra);
                    }
                    ModelState.Clear();
                    return View(partijenVM);
                }

                //var zetels = from o in db.PartijZetels
                //         where o.PartijID == partijenVM.partij.PartijID
                //         select o;
                //foreach (var zd in zetels)
                //{                   
                //    db.PartijZetels.Remove(zd);
                //    db.SaveChanges();
                //}
                db.PartijZetels.RemoveRange(db.PartijZetels.Where(c => c.PartijID == partijenVM.partij.PartijID.Trim()));

                db.SaveChanges();

                Partijen partijen = new Partijen();
                partijen.Fill(partijenVM);
                db.Partijen.Attach(partijen);
                db.Entry(partijen).State = EntityState.Modified;
                db.SaveChanges();


                foreach (ZetelObject zo in partijenVM.ZetelLijst)
                {
                    PartijZetels pz = new PartijZetels();
                    pz.Fill(partijenVM.partij.PartijID, zo);
                    db.PartijZetels.Add(pz);
                }
                db.SaveChanges();


                TempData["BCmessage"] = "Partij " + partijenVM.partij.PartijID.Trim() + " is gewijzigd";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Info;

                return RedirectToAction("Details", new { PartijID = partijenVM.partij.PartijID });
            }

            level = partijenVM.MessageSection.Error;
            msg = "ERROR - Partij " + partijenVM.partij.PartijID.Trim() + " is NIET gewijzigd";
            partijenVM.MessageSection.SetMessage(title, level, msg);
            return View(partijenVM);
        }

        // GET: Partijen/Delete/5
        public ActionResult Delete(string PartijID)
        {
            PartijenVM partijenVM = new PartijenVM();
            string title = "Verwijderen";
            string level = partijenVM.MessageSection.Info;
            string msg = "Selecteer VERWIJDEREN om deze partij te verwijderen";

            if (PartijID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Partij ID!";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }

            // Fill the View Model

            Partijen partij = db.Partijen.Find(PartijID);
            if (partij == null)
            {
                TempData["BCmessage"] = "Partij ID " + PartijID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }
            var query1 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join s in db.Stemmingen on p.PartijID equals s.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()
                         join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                         from lj2 in ljoin2.DefaultIfEmpty()
                         select new StemObject
                         {
                             PartijID = p.PartijID,
                             Voor = lj1 == null ? 0 : lj1.Voor,
                             Tegen = lj1 == null ? 0 : lj1.Tegen,
                             Blanco = lj1 == null ? 0 : lj1.Blanco,
                             WetID = lj1 == null ? "<geen>" : lj1.WetID,
                             WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                             WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                             WetLink = lj2 == null ? "nvt" : lj2.WetLink
                         }

                        ;

            List<StemObject> stemlijst = query1.ToList();

            if (stemlijst[0].WetID == "<geen>")
            {
                msg = "Deze partij heeft geen gekoppelde stemmingen en kan dus worden gedelete";
                level = partijenVM.MessageSection.Warning; ;
                partijenVM.DeleteAllowed = true;
            }
            else
            {
                msg = "Er zijn nog stemmingen van deze partij. Partij kan niet verwijderd worden";
                level = partijenVM.MessageSection.Warning;
                partijenVM.DeleteAllowed = false;
            }

            var query2 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join z in db.PartijZetels on p.PartijID equals z.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()

                         select new ZetelObject
                         {
                             PartijID = p.PartijID,
                             AantalZetels = lj1 == null ? 0 : lj1.AantalZetels,
                             VanDatum = lj1 == null ? DateTime.MinValue : lj1.VanDatum,
                             TotDatum = lj1 == null ? DateTime.MinValue : lj1.TotDatum,
                             IncludeIfSelected = true,
                             InError = " ",
                             ErrorMsg = " "

                         }

                        ;

            List<ZetelObject> zetellijst = query2.ToList();

            partijenVM.Fill(partij, stemlijst, zetellijst);

            partijenVM.MessageSection.SetMessage(title, level, msg);

            return View(partijenVM);
        }

        // POST: Partijen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string PartijID)
        {
            PartijenVM partijenVM = new PartijenVM();
            string title = "Verwijderen";

            if (PartijID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Partij ID!";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }

            // Check for related wetten
            // Perform outer join from Partijen via Stemmingen to Wetten

            // Fill the View Model

            Partijen partij = db.Partijen.Find(PartijID);
            if (partij == null)
            {
                TempData["BCmessage"] = "Partij ID " + PartijID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = partijenVM.MessageSection.Error;

                return RedirectToAction("Error");
            }
            var query1 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join s in db.Stemmingen on p.PartijID equals s.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()
                         join w in db.Wetten on lj1.WetID equals w.WetID into ljoin2
                         from lj2 in ljoin2.DefaultIfEmpty()
                         select new StemObject
                         {
                             PartijID = p.PartijID,
                             Voor = lj1 == null ? 0 : lj1.Voor,
                             Tegen = lj1 == null ? 0 : lj1.Tegen,
                             Blanco = lj1 == null ? 0 : lj1.Blanco,
                             WetID = lj1 == null ? "<geen>" : lj1.WetID,
                             WetNaam = lj2 == null ? "nvt" : lj2.WetNaam,
                             WetOmschrijving = lj2 == null ? "nvt" : lj2.WetOmschrijving,
                             WetLink = lj2 == null ? "nvt" : lj2.WetLink
                         }

                        ;

            List<StemObject> stemlijst = query1.ToList();

            var query2 = from p in db.Partijen
                         where p.PartijID == PartijID
                         join z in db.PartijZetels on p.PartijID equals z.PartijID into ljoin1
                         from lj1 in ljoin1.DefaultIfEmpty()

                         select new ZetelObject
                         {
                             PartijID = p.PartijID,
                             AantalZetels = lj1 == null ? 0 : lj1.AantalZetels,
                             VanDatum = lj1 == null ? DateTime.MinValue : lj1.VanDatum,
                             TotDatum = lj1 == null ? DateTime.MinValue : lj1.TotDatum,
                             IncludeIfSelected = lj1 == null ? false : true,
                             InError = lj1 == null ? " " : ZetelObject.Ok,
                             ErrorMsg = lj1 == null ? "Na aanvinken kan deze entry worden toegevoegd" : " "
                         }

                        ;

            List<ZetelObject> zetellijst = query2.ToList();

            partijenVM.Fill(partij, stemlijst, zetellijst);

            if (stemlijst[0].WetID != "<geen>")
            {
                string msg = "Er zijn nog stemmingen van deze parij. Partij kan niet verwijderd worden";
                string level = partijenVM.MessageSection.Warning;
                partijenVM.MessageSection.SetMessage(title, level, msg);
                return View(partijenVM);
            }

            Partijen partijen = db.Partijen.Find(PartijID);
            db.Partijen.Remove(partijen);
            db.SaveChanges();

            TempData["BCmessage"] = "Partij '" + partijen.PartijID.Trim() + "' is succesvol verwijderd";
            TempData["BCerrorlevel"] = partijenVM.MessageSection.Info;
            return RedirectToAction("Index");
        }
        public ActionResult Verify()
        {
            ZetelControleVM zetelcontroleVM = new ZetelControleVM();
            string title = "Kamerzetel Controle";
            string level = zetelcontroleVM.MessageSection.Info;
            string msg = "Check of alle 2e kamer-periodes correct zijn gevuld met 150 zetels";
            

            var query = from pz in db.PartijZetels
                        group pz by new { pz.VanDatum } into grp1
                        select new { VanDatum = grp1.Key.VanDatum,
                                    TotDatum = grp1.Min(pz => pz.TotDatum) }
                        into grp2
                        orderby grp2.VanDatum descending
                        select new TijdVak
                        {
                            VanDatum = grp2.VanDatum,
                            TotDatum = grp2.TotDatum

                        } 
                        ;                        

            zetelcontroleVM.Periodes = query.ToList();

            DateTime todate = DateTime.MaxValue;
            foreach (TijdVak tv in zetelcontroleVM.Periodes)
            {
                if (todate != DateTime.MaxValue)
                {
                    tv.TotDatum = todate;
                }
                todate = tv.VanDatum.AddDays(-1);

            }

            zetelcontroleVM.Periodes.Sort((x, y) => DateTime.Compare(x.VanDatum, y.VanDatum));

            int aantalfouten = 0;
            foreach (TijdVak tv2 in zetelcontroleVM.Periodes)
            {
                var query1 = from p in db.Partijen
                             join j1 in db.PartijZetels on p.PartijID equals j1.PartijID
                             where ((j1.VanDatum <= tv2.VanDatum) && (j1.TotDatum >= tv2.TotDatum))
                             select new KamerZetels
                             {
                                 PartijID = p.PartijID,
                                 PartijNaam = p.PartijNaam,
                                 AantalZetels = j1.AantalZetels,
                                 
                             };
                var q = query1.ToList();
                foreach (KamerZetels kz in q)
                {
                    KamerZetels x = new KamerZetels
                    {
                        AantalZetels = kz.AantalZetels,
                        PartijID = kz.PartijID,
                        PartijNaam = kz.PartijNaam
                    };
                    tv2.ZetelVerdeling.Add(x);

                }
                if (tv2.ZetelCheck != "ok")
                {
                    aantalfouten += 1;
                }
            }

            if (aantalfouten == 0)
            {
                title = title + " (geen fouten gevonden)";
            }
            else
            {
                title = title + " ("+ aantalfouten.ToString() + " fouten gevonden)";
                level = zetelcontroleVM.MessageSection.Warning;
            }
            zetelcontroleVM.MessageSection.SetMessage(title, level, msg);
            return View(zetelcontroleVM);
        }
        public ActionResult Error()
        {
            PartijenVM partijenVM = new PartijenVM();
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
                level = partijenVM.MessageSection.Error;
            }
            partijenVM.MessageSection.SetMessage(title, level, msg);
            return View(partijenVM);
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
