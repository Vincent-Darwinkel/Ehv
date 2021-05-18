using Authentication_Service.Logic;
using Authentication_Service.Models.HelperFiles;
using AutoMapper;
using UnitTest.AuthenticationService.MockDals;

namespace UnitTest.AuthenticationService.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            var mockedUserDal = new MockedUserDal().Mock;
            IMapper mapper = AutoMapperConfig.Config.CreateMapper();
            UserLogic = new UserLogic(mockedUserDal, new SecurityLogic(), mapper);
        }
    }
}
