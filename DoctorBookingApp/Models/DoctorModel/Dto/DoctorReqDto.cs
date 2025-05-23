using DoctorBookingApp.Models.TimeSlotModel;
using DoctorBookingApp.Models.UserModel;

namespace DoctorBookingApp.Models.DoctorModel.Dto
{
    public class DoctorReqDto
    {
        //Personal infor
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        //Professional infor
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public string? RegisterationNumber { get; set; }
        public int YearOfExperience { get; set; }
        public string? HospitalName { get; set; }
        public string? About { get; set; }
        public string? LanguagesSpoken { get; set; }


        //upload files
        public IFormFile? AvatarURL { get; set; } //image
        public IFormFile? CertificationURL { get; set; } //pdf/image

    }
}
