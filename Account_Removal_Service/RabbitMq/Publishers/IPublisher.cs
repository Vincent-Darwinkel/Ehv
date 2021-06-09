namespace Account_Removal_Service.RabbitMq.Publishers
{
    public interface IPublisher
    {
        public void Publish(object objectToSend, string routingKey, string exchange);
    }
}