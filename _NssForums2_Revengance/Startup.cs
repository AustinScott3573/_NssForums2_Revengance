using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_NssForums2_Revengance.Startup))]
namespace _NssForums2_Revengance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
