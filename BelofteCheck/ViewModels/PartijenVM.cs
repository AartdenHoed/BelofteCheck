using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelofteCheck.ViewModels
{
    public class PartijenVM
    {
        public bool DeleteAllowed { get; set; }
              
        private BCmessage _MessageSection = new BCmessage();   
        public BCmessage MessageSection { get { return _MessageSection; } set { _MessageSection = value; } }

        private Partij _partij = new Partij();
        public Partij partij { get { return _partij; } set { _partij = value; } }

        private List<StemObject> _StemmingenLijst = new List<StemObject>();
        public List<StemObject> StemmingenLijst { get { return _StemmingenLijst; } set { _StemmingenLijst = value; } }

        public void Fill(List<StemObject> sl)
        // Fill view model from DB query list
        {
            this.partij.PartijID = sl[0].PartijID;
            this.partij.PartijNaam = sl[0].PartijNaam;
            this.partij.AantalZetels = sl[0].AantalZetels;
            this.partij.VanDatum = sl[0].VanDatum;
            this.partij.TotDatum = sl[0].TotDatum;


            foreach (StemObject so in sl)
            {
                StemObject s = new StemObject();
                s.WetOmschrijving = so.WetOmschrijving;
                s.WetID = so.WetID;
                s.WetNaam = so.WetNaam;
                s.WetLink = so.WetLink;
                s.PartijID = so.PartijID;
                s.PartijNaam = so.PartijNaam;
                s.AantalZetels = so.AantalZetels;
                s.VanDatum = so.VanDatum;
                s.TotDatum = so.TotDatum;
                s.Voor = so.Voor;
                s.Tegen = so.Tegen;
                s.Blanco = so.Blanco;

                this._StemmingenLijst.Add(s);
            }


        }

    }
}