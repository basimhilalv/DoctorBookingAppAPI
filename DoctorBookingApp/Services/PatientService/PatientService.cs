using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DoctorBookingApp.Data;
using DoctorBookingApp.Models.PatientModel;
using DoctorBookingApp.Models.PatientModel.Dto;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DoctorBookingApp.Services.PatientService
{
    public class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly Cloudinary _cloudinary;

        public PatientService(IMapper mapper, AppDbContext context, Cloudinary cloudinary)
        {
            _mapper = mapper;
            _context = context;
            _cloudinary = cloudinary;
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

        public Task<string> DeletePatientProfile(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPatientProfile(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdatePatientProfile(Guid userId, PatientReqDto request)
        {
            throw new NotImplementedException();
        }
    }
}
