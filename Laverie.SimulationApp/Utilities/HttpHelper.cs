                                             using System.Net.Http;
using System.Threading.Tasks;

namespace Laverie.SimulationApp.Utilities
{
    public static class HttpHelper
    {
        public static async Task<string> GetAsync(HttpClient httpClient, string url)
        {
            return await httpClient.GetStringAsync(url);
        }
    }
}

