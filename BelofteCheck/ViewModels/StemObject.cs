
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class StemObject
    {
        public string WetID { get; set; }
        public string WetNaam { get; set; }
        public string WetOmschrijving { get; set; }
        public string WetLink { get; set; }
        public string PartijID { get; set; }
        public int Voor { get; set; }
        public int Tegen { get; set; }
        public int Blanco { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime StemDatum { get; set; }
    }
}