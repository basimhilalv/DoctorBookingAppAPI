using DoctorBookingApp.Models.DoctorModel.Dto;
using DoctorBookingApp.Models.TimeSlotModel.Dto;

namespace DoctorBookingApp.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<string> CreateDoctorProfile(Guid userId, DoctorReqDto request);
        Task<string> UpdateDoctorProfile(Guid userId, DoctorReqDto request);
        Task<string> DeleteDoctorProfile(Guid userId);
        Task<DoctorResDto> GetDoctorProfile(Guid userId);
        Task<string> GenerateTimeSlot(Guid userId, SetScheduleDto request);
    }
}
