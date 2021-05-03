
using System;
using System.ComponentModel.DataAnnotations;

namespace BelofteCheck.ViewModels
{
    public class Stemming
    {
        
        public string WetID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StemDatum { get; set; }
        public int Voor { get; set; }
        public int Tegen { get; set; }
        public int Blanco { get; set; }

        
    }
}