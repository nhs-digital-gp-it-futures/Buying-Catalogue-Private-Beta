using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace GifAuthenticationTester
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("https://localhost:44365");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadLine();
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "a3c677e9-1867-4456-883a-4f6fdca9ba6e", "5Xe4mJi6Rcb4xL+3y/91XAqPwaUG3Dj9DVkuWE2wZxQ=");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("GIFBuyingCatalogue");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadLine();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("https://localhost:44365/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                Console.ReadLine();
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));

                Console.ReadLine();
            }
        }
    }
}
    
