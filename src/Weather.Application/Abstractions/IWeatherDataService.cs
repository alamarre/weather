using System;
using System.Collections.Generic;
using Weather.Domain.Entities;

namespace Weather.Application.Abstractions
{
    public interface IWeatherDataService
    {
        IEnumerable<WeatherData> GetWeatherData(DateTime? start, DateTime? end);
    }
}
