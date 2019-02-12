using AutoMapper;
using Eatster.Domain.Entities;
using Eatster.Persistence.Identity;

namespace Eatster.Application.Mapper
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<User, AppUser>().ConstructUsing(u => new AppUser { UserName = u.UserName, Email = u.Email }).ForMember(au => au.Id, opt => opt.Ignore());
            CreateMap<AppUser, User>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)).
                                       ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash)).
                                       ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}