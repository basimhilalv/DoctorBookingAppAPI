using DoctorBookingApp.Models.AppointmentModel;
using DoctorBookingApp.Models.DoctorModel;
using DoctorBookingApp.Models.DoctorModel.Dto;
using DoctorBookingApp.Models.PatientModel.Dto;

namespace DoctorBookingApp.Services.PatientService
{
    public interface IPatientService
    {
        Task<string> CreatePatientProfile(Guid userId, PatientReqDto request);
        Task<string> UpdatePatientProfile(Guid userId, PatientReqDto request);
        Task<string> DeletePatientProfile(Guid userId);
        Task<PatientResDto> GetPatientProfile(Guid userId);
        Task<IEnumerable<Doctor>> GetAvailableDoctors();
        Task<string> MakeAppointment(Guid userId, Guid slotId);
        Task<string> CancelAppointment(Guid userId, Guid appointmentId);
        Task<IEnumerable<Appointment>> GetAppointments(Guid userId);
    }
}
