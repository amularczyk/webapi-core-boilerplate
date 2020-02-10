using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectName.Api.E2ETests
{
    public class ApiTestsBase
    {
        protected static readonly string ItemsUrl = "api/items";

        protected static StringContent GetContent<T>(T request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }

        protected static async Task<T> GetResult<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}