using Authentication_Service.Models.HelperFiles;

namespace UnitTest.AuthenticationService.MockedDotnetClasses
{
    public class MockedJwtConfig
    {
        public readonly JwtConfig JwtConfig;

        /// <summary>
        /// Mocks the configuration file appsettings.json
        /// </summary>
        public MockedJwtConfig()
        {
            var jwtConfig = new JwtConfig
            {
                FrontendUrl = "http://test.example.com",
                Secret = "jewfjoasdfjaweoifjaweiofjwaeofijwaeifjweflg"
            };

            JwtConfig = jwtConfig;
        }
    }
}