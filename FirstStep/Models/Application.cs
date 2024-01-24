using System.ComponentModel.DataAnnotations;

namespace FirstStep.Models
    {
        public class Application
        {
            [Key]
            public required string applicationId { get; set; }

            public required string firstName { get; set; }

            public required string lastName { get; set; }

            public required string email { get; set; }

            public required string phoneNumber { get; set; }

            public required bool status { get; set; }

            public DateTime? lastReviewDate { get; set; }

            public string? lastComment { get; set; }

            public string? linkedinURL { get; set; }

            public required DateTime submittedDate { get; set; }
        }
    }



