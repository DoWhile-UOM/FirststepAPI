﻿namespace FirstStep.Models.DTOs
{
    public class AddApplicationDto
    {
        public required int advertisement_id { get; set; }

        public required int seeker_id { get; set; }
       
        public  IFormFile? cv { get; set; }

        public required bool UseDefaultCv { get; set; }
    }
}
