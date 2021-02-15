using BelofteCheck.ViewModels;

namespace BelofteCheck
{
    
    public partial class Wetten
    {
        public void Fill(WettenVM vm)
        {
            this.WetID = vm.WetID;
            this.WetLink = vm.WetLink;
            this.WetNaam = vm.WetNaam;
            this.WetOmschrijving = vm.WetOmschrijving;
            
        }
    }
}