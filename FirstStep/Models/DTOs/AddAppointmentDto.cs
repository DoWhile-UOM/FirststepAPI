namespace FirstStep.Models.DTOs
{
    public struct AddAppointmentDto
    {
        public int company_id { get; set; }
        public int advertisement_id { get; set; }
        public int duration { get; set; }
        public List<DateTime> time_slots { get; set; }

        public string comment { get; set; }


    }
}
