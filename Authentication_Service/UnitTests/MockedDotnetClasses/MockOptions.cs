using Authentication_Service.Models.HelperFiles;
using Microsoft.Extensions.Options;

namespace Authentication_Service.UnitTests.MockedDotnetClasses
{
    public class MockOptions
    {
        public readonly IOptions<JwtConfig> JwtConfig;

        /// <summary>
        /// Mocks the configuration file appsettings.json
        /// </summary>
        public MockOptions()
        {
            var jwtConfig = new JwtConfig
            {
                FrontendUrl = "http://test.example.com",
                Secret = "jewfjoasdfjaweoifjaweiofjwaeofijwaeifjweflg"
            };

            JwtConfig = Options.Create(jwtConfig);
        }
    }
}