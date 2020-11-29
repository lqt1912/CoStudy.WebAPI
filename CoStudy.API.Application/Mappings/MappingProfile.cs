using AutoMapper;

namespace Tek.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddUserRequest, User>().ReverseMap();
        }
    }
}
