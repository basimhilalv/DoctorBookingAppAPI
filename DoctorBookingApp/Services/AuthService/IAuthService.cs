using DoctorBookingApp.Models.UserModel.Dto;

namespace DoctorBookingApp.Services.AuthService
{
    public interface IAuthService
    {
        public Task<string> Register(UserRegDto request);
        public Task<UserLogResDto> Login(UserLoginDto request);
    }
}
