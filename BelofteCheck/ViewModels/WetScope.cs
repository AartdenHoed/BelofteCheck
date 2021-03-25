using BelofteCheck.ViewModels;


namespace BelofteCheck
{

    public partial class WetScope
    {
        public void Fill(string wetid, Onderwerp o)
        {

            this.OnderwerpID = o.OnderwerpID.Trim().ToUpper();
            this.WetID = wetid.Trim().ToUpper();
            this.Toelichting = o.Toelichting.Trim();

        }
    }
}