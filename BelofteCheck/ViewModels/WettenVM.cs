using System.Collections.Generic;

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

        private List<Stemming> _Stemmingen = new List<Stemming>();
        public List<Stemming> Stemmingen { get { return _Stemmingen; } set { _Stemmingen = value; } }

        public void Fill(List<WetObject> wl)
        // Fill view model from DB query list
        {
            this._wet.WetID = wl[0].WetID.Trim().ToUpper();
            this._wet.WetNaam = wl[0].WetNaam.Trim();
            this._wet.WetOmschrijving = wl[0].WetOmschrijving.Trim();
            this._wet.WetLink = wl[0].WetLink.Trim();
            this._wet.WetType = wl[0].WetType.Trim().ToUpper();

            foreach (WetObject wo in wl)
            {
                Onderwerp v = new Onderwerp
                {
                    Omschrijving = wo.Omschrijving.Trim(),
                    OnderwerpID = wo.OnderwerpID.Trim().ToUpper(),
                    Toelichting = wo.Toelichting.Trim()
                };
                if (string.IsNullOrEmpty(v.Toelichting))
                {
                    v.Geselecteerd = false;
                }
                else
                {
                    v.Geselecteerd = true;
                }
                this._OnderwerpenLijst.Add(v);
            }


        }
    }



}