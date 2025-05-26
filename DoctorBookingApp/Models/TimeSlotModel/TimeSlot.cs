using DoctorBookingApp.Models.DoctorModel;

namespace DoctorBookingApp.Models.TimeSlotModel
{
    public class TimeSlot
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
