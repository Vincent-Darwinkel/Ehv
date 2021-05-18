using Authentication_Service.Models.RabbitMq;

namespace UnitTest.AuthenticationService.TestModels.RabbitMq
{
    public class TestRabbitMqUserSensitiveInformation
    {
        public UserRabbitMqSensitiveInformation SiteAdminFromUserService = new UserRabbitMqSensitiveInformation
        {
            AccountRole = new TestUserDto().SiteAdmin.AccountRole,
            Username = new TestUserDto().SiteAdmin.Username,
            Uuid = new TestUserDto().SiteAdmin.Uuid
        };
    }
}