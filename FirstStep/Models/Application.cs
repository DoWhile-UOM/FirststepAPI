using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
    {
        public class Application
        {
            [Key]
            public required string applicationId { get; set; }

            public required string email { get; set; }

            public required string phoneNumber { get; set; }

            public required string status { get; set; }

            public DateTime? reviewDate { get; set; }

            public string? comment { get; set; }

            public required DateTime submittedDate { get; set; }
        }
    }



