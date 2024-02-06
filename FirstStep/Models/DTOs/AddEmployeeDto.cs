using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models.DTOs
{
    public class AddEmployeeDto
    {
        [EmailAddress]
        public required string email { get; set; }

        public required string password { get; set; }

        public required string first_name { get; set; }
        
        public required string last_name { get; set; }
        
        public int company_id { get; set; }
    }
}
