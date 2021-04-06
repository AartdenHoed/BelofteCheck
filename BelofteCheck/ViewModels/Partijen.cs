using BelofteCheck.ViewModels;

namespace BelofteCheck
{

    public partial class Partijen
    {
        public void Fill(PartijenVM vm)
        {
            this.PartijID = vm.partij.PartijID;
            this.PartijNaam = vm.partij.PartijNaam;


        }
    }
}