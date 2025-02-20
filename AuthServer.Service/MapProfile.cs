using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AutoMapper;

namespace AuthServer.Service;
public class MapProfile : Profile
{

    public MapProfile()
    {
        CreateMap<UserApp, UserAppDto>().ReverseMap();
    }
}
