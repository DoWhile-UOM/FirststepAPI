namespace FirstStep.Models.DTOs
{
    public struct HRManagerApplicationListDto
    {
        public int application_Id { get; set; }

        public string seekerName { get; set; }

        public string status { get; set; }

        public bool is_evaluated { get; set; }

        public DateTime submitted_date { get; set; }
    }
}
