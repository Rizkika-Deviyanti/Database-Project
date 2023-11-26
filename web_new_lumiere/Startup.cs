using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_new_lumiere.Startup))]
namespace web_new_lumiere
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
