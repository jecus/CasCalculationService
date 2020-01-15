using System.Threading.Tasks;
using CalculationService.Extentions;
using CalculationService.Workers;
using CalculationService.Workers.Infrastructure;
using Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;

namespace CalculationService
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddWorker<AircraftWorker>();
			services.AddWorker<AircraftFlightWorker>();
			services.AddWorker<BaseComponentWorker>();
			services.AddMvc().AddJsonOptions(options =>
			{
				options.SerializerSettings.Converters.Add(new StringEnumConverter());
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddDbContext<DatabaseContext>(builder => builder.UseSqlServer(Configuration.GetConnectionString("CORE_CONNECTION_STRING")));
			services.AddRepositories();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Version = "v1",
					Title = "Calculator API",
					Description = "CAS API Calculator"
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
			});

			Initialize(app);
		}

		public virtual void Initialize(IApplicationBuilder app)
		{
			var scope = app.ApplicationServices.CreateScope();
			var workers = scope.ServiceProvider.GetServices<IWorker>();
			foreach (var worker in workers)
				Task.Run(() => worker.Start());
		}
	}
}
