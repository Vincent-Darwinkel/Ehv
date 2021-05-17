using UnitTest.UserService.MockedDals;
using User_Service.Logic;
using User_Service.Models.HelperFiles;

namespace UnitTest.UserService.MockedLogics
{
    public class MockedDisabledUserLogic
    {
        public readonly DisabledUserLogic DisabledUserLogic;

        public MockedDisabledUserLogic()
        {
            var disabledUserDal = new MockedDisabledUserDal().Mock;
            var autoMapperConfig = AutoMapperConfig.Config.CreateMapper();
            var userDal = new MockedUserDal().Mock;
            DisabledUserLogic = new DisabledUserLogic(disabledUserDal, autoMapperConfig, userDal);
        }
    }
}