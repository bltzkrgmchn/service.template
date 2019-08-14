using Microsoft.AspNetCore.Builder;
using Nancy.Owin;

namespace Facade.Scaffold.Instance
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());
        }
    }
}