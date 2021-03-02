
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class ZetelObject
    {
        
        public string PartijID { get; set; }
        public int AantalZetels { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime VanDatum { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime TotDatum { get; set; }
        
    }
}