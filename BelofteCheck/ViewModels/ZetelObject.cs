
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class ZetelObject
    {
        [DisplayName("Selecteren om te behouden")]
        public bool IncludeIfSelected { get; set; }
        [DisplayName("Datum check")]
        public string InError { get; set; }
        public static string Fout = "fout";
        public static string Ok = "ok";
        public string ErrorMsg { get; set; }
        [DisplayName("Unieke partij ID")]
        public string PartijID { get; set; }

        [Range(0, 150, ErrorMessage = "Minimaal 0, maximaal 150")]
        [DisplayName("Aantal zetels")]
        public int AantalZetels { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Vanaf datum")]
        public System.DateTime VanDatum { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Tot en met datum")]
        public System.DateTime TotDatum { get; set; }
        
    }
}