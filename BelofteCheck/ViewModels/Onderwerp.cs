

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class Onderwerp
    {
        [Required]      
        public bool Geselecteerd { get; set; }

        [Required]
        [DisplayName("Unieke onderwerp ID")]
        [MaxLength(32)]
        
        public string OnderwerpID { get; set; }

        [Required]
        [DisplayName("Onderwerp omschrijving")]
        [MaxLength(512)]
        [DataType(DataType.MultilineText)]
        public string Omschrijving { get; set; }

        [DisplayName("Toelichting op onderwerpkeuze")]
        [MaxLength(512)]
        [DataType(DataType.MultilineText)]
        public string Toelichting { get; set; }

    }
}