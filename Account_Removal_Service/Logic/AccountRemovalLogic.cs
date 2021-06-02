using Account_Removal_Service.Enums;
using Account_Removal_Service.Models.Helpers;
using Account_Removal_Service.RabbitMq.Publishers;

namespace Account_Removal_Service.Logic
{
    public class AccountRemovalLogic
    {
        private readonly IPublisher _publisher;

        public AccountRemovalLogic(IPublisher publisher)
        {
            _publisher = publisher;
        }

        private void RemoveUserData(AccountRemoval accountRemoval)
        {
            _publisher.Publish(accountRemoval.UserUuid, RabbitMqRouting.RemoveUserFromAuthorizationService, RabbitMqExchange.AuthorizationExchange);
            _publisher.Publish(accountRemoval.UserUuid, RabbitMqRouting.RemoveUserFromUserService, RabbitMqExchange.UserExchange);
        }

        private void RemoveDatepickerData(AccountRemoval accountRemoval)
        {
            _publisher.Publish(accountRemoval.UserUuid, RabbitMqRouting.RemoveUserFromDatepickerService, RabbitMqExchange.DatepickerExchange);
        }

        private void RemoveEventData(AccountRemoval accountRemoval)
        {
            _publisher.Publish(accountRemoval.UserUuid, RabbitMqRouting.RemoveUserFromEventService, RabbitMqExchange.EventExchange);
        }

        private void RemoveMediaData(AccountRemoval accountRemoval)
        {
            _publisher.Publish(accountRemoval.UserUuid, RabbitMqRouting.RemoveUserFromFileService, RabbitMqExchange.FileExchange);
        }

        public void RemoveAccount(AccountRemoval accountRemoval)
        {
            if (accountRemoval.DataToRemove.Contains(RemovableOptions.UserData))
            {
                RemoveUserData(accountRemoval);
            }

            if (accountRemoval.DataToRemove.Contains(RemovableOptions.DatepickerData))
            {
                RemoveDatepickerData(accountRemoval);
            }

            if (accountRemoval.DataToRemove.Contains(RemovableOptions.EventData))
            {
                RemoveEventData(accountRemoval);
            }

            if (accountRemoval.DataToRemove.Contains(RemovableOptions.MediaData))
            {
                RemoveMediaData(accountRemoval);
            }
        }
    }
}