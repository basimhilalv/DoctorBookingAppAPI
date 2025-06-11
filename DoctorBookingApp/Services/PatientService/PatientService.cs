using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DoctorBookingApp.Data;
using DoctorBookingApp.Models.AppointmentModel;
using DoctorBookingApp.Models.DoctorModel;
using DoctorBookingApp.Models.DoctorModel.Dto;
using DoctorBookingApp.Models.PatientModel;
using DoctorBookingApp.Models.PatientModel.Dto;
using DoctorBookingApp.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DoctorBookingApp.Services.PatientService
{
    public class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly IHubContext<BookingHub> _hubContext;

        public PatientService(IMapper mapper, AppDbContext context, Cloudinary cloudinary, IHubContext<BookingHub> hubContext)
        {
            _mapper = mapper;
            _context = context;
            _cloudinary = cloudinary;
            _hubContext = hubContext;
        }

        public Task<string> CancelAppointment(Guid appointmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreatePatientProfile(Guid userId, PatientReqDto request)
        {
            try
            {
                if (request is null) throw new Exception("Please provide profile details");
                var profileexist = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (profileexist != null) throw new Exception("Patient profile is already exists");
                var profile = _mapper.Map<Patient>(request);
                profile.UserId = userId;

                if(request.Avatar != null && request.Avatar.Length > 0)
                {
                    await using var stream = request.Avatar.OpenReadStream();

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(request.Avatar.FileName, stream),
                        Folder = "patient_avatars"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if(uploadResult.StatusCode == HttpStatusCode.OK)
                    {
                        profile.AvatarURL = uploadResult.SecureUrl.ToString();
                    }
                    else
                    {
                        throw new Exception("Image Upload Failed");
                    }
                }

                _context.Patients.Add(profile);
                await _context.SaveChangesAsync();
                return "Profile is created successfully";
                
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeletePatientProfile(Guid userId)
        {
            try
            {
                var profile = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if(profile != null)
                {
                    _context.Patients.Remove(profile);
                    await _context.SaveChangesAsync();
                    return "Profile Deleted Successfully";
                }
                return "Profile data deosn't exists";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Doctor>> GetAvailableDoctors()
        {
            try
            {
                var doctors = await _context.Doctors.ToListAsync();
                if (doctors.Count == 0) throw new Exception("No Doctors available");
                return doctors;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PatientResDto> GetPatientProfile(Guid userId)
        {
            try
            {
                var profile = await _context.Patients.Include(p=>p.User).FirstOrDefaultAsync(p => p.UserId == userId);
                if (profile is null) throw new Exception("Profile data deosn't exists");
                var profileRes = _mapper.Map<PatientResDto>(profile);
                return profileRes;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> MakeAppointment(Guid userId, Guid slotId)
        {
            var slot = await _context.TimeSlots.FirstOrDefaultAsync(t => t.Id == slotId);
            if (slot == null || slot.IsBooked) throw new Exception("Time Slot is not available");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null) throw new Exception("Profile Data Deosn't Exist");

            //Begin Transaction to avoid race conditions.
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                slot.IsBooked = true;
                var appointment = new Appointment
                {
                    PatientId = patient.Id,
                    DoctorId = slot.DoctorId,
                    TimeSlotId = slot.Id,
                    AppointmentDate = slot.SlotDate,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Status = "Booked"
                };
                _context.Appointments.Add(appointment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                //broadcast booking updates using signalR
                await _hubContext.Clients.Group(slot.DoctorId.ToString()).SendAsync("SlotBooked", slot.Id);
                return "Appointment Booked Successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdatePatientProfile(Guid userId, PatientReqDto request)
        {
            try
            {
                var profile = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (profile is null) throw new Exception("Profile Data Deosn't exist");
                if (request.Avatar != null && request.Avatar.Length > 0)
                {
                    await using var stream = request.Avatar.OpenReadStream();

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(request.Avatar.FileName, stream),
                        Folder = "patient_avatars"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == HttpStatusCode.OK)
                    {
                        profile.AvatarURL = uploadResult.SecureUrl.ToString();
                    }
                    else
                    {
                        throw new Exception("Image Upload Failed");
                    }
                }
                profile.FullName = request.FullName;
                profile.Age = request.Age;
                profile.Gender = request.Gender;
                profile.DateOfBirth = request.DateOfBirth;
                profile.Phone = request.Phone;
                profile.Address = request.Address;
                profile.BloodGroup = request.BloodGroup;
                profile.Height = request.Height;
                profile.Weight = request.Weight;
                profile.KnownAllergies = request.KnownAllergies;
                profile.ExistingConditions = request.ExistingConditions;
                profile.MedicalHistoryNotes = request.MedicalHistoryNotes;
                profile.Medications = request.Medications;
                profile.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return "Profile Updated Successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
