using Authentication_Service.Models.Dto;

namespace UnitTest.AuthenticationService.TestModels
{
    public class TestPendingLoginDto
    {
        public readonly PendingLoginDto SiteAdmin = new PendingLoginDto
        {
            AccessCode = 392033,
            UserUuid = new TestUserDto().SiteAdmin.Uuid,
        };
    }
}