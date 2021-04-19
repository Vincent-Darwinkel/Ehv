using AutoMapper;
using User_Service.Logic;
using User_Service.Models.HelperFiles;
using User_Service.UnitTests.MockedDals;

namespace User_Service.UnitTests.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            var mockedUserDal = new MockedUserDal().Mock;
            UserLogic = new UserLogic(mockedUserDal, new Mapper(AutoMapperConfig.Config), null);
        }
    }
}
