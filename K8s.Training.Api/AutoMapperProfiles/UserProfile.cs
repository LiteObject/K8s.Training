using AutoMapper;

namespace K8s.Training.Api.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            _ = CreateMap<Domain.Entities.User, DTO.User>().ReverseMap();
        }
    }
}
