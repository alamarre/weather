namespace Weather.Application.Abstractions
{
    public interface IWeatherForecastService
    {
        string GetForecast(string location);
    }
}
