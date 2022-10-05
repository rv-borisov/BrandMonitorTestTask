using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DI
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITasksService, TasksService>();
        }
    }
}
