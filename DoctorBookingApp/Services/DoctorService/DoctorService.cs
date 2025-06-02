﻿using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DoctorBookingApp.Data;
using DoctorBookingApp.Models.DoctorModel;
using DoctorBookingApp.Models.DoctorModel.Dto;
using DoctorBookingApp.Models.PatientModel;
using DoctorBookingApp.Models.TimeSlotModel;
using DoctorBookingApp.Models.TimeSlotModel.Dto;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DoctorBookingApp.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly Cloudinary _cloudinary;

        public DoctorService(IMapper mapper, AppDbContext context, Cloudinary cloudinary)
        {
            _mapper = mapper;
            _context = context;
            _cloudinary = cloudinary;
        }
        public async Task<string> GenerateTimeSlot(Guid userId, SetScheduleDto request)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor is null) throw new Exception("Doctor Profile not available");
                var today = DateTime.Today;
                for(int week = 0; week < request.WeeksToGenerate; week++)
                {
                    foreach(var day in request.DaysAvailable)
                    {
                        var date = today.AddDays((int)day - (int)today.DayOfWeek + 7 * week);
                        if (date < today) continue;
                        for(var time = request.StartTime; time<request.EndTime; time += TimeSpan.FromMinutes(request.SlotDurationInMinutes))
                        {
                            var startTime = time;
                            var endTime = time + TimeSpan.FromMinutes(request.SlotDurationInMinutes);

                            var exists = await _context.TimeSlots.AnyAsync(s => 
                                s.DoctorId == doctor.Id && 
                                s.SlotDate == date && 
                                s.StartTime == startTime &&
                                s.EndTime == endTime);
                            if (!exists)
                            {
                                var slot = new TimeSlot
                                {
                                    DoctorId = doctor.Id,
                                    SlotDate = date,
                                    StartTime = startTime,
                                    EndTime = endTime
                                };
                                _context.TimeSlots.Add(slot);
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                return "Time Slotes added successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> RemoveOneTimeSlot(Guid userId, Guid slotId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor is null) throw new Exception("Doctor profile not available");
                var timeSlot = await _context.TimeSlots.FirstOrDefaultAsync(t => t.Id == slotId && t.DoctorId == doctor.Id);
                if (timeSlot is null) throw new Exception("Time Slot not available");
                _context.TimeSlots.Remove(timeSlot);
                await _context.SaveChangesAsync();
                return "Time Slot deleted successfully";
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public async Task<string> CreateDoctorProfile(Guid userId, DoctorReqDto request)
        {
            try
            {
                if (request is null) throw new Exception("Please provide profile details");
                var profileexist = await _context.Doctors.FirstOrDefaultAsync(p => p.UserId == userId);
                if (profileexist != null) throw new Exception("Doctor profile is already exists");
                var profile = _mapper.Map<Doctor>(request);
                profile.UserId = userId;

                if (request.Avatar != null && request.Avatar.Length > 0)
                {
                    await using var stream = request.Avatar.OpenReadStream();

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(request.Avatar.FileName, stream),
                        Folder = "doctor_avatars"
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
                if(request.Certification != null && request.Certification.Length > 0)
                {
                    await using var stream = request.Certification.OpenReadStream();
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(request.Certification.FileName, stream),
                        Folder = "doctor_certi"
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    if (uploadResult.StatusCode == HttpStatusCode.OK) profile.CertificationURL = uploadResult.SecureUrl.ToString();
                    else throw new Exception("Document upload failed");
                }

                _context.Doctors.Add(profile);
                await _context.SaveChangesAsync();
                return "Doctor Profile is created successfully";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteDoctorProfile(Guid userId)
        {
            try
            {
                var profile = await _context.Doctors.FirstOrDefaultAsync(u => u.UserId == userId);
                if (profile is null) throw new Exception("Doctor profile not available");
                _context.Doctors.Remove(profile);
                await _context.SaveChangesAsync();
                return "Doctor profile deleted successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<DoctorResDto> GetDoctorProfile(Guid userId)
        {
            try
            {
                var profile = await _context.Doctors.Include(d=> d.User).FirstOrDefaultAsync(u => u.UserId == userId);
                if (profile is null) throw new Exception("Doctor profile not available");
                var profileRes = _mapper.Map<DoctorResDto>(profile);
                return profileRes;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateDoctorProfile(Guid userId, DoctorReqDto request)
        {
            try
            {
                var profile = await _context.Doctors.FirstOrDefaultAsync(u => u.UserId == userId);
                if (profile is null) throw new Exception("Doctor profile not available");
                if (request.Avatar != null && request.Avatar.Length > 0)
                {
                    await using var stream = request.Avatar.OpenReadStream();

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(request.Avatar.FileName, stream),
                        Folder = "doctor_avatars"
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
                if (request.Certification != null && request.Certification.Length > 0)
                {
                    await using var stream = request.Certification.OpenReadStream();
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(request.Certification.FileName, stream),
                        Folder = "doctor_certi"
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    if (uploadResult.StatusCode == HttpStatusCode.OK) profile.CertificationURL = uploadResult.SecureUrl.ToString();
                    else throw new Exception("Document upload failed");
                }
                profile.FullName = request.FullName;
                profile.Gender = request.Gender;
                profile.Age = request.Age;
                profile.Phone = request.Phone;
                profile.Address = request.Address;
                profile.Qualification = request.Qualification;
                profile.Specialization = request.Specialization;
                profile.RegisterationNumber = request.RegisterationNumber;
                profile.YearOfExperience = request.YearOfExperience;
                profile.HospitalName = request.HospitalName;
                profile.About = request.About;
                profile.LanguagesSpoken = request.LanguagesSpoken;
                profile.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return "Profile updated successfully";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
