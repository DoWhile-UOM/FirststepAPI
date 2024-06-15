namespace FirstStep.Models.DTOs
{
    public struct AddAppointmentDto
    {
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public int company_id { get; set; }
        public int? advertisement_id { get; set; }
    }
}
