

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class Partij   {
        

        [Required]
        [DisplayName("Unieke partij ID")]
        [MaxLength(8)]        
        public string PartijID { get; set; }

        [Required]
        [DisplayName("Partijnaam")]
        [MaxLength(32)]
        public string PartijNaam { get; set; }

       
               
    }
}