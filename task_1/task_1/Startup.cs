using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(task_1.Startup))]
namespace task_1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
