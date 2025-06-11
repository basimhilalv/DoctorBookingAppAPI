namespace DoctorBookingApp.Models.PaymentModel.Dto
{
    public class CreatePaymentDto
    {
        public Guid AppointmentId { get; set; }
        public string? PaymentMethod { get; set; }  //cash or online

    }
}
