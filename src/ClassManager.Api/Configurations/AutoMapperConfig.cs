using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Shared.ViewModels;

namespace ClassManager.Api.Configurations;
public class AutoMapperConfig : Profile
{
  public AutoMapperConfig()
  {
    CreateMap<User, UserViewModel>()
    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
  }
}