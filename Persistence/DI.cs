using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DI
    {
        public static void AddPersistence(this IServiceCollection services, string connection)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));
            services.AddScoped<IDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        }
    }
}
