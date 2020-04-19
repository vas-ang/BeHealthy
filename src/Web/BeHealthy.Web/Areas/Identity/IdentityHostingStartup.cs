using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BeHealthy.Web.Areas.Identity.IdentityHostingStartup))]

namespace BeHealthy.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
