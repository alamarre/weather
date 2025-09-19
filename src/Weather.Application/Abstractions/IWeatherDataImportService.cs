namespace Weather.Application.Abstractions
{
    public interface IWeatherDataImportService
    {
        void Import(string csvPath);
    }
}
