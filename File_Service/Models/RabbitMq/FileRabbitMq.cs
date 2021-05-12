using System;

namespace File_Service.Models.RabbitMq
{
    public class FileRabbitMq
    {
        public string Files { get; set; }
        public string UserSpecifiedPath { get; set; }
        public Guid RequestingUserUuid { get; set; }
    }
}