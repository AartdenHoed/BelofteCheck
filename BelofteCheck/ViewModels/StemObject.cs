
using System;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class StemObject
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public string WetID { get; set; }
        public string WetNaam { get; set; }
        public string WetOmschrijving { get; set; }
        public string WetLink { get; set; }
        public string WetType { get; set; }
        public string PartijID { get; set; }
        public string PartijNaam { get; set; }
        public int PartijZetels { get; set; }
        public int Voor { get; set; }
        public int Tegen { get; set; }
        public int Blanco { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StemDatum { get; set; }
    }
}