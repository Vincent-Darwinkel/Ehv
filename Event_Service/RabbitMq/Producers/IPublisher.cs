namespace Event_Service.RabbitMq.Producers
{
    public interface IPublisher
    {
        public void Publish(object objectToSend, string routingKey, string exchange);
    }
}