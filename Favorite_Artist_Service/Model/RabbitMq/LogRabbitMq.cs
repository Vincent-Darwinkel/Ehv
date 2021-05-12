namespace Favorite_Artist_Service.Model.RabbitMq
{
    public class LogRabbitMq
    {
        public readonly string FromMicroService = "FavoriteArtist_Service";
        public string Message { get; set; }
        public string Stacktrace { get; set; }
    }
}