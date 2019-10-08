using Microsoft.Extensions.Configuration;

namespace ProjectName.Web.E2ETests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}