using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BelofteCheck.Startup))]
namespace BelofteCheck
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
