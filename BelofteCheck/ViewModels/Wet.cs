using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BelofteCheck.ViewModels
{
    public class Wet
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

        


    }
}