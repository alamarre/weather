using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weather.Application.Abstractions;
using Weather.Domain.Entities;
using Weather.Domain.Repositories;

namespace Weather.Application.Services
{
    public class WeatherDataService : IWeatherDataService
    {
        private readonly IWeatherDataRepository _repository;

        public WeatherDataService(IWeatherDataRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<WeatherData>> GetWeatherData(DateTime? start, DateTime? end, CancellationToken cancellationToken = default)
        {
            return await _repository.GetWeatherData(start, end, cancellationToken);
        }
    }
}
