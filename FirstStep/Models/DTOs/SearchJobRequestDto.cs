namespace FirstStep.Models.DTOs
{
    public class SearchJobRequestDto
    {
        public string title { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string employeement_type { get; set; }
        public string arrangement { get; set; }

        public SearchJobRequestDto(string title, string country, string city, string employeement_type, string arrangement)
        {
            this.title = title;
            this.country = country;
            this.city = city;
            this.employeement_type = employeement_type;
            this.arrangement = arrangement;
        }
    }
}
