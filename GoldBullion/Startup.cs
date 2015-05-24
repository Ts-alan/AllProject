using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoldBullion.Startup))]
namespace GoldBullion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
