using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using SudokuApp.Api.Extensions;
using SudokuApp.Common;
using SudokuApp.Common.Middleware;
using SudokuApp.Common.Middleware.Logging;
using SudokuApp.Data.Config;

namespace SudokuApp.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            env.ConfigureNLog($"nlog.{env.EnvironmentName}.config");
            
            var configBuilder = new ConfigurationBuilder()
                               .SetBasePath(env.ContentRootPath)
                               .AddJsonFile("appsettings.json")
                               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                               .AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SudokuAppDbContext>(options =>
            {
                options.UseSqlServer(connection).ConfigureWarnings(warnings =>
                                   warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAutoMapper();

            services.AddMemoryCache();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
            });

            services.AddOptionsConfiguration(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<AutofacModule>();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();

            var autofacServiceProvider = new AutofacServiceProvider(container);

            return autofacServiceProvider;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<LogResponseMiddleware>();
            app.UseMiddleware<LogRequestMiddleware>();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UsePageNotFoundHandlingMiddleware();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sudoku}/{action=Board}/{id?}");
            });
        }
    }
}
