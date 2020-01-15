using BusinessLayer.Calculator;
using BusinessLayer.Repositiries;
using Microsoft.Extensions.DependencyInjection;

namespace CalculationService.Extentions
{
	public static class RepositoryExtentions
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<ICalculator, Calculator>();
			services.AddScoped<IAircraftRepository, AircraftRepository>();
			services.AddScoped<IComponentRepository, ComponentRepository>();
			services.AddScoped<IAircraftFlightRepository, AircraftFlightRepository>();
			services.AddScoped<IAverageUtilizationRepository, AverageUtilizationRepository>();
			return services;
		}
	}
}
