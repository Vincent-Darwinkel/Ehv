namespace User_Service.RabbitMq.Publishers
{
    public interface IUserPublisher
    {
        public void Publish(object objectToSend, string routingKey);
    }
}