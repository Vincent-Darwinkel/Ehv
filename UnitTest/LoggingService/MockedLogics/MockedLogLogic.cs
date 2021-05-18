using Logging_Service.Logic;
using Logging_Service.Models.Helpers;
using UnitTest.LoggingService.MockedDals;

namespace UnitTest.LoggingService.MockedLogics
{
    public class MockedLogLogic
    {
        public readonly LogLogic LogLogic;

        public MockedLogLogic()
        {
            var autoMapperConfig = AutoMapperConfig.Config.CreateMapper();
            var logDal = new MockedLogDal().Mock;
            LogLogic = new LogLogic(logDal, autoMapperConfig);
        }
    }
}