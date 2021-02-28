using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelofteCheck.ViewModels
{
    public class OnderwerpenVM
    {
        public bool DeleteAllowed { get; set; }
              
        private BCmessage _MessageSection = new BCmessage();   
        public BCmessage MessageSection { get { return _MessageSection; } set { _MessageSection = value; } }

        private Onderwerp _onderwerp = new Onderwerp();
        public Onderwerp onderwerp { get { return _onderwerp; } set { _onderwerp = value; } }

        private List<WetObject> _WettenLijst = new List<WetObject>();
        public List<WetObject> WettenLijst { get { return _WettenLijst; } set { _WettenLijst = value; } }

        public void Fill(List<WetObject> wl)
        // Fill view model from DB query list
        {
            this.onderwerp.OnderwerpID = wl[0].OnderwerpID;
            this.onderwerp.Omschrijving = wl[0].Omschrijving;
            

            foreach (WetObject wo in wl)
            {
                WetObject w = new WetObject();
                w.WetOmschrijving = wo.WetOmschrijving;
                w.WetID = wo.WetID;
                w.WetNaam = wo.WetNaam;
                w.WetLink = wo.WetLink;
                w.Toelichting = wo.Toelichting;
                w.OnderwerpID = wo.OnderwerpID;
                w.Omschrijving = wo.Omschrijving;
               
                this._WettenLijst.Add(w);
            }


        }

    }
}