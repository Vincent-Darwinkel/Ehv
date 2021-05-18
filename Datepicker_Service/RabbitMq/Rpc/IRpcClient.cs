namespace Datepicker_Service.RabbitMq.Rpc
{
    public interface IRpcClient
    {
        public T Call<T>(object objectToSend, string queue);
    }
}