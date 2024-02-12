namespace FirstStep.Models.DTOs
{
    public class FillDefaultCompanyDetails
    {
        public required string company_name { get; set; }
        public required string company_email { get; set; }
        public string? company_website { get; set; }
        public required int company_phone_number { get; set; }
        public string? company_logo { get; set; }
        public string? company_description { get; set; }
        public string? company_city { get; set; }
        public string? company_province { get; set; }
        public string? company_business_scale { get; set; }

    }
}
