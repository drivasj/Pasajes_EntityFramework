using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Udemy.Startup))]
namespace Udemy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
