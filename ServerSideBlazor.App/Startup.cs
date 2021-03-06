using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServerSideBlazor.App.Services;

namespace ServerSideBlazor.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Since Blazor is running on the server, we can use an application service
            // to read the forecast data.
            services.AddSingleton<WeatherForecastService>();

            services.AddScoped<IUserService, UserService>();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
