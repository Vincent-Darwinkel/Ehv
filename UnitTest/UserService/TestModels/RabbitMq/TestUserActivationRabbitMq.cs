using User_Service.Models.RabbitMq;

namespace UnitTest.UserService.TestModels.RabbitMq
{
    public class TestUserActivationRabbitMq
    {
        public readonly UserActivationRabbitMq UserActivation = new UserActivationRabbitMq
        {
            Uuid = new TestActivationDto().Activation.Uuid,
            UserUuid = new TestUserDto().User.Uuid,
            Code = new TestActivationDto().Activation.Code
        };
    }
}