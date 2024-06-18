namespace FirstStep.Models.DTOs
{
    public class LoggingsDto
    {
        public int activeTot { get; set; }
        public int inactiveTot { get; set;}
        public int activeCA { get; set; }
        public int inactiveCA { get; set; }
        public int activeHRM { get; set; }
        public int inactiveHRM { get; set; }
        public int activeHRA { get; set; }
        public int inactiveHRA { get; set; }
        public int activeSeeker { get; set; }
        public int inactiveSeeker { get; set; }
        public int activeCmpUsers { get; set; }
        public int inactiveCmpUsers { get; set; }
        public int eligibleUnregisteredCompaniesCount { get; set; }
    }
}
