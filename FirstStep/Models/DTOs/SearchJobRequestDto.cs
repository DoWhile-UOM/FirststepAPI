﻿namespace FirstStep.Models.DTOs
{
    public class SearchJobRequestDto
    {
        public string? title { get; set; }
        public string? country { get; set; }
        public string? city { get; set; }
        public float? distance { get; set; }
        public List<string>? employeement_type { get; set; }
        public List<string>? arrangement { get; set; }
    }
}
