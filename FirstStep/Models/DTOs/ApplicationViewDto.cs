using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace FirstStep.Models.DTOs
{
    public class ApplicationViewDto
    {

        [Key]
        public int application_Id { get; set; }

        public DateTime submitted_date { get; set; }

        public required string email { get; set; }

        public required string first_name { get; set; }

        public required string last_name { get; set; }

        public int phone_number { get; set; }

        public string? bio { get; set; }

        public required string cVurl { get; set; }

        public string? profile_picture { get; set; }

        public string? linkedin { get; set; }

        public IEnumerable<RevisionDto>? revisionList { get; set; }




    }
}
