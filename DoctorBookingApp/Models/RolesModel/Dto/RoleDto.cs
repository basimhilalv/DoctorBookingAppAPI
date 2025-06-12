using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Models.RolesModel.Dto
{
    public class RoleDto
    {
        [Required]
        public string? Name { get; set; }

    }
}
