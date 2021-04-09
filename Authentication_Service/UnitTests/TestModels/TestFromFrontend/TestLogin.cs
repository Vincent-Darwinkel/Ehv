using Authentication_Service.Models.FromFrontend;

namespace Authentication_Service.UnitTests.TestModels.TestFromFrontend
{
    public class TestLogin
    {
        public Login Login = new Login
        {
            Username = new TestUserDto().User.Username,
            Password = "test"
        };
    }
}