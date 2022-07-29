using App.API.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Tests
{
    internal class App : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Tests");

            builder.ConfigureServices(services =>
            {
                services.Remove(services.First(x => x.ServiceType == typeof(AppDbContext)));
                services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase(nameof(AppDbContext)));
            });

            return base.CreateHost(builder);
        }
    }
}
