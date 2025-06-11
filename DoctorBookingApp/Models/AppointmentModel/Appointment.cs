using DoctorBookingApp.Models.TimeSlotModel;

namespace DoctorBookingApp.Models.AppointmentModel
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid TimeSlotId { get; set; }
        public TimeSlot? TimeSlot { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
