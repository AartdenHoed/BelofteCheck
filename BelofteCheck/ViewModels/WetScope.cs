using BelofteCheck.ViewModels;


namespace BelofteCheck
{
    
    public partial class WetScope
    {
        public void Fill(string wetid,Onderwerp o)
        {
        
            this.OnderwerpID = o.OnderwerpID;
            this.WetID = wetid;
            this.Toelichting = o.Toelichting;
          
        }
    }
}