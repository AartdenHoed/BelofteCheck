using BelofteCheck.ViewModels;

namespace BelofteCheck
{
    
    public partial class Wetten
    {
        public void Fill(WettenVM vm)
        {
            this.WetID = vm.wet.WetID;
            this.WetLink = vm.wet.WetLink;
            this.WetNaam = vm.wet.WetNaam;
            this.WetOmschrijving = vm.wet.WetOmschrijving;
            
        }
    }
}