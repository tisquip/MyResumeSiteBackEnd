
using System.Net.Http;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyResumeSiteBackEnd.Areas.Identity;
using MyResumeSiteBackEnd.BackgroundWorkers;
using MyResumeSiteBackEnd.Data;

using Polly.Extensions.Http;

using Polly;

using Serilog;
using System;
using MyResumeSiteBackEnd.Hubs;
using MyResumeSiteModels;
using MyResumeSiteBackEnd.Services;

namespace MyResumeSiteBackEnd
{
    public class Startup
    {
        string _allowAllCorsName = "AllowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages();

            services.AddServerSideBlazor();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddHttpClient<BackgroundWorkerMatchScheduler>()
                .AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<BackgroundMatchBroadcaster>()
              .AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<BackgroundWorkerStandings>()
             .AddPolicyHandler(GetRetryPolicy());

            services.AddSingleton<IEmailService, EmailService>();

            services.AddHostedService<BackgroundWorkerMatchScheduler>();
            services.AddHostedService<BackgroundMatchBroadcaster>();
            services.AddHostedService<BackgroundWorkerStandings>();
            

            services.AddCors(buider =>
            {
                buider.AddPolicy(_allowAllCorsName, config =>
                {
                    config.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .Build();
                });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseCors(_allowAllCorsName);
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>(VariablesCore.MessageHubUrlEndPointWithPreSlash);
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
