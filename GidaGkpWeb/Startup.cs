using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GidaGkpWeb.Startup))]
namespace GidaGkpWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
