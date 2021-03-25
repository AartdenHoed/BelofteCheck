﻿using System;
using System.Collections.Generic;

namespace BelofteCheck.ViewModels
{
    public class PartijenVM
    {
        public bool DeleteAllowed { get; set; }
        public bool ZetelError { get; set; }

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
            this._partij.PartijID = p.PartijID.Trim().ToUpper();
            this._partij.PartijNaam = p.PartijNaam.Trim();

            foreach (StemObject so in sl)
            {
                StemObject s = new StemObject
                {
                    WetOmschrijving = so.WetOmschrijving.Trim(),
                    WetID = so.WetID.Trim().ToUpper(),
                    WetNaam = so.WetNaam.Trim(),
                    WetLink = so.WetLink.Trim(),
                    PartijID = so.PartijID.Trim().ToUpper(),
                    Voor = so.Voor,
                    Tegen = so.Tegen,
                    Blanco = so.Blanco
                };

                this._StemmingenLijst.Add(s);
            }
            foreach (ZetelObject zo in zl)
            {
                if (zo.IncludeIfSelected)
                {

                    ZetelObject z = new ZetelObject
                    {
                        AantalZetels = zo.AantalZetels,
                        VanDatum = zo.VanDatum,
                        TotDatum = zo.TotDatum,
                        PartijID = this._partij.PartijID,
                        IncludeIfSelected = zo.IncludeIfSelected,
                        InError = zo.InError,
                        ErrorMsg = zo.ErrorMsg

                    };
                    if ((z.VanDatum == DateTime.MinValue) && (z.TotDatum == DateTime.MinValue))
                    {
                        z.IncludeIfSelected = false;
                    }
                    this._ZetelLijst.Add(z);
                }
            }

        }

        public void CheckZetels()
        {
            this.ZetelError = false;
            List<ZetelObject> templist = new List<ZetelObject>();
            foreach (ZetelObject x in this._ZetelLijst)
            {
                if (x.IncludeIfSelected)
                {
                    ZetelObject y = new ZetelObject
                    {
                        AantalZetels = x.AantalZetels,
                        IncludeIfSelected = x.IncludeIfSelected,
                        InError = x.InError,
                        PartijID = this._partij.PartijID,
                        TotDatum = x.TotDatum,
                        VanDatum = x.VanDatum
                    };
                    templist.Add(y);
                }
            }

            this._ZetelLijst.Clear();
            templist.Sort((x, y) => DateTime.Compare(x.VanDatum, y.VanDatum));
            DateTime lasttot = DateTime.MinValue;
            foreach (ZetelObject zo in templist)
            {
                if ((zo.VanDatum == DateTime.MinValue) || !zo.IncludeIfSelected)
                {
                    continue;
                }
                zo.InError = ZetelObject.Ok;
                if (zo.VanDatum > zo.TotDatum)
                {
                    zo.InError = ZetelObject.Fout;
                    zo.ErrorMsg = "De vanaf datum is groter dan de tot-en-met datum";
                    this.ZetelError = true;
                }
                if (zo.VanDatum <= lasttot)
                {
                    zo.InError = ZetelObject.Fout;
                    zo.ErrorMsg = "De vanaf datum valt binnen de daaraan voorafgaande periode";
                    this.ZetelError = true;
                }
                this._ZetelLijst.Add(zo);
                lasttot = zo.TotDatum;
            }

            return;
        }

    }
}