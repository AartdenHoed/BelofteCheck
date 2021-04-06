using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class WetObject
    {
        [Required]
        [DisplayName("Unieke wet ID")]
        [MaxLength(32)]
        public string WetID { get; set; }
        [Required]
        [DisplayName("Wetnaam")]
        [MaxLength(120)]
        public string WetNaam { get; set; }
        [Required]
        [DisplayName("Wetomschrijving")]
        [MaxLength(512)]
        [DataType(DataType.MultilineText)]
        public string WetOmschrijving { get; set; }
        [DisplayName("URL van deze wet")]
        [MaxLength(120)]
        public string WetLink { get; set; }
        [DisplayName("Type wetsvoorstel")]
        [Required]
        [MaxLength(16)]
        public string WetType { get; set; }
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