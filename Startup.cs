using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Doancoso.Startup))]
namespace Doancoso
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
