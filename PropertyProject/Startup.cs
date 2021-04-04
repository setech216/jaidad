using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PropertyProject.Startup))]
namespace PropertyProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
