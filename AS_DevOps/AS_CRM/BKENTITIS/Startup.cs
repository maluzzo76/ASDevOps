using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AS_CRM.Startup))]
namespace AS_CRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
