using System;
using Microsoft.Owin.Hosting;

namespace Weather.Api
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:9000/";
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Server running on " + baseAddress);
                Console.ReadLine();
            }
        }
    }
}
