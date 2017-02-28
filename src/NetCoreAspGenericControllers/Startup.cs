using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreAspGenericControllers.Controllers;
using NetCoreAspGenericControllers.Repository;
using NetCoreAspGenericControllers.Abstract;
using NetCoreAspGenericControllers.Data;
using Microsoft.EntityFrameworkCore;
using Scheduler.API.ViewModels.Mappings;
using Newtonsoft.Json.Serialization;
using NetCoreAspGenericControllers.Model;
using Swashbuckle.AspNetCore.Swagger;

namespace NetCoreAspGenericControllers
{
	public class Startup
	{
		public IConfigurationRoot Configuration { get; }

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsEnvironment("Development"))
			{
				// This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
				builder.AddApplicationInsightsSettings(developerMode: true);
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddApplicationInsightsTelemetry(Configuration);

			// Add database Context
			services.AddDbContext<DomainContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
			);

			// Add Repositories to DI Service Container
			services.AddScoped<IEntityBaseRepository<Schedule>, ScheduleRepository>();
			services.AddScoped<IEntityBaseRepository<User>, UserRepository>();
			services.AddScoped<IEntityBaseRepository<Attendee>, AttendeeRepository>();

			// Automapper Configuration
			AutoMapperConfiguration.Configure();

			// Enable Cors
			services.AddCors();

			// Add MVC services to the services container
			services.AddMvc()
				.ConfigureApplicationPartManager(manager =>
				{
					// Custom Controller Provider
					manager.FeatureProviders.Add(new CustomControllerFeatureProvider());
				})
				.AddJsonOptions(opts =>
				{
					// Force Camel Case to JSON
					opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});

			// If you need access to generic IConfigurationRoot this is **required**
			services.AddSingleton(Configuration);

			// Register the Swagger generator, defining one or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = Configuration["Swagger:Title"], Version = Configuration["Swagger:Version"] });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseApplicationInsightsRequestTelemetry();

			app.UseApplicationInsightsExceptionTelemetry();

			app.UseMvc();

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(Configuration["Swagger:EndPoint"],
					string.Format("{0} {1}", Configuration["Swagger:Title"], Configuration["Swagger:Version"])
				);
			});

			// Get Service and Ensure Database is Created before Initialize Context
			var serviceDomainContext = app.ApplicationServices.GetRequiredService<DomainContext>();
			serviceDomainContext.Database.EnsureCreated();
			DomainContextInitializer.Initialize(app.ApplicationServices);
		}
	}
}
