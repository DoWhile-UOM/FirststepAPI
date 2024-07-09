namespace FirstStep.Models.DTOs
{
    public class InterviewStatDto
    {
        public List<DailyInterviewCount>? InterviewCountPerDay { get; set; }
        public double IsCalledPercentage { get; set; }
    }
}
