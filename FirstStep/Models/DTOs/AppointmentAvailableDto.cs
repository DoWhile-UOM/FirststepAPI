namespace FirstStep.Models.DTOs
{
    public struct AppointmentAvailableDto
    {
        public IEnumerable<AppointmentAvailabelTimeDto> slot { get; set; }

        public int interview_duration { get; set; }
        public string title { get; set; }

        public string company_name { get; set; }
    }
}
