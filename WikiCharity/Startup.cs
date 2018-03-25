using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WikiCharity.Startup))]
namespace WikiCharity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
