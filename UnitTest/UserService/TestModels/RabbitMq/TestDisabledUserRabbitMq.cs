using UnitTest.UserService.TestModels.FromFrontend;
using User_Service.Models.RabbitMq;

namespace UnitTest.UserService.TestModels.RabbitMq
{
    public class TestDisabledUserRabbitMq
    {
        public readonly DisabledUserRabbitMq User = new DisabledUserRabbitMq
        {
            Reason = new TestDisabledUser().DisabledUser.Reason,
            UserUuid = new TestDisabledUser().DisabledUser.UserUuid
        };
    }
}