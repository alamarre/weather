using System;
using System.Collections.Generic;
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

        public IEnumerable<WeatherData> GetWeatherData(DateTime? start, DateTime? end)
        {
            return _repository.GetWeatherData(start, end);
        }
    }
}
