using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrdersWebsite.Startup))]
namespace OrdersWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
