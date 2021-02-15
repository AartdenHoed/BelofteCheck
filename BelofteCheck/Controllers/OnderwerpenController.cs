using System.Data.Entity;
using System.Linq;
using System.Data.Linq;
using System.Web.Mvc;
using BelofteCheck.ViewModels;
using System.Data.SqlClient;
using System;

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
                    OnderwerpenVM ond = new OnderwerpenVM();
                    ond.Omschrijving = entry.Omschrijving;
                    ond.OnderwerpID = entry.OnderwerpID;
                   
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
            string msg = "Politiek onderwerp";

            if (OnderwerpID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Onderwerp ID!";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);

            if (onderwerpen == null)
            {
                TempData["BCmessage"] = "Ondewerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            onderwerpenVM.Omschrijving = onderwerpen.Omschrijving;
            onderwerpenVM.OnderwerpID = onderwerpen.OnderwerpID;
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
       
        public ActionResult Create([Bind(Include = "OnderwerpID,Omschrijving")]OnderwerpenVM onderwerpenVM)
        {
            string title = "Nieuw";
           

            if (ModelState.IsValid)
            {
                Onderwerpen onderwerpen = new Onderwerpen();
                onderwerpen.OnderwerpID = onderwerpenVM.OnderwerpID;
                onderwerpen.Omschrijving = onderwerpenVM.Omschrijving;
                try {
                    db.Onderwerpen.Add(onderwerpen);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // string exnum = ex.Message;
                    
                    string emsg = "Onderwerp '" + onderwerpenVM.OnderwerpID.Trim() + "' bestaat al";
                    string elevel = onderwerpenVM.MessageSection.Error;
                    onderwerpenVM.MessageSection.SetMessage(title, elevel, emsg );
                    return View(onderwerpenVM);
                }
                TempData["BCmessage"] = "Onderwerp " +onderwerpenVM.OnderwerpID.Trim() + " is nu aangemaakt";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Info;

                return RedirectToAction("Index");
            }

            string level =onderwerpenVM.MessageSection.Error;
            string msg = "ERROR - Wet " + onderwerpenVM.OnderwerpID.Trim() + " is NIET aangemaakt";
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

            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);

            if (onderwerpen == null)
            {
                TempData["BCmessage"] = "Ondewerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            onderwerpenVM.Omschrijving = onderwerpen.Omschrijving;
            onderwerpenVM.OnderwerpID = onderwerpen.OnderwerpID;
            onderwerpenVM.MessageSection.SetMessage(title, level, msg);

            return View(onderwerpenVM);

        }

        // POST: Onderwerpen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OnderwerpID,Omschrijving")] OnderwerpenVM onderwerpenVM)
        {
            string title = "Bewerken";

            if (ModelState.IsValid)
            {
                Onderwerpen onderwerpen = new Onderwerpen();
                onderwerpen.OnderwerpID = onderwerpenVM.OnderwerpID;
                onderwerpen.Omschrijving = onderwerpenVM.Omschrijving;
                db.Entry(onderwerpen).State = EntityState.Modified;
                db.SaveChanges();
                TempData["BCmessage"] = "Onderwerp " + onderwerpenVM.OnderwerpID.Trim() + " is gewijzigd";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Info;

                return RedirectToAction("Index");
            }

            string level = onderwerpenVM.MessageSection.Error;
            string msg = "ERROR - Onderwerp " + onderwerpenVM.OnderwerpID.Trim() + " is NIET gewijzigd";
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

            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);

            if (onderwerpen == null)
            {
                TempData["BCmessage"] = "Ondewerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            onderwerpenVM.Omschrijving = onderwerpen.Omschrijving;
            onderwerpenVM.OnderwerpID = onderwerpen.OnderwerpID;
            onderwerpenVM.MessageSection.SetMessage(title, level, msg);

            return View(onderwerpenVM);
        }

        // POST: Onderwerpen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string OnderwerpID)
        {
            OnderwerpenVM onderwerpenVM = new OnderwerpenVM();
            if (OnderwerpID == null)
            {
                TempData["BCmessage"] = "Specificeer een geldige Wet ID!";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

            Onderwerpen onderwerpen = db.Onderwerpen.Find(OnderwerpID);

            if (onderwerpen == null)
            {
                TempData["BCmessage"] = "Onderwerp ID " + OnderwerpID.Trim() + " is niet gevonden";
                TempData["BCerrorlevel"] = onderwerpenVM.MessageSection.Warning;

                return RedirectToAction("Error");
            }

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
