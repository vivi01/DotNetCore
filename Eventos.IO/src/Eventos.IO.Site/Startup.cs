using System.Reflection;
using AutoMapper;
using Eventos.IO.Application.AutoMapper;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Infra.CrossCutting.Bus;
using Eventos.IO.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventos.IO.Site.Data;
using Eventos.IO.Site.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eventos.IO.Infra.CrossCutting.AspNetFilters;
using Microsoft.Extensions.Logging;

namespace Eventos.IO.Site
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
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddAuthorization(options =>
			{
				options.AddPolicy("PodeLerEventos", policy => policy.RequireClaim("Eventos", "Ler"));
				options.AddPolicy("PodeGravar", policy => policy.RequireClaim("Eventos", "Gravar"));

			});

			services.AddMvc(options =>
			{
				options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalExceptionHandlingFilter)));
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperConfiguration)));

			services.AddScoped<IUser, AspNetUser>();

			//Add application services
			RegisterServices(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
			IHttpContextAccessor accessor,  ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/erro-de-aplicacao");
				app.UseStatusCodePagesWithReExecute("/erro-de-aplicacao/{0}");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			InMemoryBus.ContainerAccessor = () => accessor.HttpContext.RequestServices;
		}

		private static void RegisterServices(IServiceCollection services)
		{
			NativeInjectorBootStrapper.RegisterServices(services);
		}
	}
}
