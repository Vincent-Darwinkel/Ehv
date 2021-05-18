using System;
using User_Service.Models;

namespace UnitTest.UserService.TestModels
{
    public class TestActivationDto
    {
        public readonly ActivationDto Activation = new ActivationDto
        {
            Uuid = Guid.Parse("be0c6527-da0e-485a-b9c8-54c14122c4ca"),
            UserUuid = new TestUserDto().User.Uuid,
            Code = "fjowefiwjef"
        };
    }
}