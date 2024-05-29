using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace FirstStep.Models.DTOs
{
    public class ApplicationViewDto
    {

        [Key]
        public int application_Id { get; set; }

        public DateTime submitted_date { get; set; }
        



    }
}
