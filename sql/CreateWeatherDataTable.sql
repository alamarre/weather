CREATE TABLE WeatherData (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StationName NVARCHAR(100) NOT NULL,
    DateTime DATETIME NOT NULL,
    TempC FLOAT NULL,
    DewPointTempC FLOAT NULL,
    RelHum INT NULL,
    PrecipAmountMm FLOAT NULL,
    WindDirDeg INT NULL,
    WindSpdKmH FLOAT NULL,
    VisibilityKm FLOAT NULL,
    StnPressKPa FLOAT NULL,
    Hmdx FLOAT NULL,
    WindChill FLOAT NULL,
    Weather NVARCHAR(50) NULL
);
