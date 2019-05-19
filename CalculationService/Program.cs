using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CalculationService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("AppSettings.json")
				.Build();

			return WebHost.CreateDefaultBuilder(args)
				.ConfigureLogging(builder => builder.ClearProviders())
				.UseStartup<Startup>()
				.UseConfiguration(config);
		}
	}
}
