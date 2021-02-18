using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelofteCheck.ViewModels
{
    public class WettenVM
    {
        private BCmessage _MessageSection = new BCmessage();
        
        public BCmessage MessageSection { get { return _MessageSection; } set { _MessageSection = value; } }

        private Wet _wet = new Wet();
        public Wet wet { get { return _wet; } set { _wet = value; } }

        private List<Onderwerp> _OnderwerpenLijst = new List<Onderwerp>();
        public List<Onderwerp> OnderwerpenLijst { get { return _OnderwerpenLijst; } set { _OnderwerpenLijst = value; } }


        public void Fill(List<WetObject> wl)
        // Fill view model from DB query list
        {
            this.wet.WetID = wl[0].WetID;
            this.wet.WetNaam = wl[0].WetNaam;
            this.wet.WetOmschrijving = wl[0].WetOmschrijving;
            this.wet.WetLink = wl[0].WetLink;
         
            foreach (WetObject wo in wl)
            {
                Onderwerp v = new Onderwerp();
                v.Omschrijving = wo.Omschrijving;
                v.OnderwerpID = wo.OnderwerpID;
                v.Toelichting = wo.Toelichting;
                this.OnderwerpenLijst.Add(v);
            }


        }
    }



}