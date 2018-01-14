using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(softechwebService.Startup))]

namespace softechwebService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}