using UnitTest.UserService.MockedDals;
using User_Service.Logic;
using User_Service.Models.HelperFiles;

namespace UnitTest.UserService.MockedLogics
{
    public class MockedActivationLogic
    {
        public readonly ActivationLogic ActivationLogic;

        public MockedActivationLogic()
        {
            var activationDal = new MockedActivationDal().Mock;
            var disabledUserDal = new MockedDisabledUserDal().Mock;
            var autoMapperConfig = AutoMapperConfig.Config.CreateMapper();
            ActivationLogic = new ActivationLogic(activationDal, disabledUserDal, autoMapperConfig);
        }
    }
}