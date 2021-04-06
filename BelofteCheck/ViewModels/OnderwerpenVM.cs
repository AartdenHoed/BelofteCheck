using System.Collections.Generic;

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
            this._onderwerp.OnderwerpID = wl[0].OnderwerpID;
            this._onderwerp.Omschrijving = wl[0].Omschrijving;


            foreach (WetObject wo in wl)
            {
                WetObject w = new WetObject();
                w.WetOmschrijving = wo.WetOmschrijving.Trim();
                w.WetID = wo.WetID.Trim().ToUpper();
                w.WetNaam = wo.WetNaam.Trim();
                w.WetLink = wo.WetLink.Trim();
                w.Toelichting = wo.Toelichting.Trim();
                w.OnderwerpID = wo.OnderwerpID.Trim().ToUpper();
                w.Omschrijving = wo.Omschrijving.Trim();

                this._WettenLijst.Add(w);
            }


        }

    }
}