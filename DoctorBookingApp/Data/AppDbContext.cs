using DoctorBookingApp.Models.PatientModel;
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
    }
}
