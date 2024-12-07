using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Proyecto01.Startup))]
namespace Proyecto01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
