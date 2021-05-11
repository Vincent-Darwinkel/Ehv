using AutoMapper;
using Hobby_Service.Models.FromFrontend;
using Hobby_Service.Models.ToFrontend;

namespace Hobby_Service.Models.Helpers
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Hobby, HobbyDto>();
                cfg.CreateMap<HobbyDto, HobbyViewmodel>();
            });
    }
}
