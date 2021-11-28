using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrudWithImage1.Startup))]
namespace CrudWithImage1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
