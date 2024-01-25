using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
    {
        public class Application
        {
            [Key]
            public int application_Id { get; set; }

            public required string email { get; set; }

            public required string phone_number { get; set; }

            public required string status { get; set; }

            public DateTime? review_date { get; set; }

            public string? comment { get; set; }

            public required DateTime submitted_date { get; set; }
        }
    }



