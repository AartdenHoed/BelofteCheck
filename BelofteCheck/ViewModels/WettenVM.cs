using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelofteCheck.ViewModels
{
    public class WettenVM
    {
        public string WetID { get; set; }
        public string WetNaam { get; set; }
        public string WetOmschrijving { get; set; }
        public string WetLink { get; set; }

        public List<OnderwerpenVM> OnderwerpenLijst = new List<OnderwerpenVM>();

        public void Fill(List<WetObject> wl)
        // Fill view model from DB query list
        {
            this.WetID = wl[0].WetID;
            this.WetNaam = wl[0].WetNaam;
            this.WetOmschrijving = wl[0].WetOmschrijving;
            this.WetLink = wl[0].WetLink;
         
            foreach (WetObject wo in wl)
            {
                OnderwerpenVM v = new OnderwerpenVM();
                v.Omschrijving = wo.Omschrijving;
                v.OnderwerpID = wo.OnderwerpID;
                v.Toelichting = wo.Toelichting;
                this.OnderwerpenLijst.Add(v);
            }


        }
    }



}