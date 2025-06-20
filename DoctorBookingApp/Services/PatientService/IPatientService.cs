﻿using DoctorBookingApp.Models.AppointmentModel;
using DoctorBookingApp.Models.DoctorModel;
using DoctorBookingApp.Models.DoctorModel.Dto;
using DoctorBookingApp.Models.PatientModel.Dto;
using DoctorBookingApp.Models.TimeSlotModel;

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
        Task<IEnumerable<TimeSlot>> GetTimeSlots(Guid doctorId);
        Task<string> CreateStripePaymentIntent(Guid appointmentId);
        Task<string> MarkAppointmentAsPaid(string paymentIntentId, string transactionId);
        Task<string> MarkAppointmentAsCash(Guid appointmentId);
    }
}
