using System;
using System.ComponentModel.DataAnnotations;

namespace Hobby_Service.Models.FromFrontend
{
    public class Hobby
    {
        public Guid Uuid { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
