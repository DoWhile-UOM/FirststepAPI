namespace FirstStep.Models.DTOs
{
    public class DailyInterviewCount
    {
        public DateTime Date { get; set; }
        public int Booked { get; set; }
        public int Completed { get; set; }
        public int Missed { get; set; }
    }
}
