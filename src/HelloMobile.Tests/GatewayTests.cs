using System.Threading.Tasks;
using NUnit.Framework;
using Shared.Client;

namespace HelloMobile.Tests
{
    [TestFixture]
    public class GatewayTests
    {
        [Test]
        public async Task Can_call_SayHello()
        {
            var client = new SharedGateway();

            var response = await client.SayHello("NUnit");

            Assert.That(response, Is.EqualTo("Hello, NUnit"));
        }
    }
}
