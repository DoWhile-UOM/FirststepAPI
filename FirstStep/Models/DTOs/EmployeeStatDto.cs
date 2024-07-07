namespace FirstStep.Models.DTOs
{
    public class EmployeeStatDto
    {
        public int hraCount { get; set; }
        public int hrmCount { get; set; }
        public List<HRAEvaluationDto> hraEvaluations { get; set; }
    }
}
