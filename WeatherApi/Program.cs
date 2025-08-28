using System;
using Microsoft.Owin.Hosting;

namespace WeatherApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = "http://localhost:9000/";
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Server running on " + baseAddress);
                Console.ReadLine();
            }
        }
    }
}
