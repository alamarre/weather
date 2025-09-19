using System.Net.Http;
using System.Threading;
using System.Web.Http;
using NUnit.Framework;
using Weather.Api;

namespace Weather.Api.Tests
{
    [TestFixture]
    public class PingControllerTests
    {
        [Test]
        public void Ping_ReturnsPong()
        {
            var config = new HttpConfiguration();
            Startup.Configure(config);

            using (var server = new HttpServer(config))
            using (var client = new HttpMessageInvoker(server))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/ping");
                var response = client.SendAsync(request, CancellationToken.None).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                Assert.AreEqual("pong", content.Trim('"'));
            }
        }
    }
}
