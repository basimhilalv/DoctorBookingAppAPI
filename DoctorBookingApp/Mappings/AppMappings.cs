using AutoMapper;
using DoctorBookingApp.Models.UserModel;
using DoctorBookingApp.Models.UserModel.Dto;

namespace DoctorBookingApp.Mappings
{
    public class AppMappings : Profile
    {
        public AppMappings()
        {
            CreateMap<User, UserRegDto>().ReverseMap();
            CreateMap<User, UserRegResDto>().ReverseMap();
        }
    }
}
