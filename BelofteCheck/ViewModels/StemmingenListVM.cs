using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class StemmingenListVM

    {
        public bool ModelOk { get; set; }
        public bool ZetelsOk { get; set; }
        public string WetID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StemDatum { get; set; }
        public int TotaalVoor { get { return MyTotal("v"); } }
        public int TotaalTegen { get { return MyTotal("t"); } }
        public int TotaalBlanco { get { return MyTotal("b"); } }
        private BCmessage _MessageSection = new BCmessage();
        public BCmessage MessageSection { get { return _MessageSection; } set { _MessageSection = value; } }

        private List<StemObject> _StemLijst = new List<StemObject>();
        public List<StemObject> StemLijst { get { return _StemLijst; } set { _StemLijst = value; } }

        private int MyTotal(string type)
        {
            int thistotal = 0;
            foreach (StemObject so in this.StemLijst)
            {
                switch (type)
                {
                    case "v":
                        thistotal = thistotal + so.Voor;
                        break;
                    case "t":
                        thistotal = thistotal + so.Tegen;
                        break;
                    case "b":
                        thistotal = thistotal + so.Blanco;
                        break;

                }
            }
            return thistotal;
        }

        public void Fill(List<StemObject> sl)
        // Fill view model from DB query list
        {


            foreach (StemObject so in sl)
            {
                StemObject s = new StemObject
                {
                    WetOmschrijving = so.WetOmschrijving.Trim(),
                    WetID = so.WetID.Trim().ToUpper(),
                    WetNaam = so.WetNaam.Trim(),
                    WetLink = so.WetLink.Trim(),
                    WetType = so.WetType,
                    PartijID = so.PartijID,
                    PartijNaam = so.PartijNaam,
                    StemDatum = so.StemDatum,
                    Voor = so.Voor,
                    Tegen = so.Tegen,
                    Blanco = so.Blanco,
                    PartijZetels = so.PartijZetels
                };


                this._StemLijst.Add(s);
            }


        }
    }
}