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

        private List<ZetelObject> _ZetelLijst = new List<ZetelObject>();
        public List<ZetelObject> ZetelLijst { get { return _ZetelLijst; } set { _ZetelLijst = value; } }

        private List<StemObject> _StemmingenLijst = new List<StemObject>();
        public List<StemObject> StemmingenLijst { get { return _StemmingenLijst; } set { _StemmingenLijst = value; } }

        public void Fill(Partijen p, List<StemObject> sl, List<ZetelObject> zl)
        // Fill view model from DB query list
        {
            this.partij.PartijID = p.PartijID;
            this.partij.PartijNaam = p.PartijNaam;
           
            foreach (StemObject so in sl)
            {
                StemObject s = new StemObject
                {
                    WetOmschrijving = so.WetOmschrijving,
                    WetID = so.WetID,
                    WetNaam = so.WetNaam,
                    WetLink = so.WetLink,
                    PartijID = so.PartijID,
                    Voor = so.Voor,
                    Tegen = so.Tegen,
                    Blanco = so.Blanco
                };

                this._StemmingenLijst.Add(s);
            }
            foreach (ZetelObject zo in zl)
            {
                ZetelObject z = new ZetelObject
                {
                    AantalZetels = zo.AantalZetels,
                    VanDatum = zo.VanDatum,
                    TotDatum = zo.TotDatum,
                    PartijID = zo.PartijID
                };

                this._ZetelLijst.Add(z);
            }


        }

    }
}