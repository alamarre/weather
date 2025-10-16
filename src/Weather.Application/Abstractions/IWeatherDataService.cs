using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weather.Domain.Entities;

namespace Weather.Application.Abstractions
{
    public interface IWeatherDataService
    {
        async Task<IEnumerable<WeatherData>> GetWeatherData(DateTime? start, DateTime? end, CancellationToken cancellationToken = default);
    }
}
