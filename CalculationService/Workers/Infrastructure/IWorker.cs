using System;

namespace CalculationService.Workers.Infrastructure
{
	public interface IWorker : IDisposable
	{
		void Start();
	}
}