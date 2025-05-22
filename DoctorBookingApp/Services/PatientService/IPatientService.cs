using DoctorBookingApp.Models.PatientModel.Dto;

namespace DoctorBookingApp.Services.PatientService
{
    public interface IPatientService
    {
        Task<string> CreatePatientProfile(Guid userId, PatientReqDto request);
        Task<string> UpdatePatientProfile(Guid userId, PatientReqDto request);
        Task<string> DeletePatientProfile(Guid userId);
        Task<string> GetPatientProfile(Guid userId);
    }
}
