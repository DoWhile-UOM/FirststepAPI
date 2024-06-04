namespace FirstStep.Models.DTOs
{
    public class AddSeekerDto
    {
        public  string first_name { get; set; }

        public  string last_name { get; set; }

        public required string password { get; set; }

        public int phone_number { get; set; }

        public string email { get; set; }

        public string? university { get; set; }

        public string? linkedin { get; set; }

        public string bio { get; set; }

        public int field_id { get; set; }

        public required string cVurl { get; set; }

        public string? profile_picture { get; set; }

        public string description { get; set; }

        public virtual List<string>? seekerSkills { get; set; }

        //public string password_hash { get; set; }
    }
}
