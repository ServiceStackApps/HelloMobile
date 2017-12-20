using System.Threading.Tasks;
using ServiceModel;
using ServiceStack;

namespace Shared.Client
{
    public class SharedGateway
    {
        public IServiceClient ServiceClient { get; set; }

        public SharedGateway(string url = null)
        {
            ServiceClient = new JsonServiceClient(url ?? Config.BaseUrl);
        }

        public async Task<string> SayHello(string name)
        {
            var response = await ServiceClient.GetAsync(new Hello { Name = name });
            return response.Result;
        }
    }
}
