using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class KamerZetels
    {
        public String PartijID { get; set; }
        public String PartijNaam { get; set; }
        public int AantalZetels { get; set; }
    }
    public class TijdVak
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Periode start")]
        public DateTime VanDatum { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Periode einde")]
        public DateTime TotDatum { get; set;  }

        public List<KamerZetels> _ZetelVerdeling = new List<KamerZetels>();
        public List<KamerZetels> ZetelVerdeling { get { return _ZetelVerdeling; } set { _ZetelVerdeling = value; } }
        public int ZetelTotaal { get
            {
                int r = 0;
                foreach (KamerZetels kz in _ZetelVerdeling) {
                    r += kz.AantalZetels;
                }
                return r;
            } }

        [MaxLength(4)]
        public string ZetelCheck
        {
            get
            {
                if (ZetelTotaal == 150)
                {
                    return "ok";
                }
                else
                {
                    return "fout";
                }
            }
        }
    }
    public class ZetelControleVM
    {
        private BCmessage _MessageSection = new BCmessage();
        public BCmessage MessageSection { get { return _MessageSection; } set { _MessageSection = value; } }

        private List<TijdVak> _Periodes = new List<TijdVak>();
        public List<TijdVak> Periodes { get { return _Periodes; } set { _Periodes = value; } }
    }



}