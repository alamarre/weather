using System;

namespace WeatherApi.Models
{
    public class WeatherData
    {
        public DateTime DateTime { get; set; }
        public double? TempC { get; set; }
        public double? DewPointTempC { get; set; }
        public int? RelHum { get; set; }
        public double? PrecipAmountMm { get; set; }
        public int? WindDirDeg { get; set; }
        public double? WindSpdKmH { get; set; }
        public double? VisibilityKm { get; set; }
        public double? StnPressKPa { get; set; }
        public double? Hmdx { get; set; }
        public double? WindChill { get; set; }
        public string Weather { get; set; }
    }
}
