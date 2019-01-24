using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Mime;

namespace ServerSideBlazor.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds the Server-Side Blazor services, and those registered by the app project's startup.
            services.AddServerSideBlazor<App.Startup>();





            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });    

            var clientId = "927949632708-utjpm9s428sqd8equit6rfadpr9ht65n.apps.googleusercontent.com";

            services.AddAuthentication()
                .AddOpenIdConnect(
                    authenticationScheme: "Google",
                    displayName: "Google",
                    options =>
                    {
                        options.Authority = "https://accounts.google.com/";
                        options.ClientId = clientId;

                        options.CallbackPath = "/signin-google";

                        // TODO: Do I need these???
                        //options.SignedOutCallbackPath = "/signout-callback-google";
                        //options.RemoteSignOutPath = "/signout-google";

                        options.Scope.Add("email");
                    });

            services.AddMvc(options => { })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);






            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                // Allows Blazor Client project code to call Blazor
                // Server project pages
                routes.MapRoute(name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Use component registrations and static files from the app project.
            app.UseServerSideBlazor<App.Startup>();
        }
    }
}
