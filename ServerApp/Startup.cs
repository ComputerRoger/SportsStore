using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using ServerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ServerApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//  Ensure that seed data is applied when ASP.Net Core starts.
			string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
			services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

			//services.AddControllersWithViews();
			//	Add additional instructions to tell the JSON serializer to omit properties that are null.
			services.AddControllersWithViews()
				.AddJsonOptions(option =>
			   {
				   option.JsonSerializerOptions.IgnoreNullValues = true;
			   }).AddNewtonsoftJson();

			services.AddRazorPages();

			//	Provide a description of the web service.
			services.AddSwaggerGen(options =>
		   {
			   options.SwaggerDoc("v1", new OpenApiInfo
			   {
				   Title = "Sports Store API",
				   Version = "v1"
			   });
		   });

			//	Tell framework to use SQL Server as a data cache and provide the full location of the table.
			services.AddDistributedSqlServerCache(options =>
		   {
			   options.ConnectionString = connectionString;
			   options.SchemaName = "dbo";
			   options.TableName = "SessionData";
		   });

			//	Provide the session state and configure the cookie to be used to identify sessions.
			services.AddSession(options =>
		   {
			   options.Cookie.Name = "SportsStore.Session";
			   options.IdleTimeout = System.TimeSpan.FromHours(48);
			   options.Cookie.HttpOnly = false;
			   options.Cookie.IsEssential = true;
		   });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			//	Enable sessions in the application.
			app.UseSession();

			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
		   {
			   endpoints.MapControllerRoute(
				   name: "default",
				   pattern: "{controller=Home}/{action=Index}/{id?}");

			   //	Allowing direct navigation during development.
			   //	Note:  if spaces are within regex(), then the pattern does NOT match.

			   //	Note:  "nonfile" is required because the name of the file that will be requested
			   //			when the dynamic module loaded is admin-admin-module.js
			   //			and CARE MUST BE TAKEN not to direct requests for this file to MVC.
			   endpoints.MapControllerRoute(
				  name: "angular_fallback",
				  //pattern: "{target:regex(table|detail)}/{*catchall}",
				  pattern: "{target:regex(admin|store|cart|checkout):nonfile}/{*catchall}",
				  defaults: new { controller = "Home", action = "Index" });
		   });

			app.UseSwagger();
			app.UseSwaggerUI(options =>
		   {
			   options.SwaggerEndpoint("/swagger/v1/swagger.json", "SportsStore API");
		   });

			app.UseSpa(spa =>
		   {
			   string strategy = Configuration.GetValue<string>("DevTools:ConnectionStrategy");

			   if (strategy == "proxy")
			   {
				   Uri angularServerUri = new Uri("http://127.0.0.1:4200");
				   spa.UseProxyToSpaDevelopmentServer(angularServerUri);
			   }
			   else if (strategy == "managed")
			   {
				   spa.Options.SourcePath = "../ClientApp";

				   spa.UseAngularCliServer(npmScript: "start");
			   }
			   else
			   {
				   //  Do nothing.
			   }
		   });

			//  Seed the database with initial data if it is empty.
			DataContext dataContext;
			dataContext = serviceProvider.GetRequiredService<DataContext>();
			SeedData.SeedDatabase(dataContext);
		}
	}
}
