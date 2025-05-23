using AutoMapper;
using DoctorBookingApp.Models.DoctorModel;
using DoctorBookingApp.Models.DoctorModel.Dto;
using DoctorBookingApp.Models.PatientModel;
using DoctorBookingApp.Models.PatientModel.Dto;
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
            CreateMap<Patient, PatientReqDto>().ReverseMap();
            CreateMap<Patient, PatientResDto>().ReverseMap();
            CreateMap<Doctor, DoctorReqDto>().ReverseMap();
            CreateMap<Doctor, DoctorResDto>().ReverseMap();
        }
    }
}
