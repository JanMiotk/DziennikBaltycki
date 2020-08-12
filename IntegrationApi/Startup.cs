using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IntegrationApi.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using IntegrationApi.Policy.Models;
using Microsoft.AspNetCore.Authorization;
using IntegrationApi.Policy.Handlers;
using DatabaseConnection.Interfaces;
using IntegrationApi.Ninject;
using Ninject;
using IntegrationApi.Interfaces;

namespace IntegrationApi
{
    public class Startup
    {
        private IKernel kernel;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            kernel = new IntegrationApiConfig().GetKernel();
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            ReturnNinject(ref services);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddGoogle(options =>
            {
                options.ClientId = Configuration["GoogleAuthentication:ClientID"];
                options.ClientSecret = Configuration["GoogleAuthentication:ClientSecret"];
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddAuthorization(options =>
                options.AddPolicy("User", policy =>
                policy.Requirements.Add(new UserPolicyRequirement())));

            services.AddAuthorization(options =>
                options.AddPolicy("Admin", policy =>
                policy.Requirements.Add(new AdminPolicyRequirement())));

            services.AddSingleton<IAuthorizationHandler, UserPolicyHandler>();
            services.AddSingleton<IAuthorizationHandler, AdminPolicyHandler>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }
            
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.WriteLogs();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        IServiceCollection ReturnNinject(ref IServiceCollection services)
        {
            services.AddSingleton(kernel.Get<IDataBase>());
            services.AddSingleton(kernel.Get<IOccasion>());
            services.AddSingleton(kernel.Get<IDataBaseService>());

            return services;

        }
    }
}
