﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstStep.Models
{
    public class Seeker : User
    {
        [DataType(DataType.PhoneNumber)]
        public int phone_number { get; set; }

        public required string bio { get; set; }

        public required string description { get; set; }

        public string? university { get; set; }

        public required string CVurl { get; set; }

        public string? profile_picture { get; set; }

        public string? linkedin { get; set; }

        public string? country { get; set; }

        public string? city { get; set; }

        public float longitude { get; set; }

        public float latitude { get; set; }


        [JsonIgnore]
        public virtual JobField? job_Field { get; set; }

        public required int field_id { get; set; }


        [JsonIgnore]
        public virtual ICollection<Advertisement>? savedAdvertisemnts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Application>? applications { get; set; }

        [JsonIgnore]
        public virtual ICollection<Skill>? skills { get; set; }

        [JsonIgnore]
        public virtual ICollection<Appointment>? appointments { get; set; }
    }
}
