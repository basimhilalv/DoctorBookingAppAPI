using DoctorBookingApp.Models.AppointmentModel;
using DoctorBookingApp.Models.DoctorModel;
using DoctorBookingApp.Models.PatientModel;
using DoctorBookingApp.Models.TimeSlotModel;
using DoctorBookingApp.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace DoctorBookingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

    }
}
