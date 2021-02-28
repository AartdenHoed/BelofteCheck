
namespace BelofteCheck.ViewModels
{
    public class StemObject
    {
        public string WetID { get; set; }
        public string WetNaam { get; set; }
        public string WetOmschrijving { get; set; }
        public string WetLink { get; set; }
        public string PartijID { get; set; }
        public string PartijNaam { get; set; }
        public int AantalZetels { get; set; }
        public System.DateTime VanDatum { get; set; }
        public System.DateTime TotDatum { get; set; }
        public int Voor { get; set; }
        public int Tegen { get; set; }
        public int Blanco { get; set; }
    }
}