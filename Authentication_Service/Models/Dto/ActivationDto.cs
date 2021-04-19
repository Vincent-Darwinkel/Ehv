using System;

namespace Authentication_Service.Models.Dto
{
    public class ActivationDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Code { get; set; }
    }
}
