using DoctorBookingApp.Models.DoctorModel;
using DoctorBookingApp.Models.PatientModel;
using DoctorBookingApp.Models.UserModel;

namespace DoctorBookingApp.Services.AdminService
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<string> BlockOrUnblock(Guid userId);
        Task<IEnumerable<Doctor>> GetDoctorsList();
        Task<IEnumerable<Patient>> GetPatientList();
        Task<string> VerifyDoctor(Guid DoctorId);
    }
}
