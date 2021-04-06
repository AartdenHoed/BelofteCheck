using BelofteCheck.ViewModels;

namespace BelofteCheck
{

    public partial class PartijZetels
    {
        public void Fill(string partijid, ZetelObject zo)
        {
            this.PartijID = partijid.Trim().ToUpper();
            this.AantalZetels = zo.AantalZetels;
            this.VanDatum = zo.VanDatum;
            this.TotDatum = zo.TotDatum;


        }
    }
}