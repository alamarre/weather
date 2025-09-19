using System;
using System.Collections.Generic;
using Weather.Domain.Entities;

namespace Weather.Domain.Repositories
{
    public interface IWeatherDataRepository
    {
        IEnumerable<WeatherData> GetWeatherData(DateTime? start, DateTime? end);
    }
}
