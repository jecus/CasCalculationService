using Microsoft.Extensions.DependencyInjection;

namespace CalculationService.Workers.Infrastructure
{
	public static class WorkerExtensions
	{
		public static IServiceCollection AddWorker<TWorker>(this IServiceCollection services)
			where TWorker : class, IWorker
		{
			services.AddScoped<IWorker, TWorker>();
			return services;
		}
	}
}