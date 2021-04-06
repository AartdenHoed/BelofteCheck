using BelofteCheck.ViewModels;

namespace BelofteCheck
{

    public partial class Wetten
    {
        public void Fill(WettenVM vm)
        {
            this.WetID = vm.wet.WetID.Trim().ToUpper();
            this.WetLink = vm.wet.WetLink.Trim();
            this.WetNaam = vm.wet.WetNaam.Trim();
            this.WetOmschrijving = vm.wet.WetOmschrijving.Trim();
            this.WetType = vm.wet.WetType.Trim().ToUpper();

        }
    }
}