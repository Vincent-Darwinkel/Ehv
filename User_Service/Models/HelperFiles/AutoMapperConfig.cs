using AutoMapper;
using User_Service.Models.FromFrontend;
using User_Service.Models.RabbitMq;
using User_Service.Models.ToFrontend;

namespace User_Service.Models.HelperFiles
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<User, UserRabbitMq>();
                cfg.CreateMap<User, UserRabbitMqSensitiveInformation>();
                cfg.CreateMap<UserDto, UserViewModel>();
                cfg.CreateMap<UserDto, UserRabbitMq>();
                cfg.CreateMap<UserDto, UserRabbitMqSensitiveInformation>();
                cfg.CreateMap<FavoriteArtist, FavoriteArtistDto>();
                cfg.CreateMap<UserHobby, UserHobbyDto>();
                cfg.CreateMap<UserActivationRabbitMq, ActivationDto>();
                cfg.CreateMap<DisabledUserRabbitMq, DisabledUserDto>();
                cfg.CreateMap<DisabledUser, DisabledUserDto>();
                cfg.CreateMap<UserDto, UserViewModel>();
                cfg.CreateMap<UserHobbyDto, UserHobbyViewModel>();
                cfg.CreateMap<FavoriteArtistDto, FavoriteArtistViewModel>();
            });
    }
}