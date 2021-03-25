using BelofteCheck.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BelofteCheck.Controllers
{
    [Authorize]
    public class StemmingenController : Controller
    {
        private BCentities db = new BCentities();

        // GET: Stemmingen
        public ActionResult Index()
        {
            StemmingenListVM stemmingenlistvm = new StemmingenListVM();

            string msg = "Selecteer een bewerking op een stemming of voeg een stemming toe";
            string level = stemmingenlistvm.MessageSection.Info;
            string title = "Overzicht";

            var query = from s in db.Stemmingen
                        group s by new { s.WetID, s.StemDatum } into grp
                        select new
                        {
                            WetID = grp.Key.WetID,
                            StemDatum = grp.Key.StemDatum,
                            Voor = grp.Sum(t => t.Voor),
                            Tegen = grp.Sum(t => t.Tegen),
                            Blanco = grp.Sum(t => t.Blanco)

                        } into rj1
                        join w in db.Wetten on rj1.WetID equals w.WetID into rjoin2
                        from rj2 in rjoin2
                        select new StemObject
                        {
                            PartijID = "** ALL **",
                            WetID = rj1.WetID,
                            StemDatum = rj1.StemDatum,
                            Voor = rj1.Voor,
                            Tegen = rj1.Tegen,
                            Blanco = rj1.Blanco,
                            WetLink = rj2.WetLink,
                            WetNaam = rj2.WetNaam,
                            WetOmschrijving = rj2.WetOmschrijving

                        }
                        ;

            stemmingenlistvm.StemLijst = query.ToList();
            if (stemmingenlistvm.StemLijst.Count == 0)
            {
                level = stemmingenlistvm.MessageSection.Warning;
                msg = "Geen stemmingen gevonden";
            }
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }
            stemmingenlistvm.MessageSection.SetMessage(title, level, msg);

            return View(stemmingenlistvm);
        }

        // GET: Stemmingen/Details/5
        public ActionResult Details(string wetid, DateTime stemdatum)
        {

            StemmingenListVM stemmingenVM = new StemmingenListVM();
            string title = "Details";
            string level = stemmingenVM.MessageSection.Info;
            string msg = "Stemverdeling bij deze wet: ";

            if ((wetid == null) || (stemdatum == null))
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet + Stemdatum";
                TempData["BCerrorlevel"] = stemmingenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            // Fill the View Model

            var query = from s in db.Stemmingen
                        where ((s.WetID == wetid) && (s.StemDatum == stemdatum))
                        select new
                        {
                            PartijID = s.PartijID,
                            WetID = s.WetID,
                            StemDatum = s.StemDatum,
                            Voor = s.Voor,
                            Tegen = s.Tegen,
                            Blanco = s.Blanco

                        }
                        into rj1
                        join w in db.Wetten on rj1.WetID equals w.WetID into rjoin2
                        from rj2 in rjoin2
                        select new StemObject
                        {
                            PartijID = rj1.PartijID,
                            WetID = rj1.WetID,
                            StemDatum = rj1.StemDatum,
                            Voor = rj1.Voor,
                            Tegen = rj1.Tegen,
                            Blanco = rj1.Blanco,
                            WetLink = rj2.WetLink,
                            WetType = rj2.WetType,
                            WetNaam = rj2.WetNaam,
                            WetOmschrijving = rj2.WetOmschrijving

                        }
            ;

            stemmingenVM.StemLijst = query.ToList();
            if (stemmingenVM.StemLijst.Count == 0)
            {
                level = stemmingenVM.MessageSection.Warning;
                msg = "Geen stemmingen gevonden";
            }
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }
            stemmingenVM.MessageSection.SetMessage(title, level, msg);

            return View(stemmingenVM);

        }

        // GET: Stemmingen/Create/5
        public ActionResult Create(string wetid, DateTime stemdatum)
        {
            StemmingenListVM stemmingenlistVM = new StemmingenListVM();
            stemmingenlistVM.WetID = wetid;
            stemmingenlistVM.StemDatum = stemdatum;
            string title = "Nieuwe Stemming";
            string msg = " ";
            string level = " ";

            if (wetid == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige WetID";
                TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }
            Wetten wet = db.Wetten.Find(wetid);
            if (wet == null)
            {
                TempData["BCmessage"] = "WetID '" + wetid.Trim() + "' is niet gevonden";
                TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            // if dat not given, prompt for it
                        
            if (stemdatum == DateTime.MinValue)
            {
                msg = "Specificeer de stemdatum voor deze wet";
                level = stemmingenlistVM.MessageSection.Info;
                stemmingenlistVM.WetID = wetid;
                stemmingenlistVM.StemDatum = DateTime.Now;
                StemObject so = new StemObject
                {
                    WetID = wet.WetID,
                    WetNaam = wet.WetNaam,
                    WetType = wet.WetType,
                    WetLink = wet.WetLink,
                    WetOmschrijving = wet.WetOmschrijving
                };
                List<StemObject> sl = new List<StemObject>();
                sl.Add(so);

                stemmingenlistVM.Fill(sl);
                stemmingenlistVM.MessageSection.SetMessage(title, level, msg);

                return View("Create", stemmingenlistVM);
            }
            else // Dat is give, so present directly the edit screen for Stemming
            {
                stemmingenlistVM = GetStemmingData(stemmingenlistVM);
                if (stemmingenlistVM.ModelOk)
                {

                    return View("CreatEdit", stemmingenlistVM);
                }
                else
                {
                    return View(stemmingenlistVM);
                }
            }
           
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StemmingenListVM stemmingenlistVM)
        {
            
            stemmingenlistVM = GetStemmingData(stemmingenlistVM);
            if (stemmingenlistVM.ModelOk)
            {
                ModelState.Clear();
                return View("CreatEdit", stemmingenlistVM);
            }
            else
            {
                return View(stemmingenlistVM);
            }

            
        }

        // POST: Stemmingen/CreatEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatEdit(StemmingenListVM stemmingenlistVM)
        {
            string level = " ";
            string msg = " ";
            string title = "Stemmingen wijzigen/toevoegen";
            if (ModelState.IsValid)
            {
                bool foutje = false;
                // Input validation
                int totaalzetels = 0;
                foreach (StemObject sobj in stemmingenlistVM.StemLijst)
                {
                    totaalzetels = totaalzetels + sobj.PartijZetels;
                    sobj.Error = "fout";
                    sobj.Message = " ";
                    if (sobj.PartijZetels != (sobj.Tegen + sobj.Voor + sobj.Blanco))
                    {
                        sobj.Error = "fout";
                        foutje = true;
                        sobj.Message = "Totaal aantal stemmen moet " + sobj.PartijZetels.ToString() + " zijn";
                    }
                    else
                    {
                        sobj.Error = "ok";
                        sobj.Message = " ";
                    }
                }
                if (totaalzetels != 150)
                {
                    level = stemmingenlistVM.MessageSection.Warning;
                    msg = "Totaal aantal stemmen moet uitkomen op 150 (nu: " + totaalzetels.ToString() + ")";
                    foutje = true;

                }
                if (foutje)
                {
                    stemmingenlistVM.MessageSection.SetMessage(title, level, msg);
                    ModelState.Clear();
                    return View(stemmingenlistVM);
                }


            }
            else
            {
                TempData["BCmessage"] = "ModelState ERROR";
                TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Error;

                return RedirectToAction("Error");

            }

            // ok, now update the database
            string mywetid = stemmingenlistVM.StemLijst[0].WetID.Trim();
            DateTime mystemdatum = stemmingenlistVM.StemLijst[0].StemDatum;
            db.Stemmingen.RemoveRange(db.Stemmingen.Where(c => c.WetID == mywetid && c.StemDatum == mystemdatum));

            db.SaveChanges();

            foreach (StemObject so in stemmingenlistVM.StemLijst)
            {
                Stemmingen stemmingen = new Stemmingen
                {
                    PartijID = so.PartijID,
                    WetID = so.WetID,
                    StemDatum = so.StemDatum,
                    Voor = so.Voor,
                    Tegen = so.Tegen,
                    Blanco = so.Blanco
                };
                
                db.Stemmingen.Add(stemmingen);
            }

            db.SaveChanges();
            TempData["BCmessage"] = "Stemming " + mywetid + " is gewijzigd";
            TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Info;

            return RedirectToAction("Details", new {WetID = mywetid, StemDatum = mystemdatum });
        }

        // GET: Stemmingen/Delete/5
        public ActionResult Delete(string wetid, DateTime stemdatum)
        {

            StemmingenListVM stemmingenVM = new StemmingenListVM();
            string title = "Verwijderen";
            string level = stemmingenVM.MessageSection.Info;
            string msg = "Selecteer VERWIJDEREN om deze stemming te verwijderen";

            if ((wetid == null) || (stemdatum == null))
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet + Stemdatum";
                TempData["BCerrorlevel"] = stemmingenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            // Fill the View Model

            var query = from s in db.Stemmingen
                        where ((s.WetID == wetid) && (s.StemDatum == stemdatum))
                        select new
                        {
                            PartijID = s.PartijID,
                            WetID = s.WetID,
                            StemDatum = s.StemDatum,
                            Voor = s.Voor,
                            Tegen = s.Tegen,
                            Blanco = s.Blanco

                        }
                        into rj1
                        join w in db.Wetten on rj1.WetID equals w.WetID into rjoin2
                        from rj2 in rjoin2
                        select new StemObject
                        {
                            PartijID = rj1.PartijID,
                            WetID = rj1.WetID,
                            StemDatum = rj1.StemDatum,
                            Voor = rj1.Voor,
                            Tegen = rj1.Tegen,
                            Blanco = rj1.Blanco,
                            WetLink = rj2.WetLink,
                            WetType = rj2.WetType,
                            WetNaam = rj2.WetNaam,
                            WetOmschrijving = rj2.WetOmschrijving

                        }
            ;

            stemmingenVM.StemLijst = query.ToList();
            if (stemmingenVM.StemLijst.Count == 0)
            {
                level = stemmingenVM.MessageSection.Warning;
                msg = "Geen stemmingen gevonden";
            }
            if (TempData.ContainsKey("BCmessage"))
            {
                msg = TempData["BCmessage"].ToString();
            }
            if (TempData.ContainsKey("BCerrorlevel"))
            {
                level = TempData["BCerrorlevel"].ToString();

            }
            stemmingenVM.MessageSection.SetMessage(title, level, msg);

            return View(stemmingenVM);

        }

        // POST: Stemmingen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string wetid, DateTime stemdatum)
        {
            StemmingenListVM stemmingenlistVM = new StemmingenListVM();
            if ((wetid == null) || (stemdatum == null))
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet ID en stemdatum!";
                TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            var query = (from s in db.Stemmingen
                        where ((s.WetID == wetid) && (s.StemDatum == stemdatum))
                        select s).ToList();

            if (query.Count == 0)
            {
                TempData["BCmessage"] = "Stemming voor Wet ID '" + wetid.Trim() + "' + op datum " + stemdatum.ToString("dd-MMMM-yyyy") + " is niet gevonden";
                TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            db.Stemmingen.RemoveRange(db.Stemmingen.Where(c => c.WetID == wetid && c.StemDatum == stemdatum));

            db.SaveChanges();

            TempData["BCmessage"] = "Stemming voor wet '" + wetid.Trim() + "' op datum " + stemdatum.ToString("dd-MMMM-yyyy") + " is succesvol verwijderd";
            TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Info;
            return RedirectToAction("Index");

            
        }
        public ActionResult Error()
        {
            StemmingenListVM stemmingenlistVM = new StemmingenListVM();
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
                level = stemmingenlistVM.MessageSection.Error;
            }
            stemmingenlistVM.MessageSection.SetMessage(title, level, msg);
            return View(stemmingenlistVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private StemmingenListVM GetStemmingData(StemmingenListVM stemmingenlistVM)
        {
            string level = " ";
            string msg = " ";
            string title = " ";
            DateTime stemdatum = stemmingenlistVM.StemDatum;
            string wetid = stemmingenlistVM.WetID;
            // Date + Wetid are given, get from the database what's there already 

            var wetten = db.Wetten.Find(wetid);
            if (wetten == null)
            {                
                //return stemmingenlistVM;
                TempData["BCmessage"] = "Wet '" + wetid.Trim() + "' is onbekend...";
                TempData["BCerrorlevel"] = stemmingenlistVM.MessageSection.Warning;
                
                RedirectToAction("Error");

            }

            var query1 = from p in db.Partijen
                         join j1 in db.PartijZetels on p.PartijID equals j1.PartijID
                         where ((j1.VanDatum <= stemdatum) && (j1.TotDatum >= stemdatum))
                         select new
                         {
                             PartijID = p.PartijID,
                             PartijNaam = p.PartijNaam,
                             Zetels = j1.AantalZetels,
                             VanDatum = j1.VanDatum,
                             TotDatum = j1.TotDatum

                         };

            var actievepartijen = query1.ToList();

            stemmingenlistVM.ModelOk = true;
            if (actievepartijen.Count == 0)
            {
                level = stemmingenlistVM.MessageSection.Warning;
                msg = "Op de gegeven datum zijn geen partijen actief in de Tweede Kamer";
                stemmingenlistVM.MessageSection.SetMessage(title, level, msg);
                StemObject so = new StemObject
                {
                    WetID = wetten.WetID,
                    WetNaam = wetten.WetNaam,
                    WetLink = wetten.WetLink,
                    WetOmschrijving = wetten.WetOmschrijving,
                    WetType = wetten.WetType
                };
                List<StemObject> sl = new List<StemObject>();
                sl.Add(so);

                stemmingenlistVM.Fill(sl);

                stemmingenlistVM.ModelOk = false;
                return stemmingenlistVM;

            }
                        

            var query3 = from s in db.Stemmingen
                         where (s.WetID == wetid) && (s.StemDatum == stemdatum)
                         select new
                         {
                             PartijID = s.PartijID,
                             StemDatum = s.StemDatum,
                             Voor = s.Voor,
                             Tegen = s.Tegen,
                             Blanco = s.Blanco
                         };

            var bestaandestemmingen = query3.ToList();

            var combine = from a in actievepartijen
                          join b in bestaandestemmingen on a.PartijID equals b.PartijID into joinedList
                          from sub in joinedList.DefaultIfEmpty()
                          select new StemObject
                          {
                              StemDatum = stemdatum,
                              WetID = wetten.WetID,
                              WetLink = wetten.WetLink,
                              WetNaam = wetten.WetNaam,
                              WetType = wetten.WetType,
                              WetOmschrijving = wetten.WetOmschrijving,
                              PartijID = a.PartijID,
                              PartijNaam = a.PartijNaam,
                              PartijZetels = a.Zetels,
                              Voor = sub == null ? 0 : sub.Voor,
                              Tegen = sub == null ? 0 : sub.Tegen,
                              Blanco = sub == null ? 0 : sub.Blanco
                          };

            List<StemObject> vmlist = combine.ToList();
            stemmingenlistVM.Fill(vmlist);

            level = stemmingenlistVM.MessageSection.Info;
            msg = "Vul de stemgegevens in voor elke partij bij deze wet en selecteer OPSLAAN";
            stemmingenlistVM.MessageSection.SetMessage(title, level, msg);
            return stemmingenlistVM;
        }
    }
}
