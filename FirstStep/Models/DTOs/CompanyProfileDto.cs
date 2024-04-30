namespace FirstStep.Models.DTOs
{
    public struct CompanyProfileDto
    {
        public string company_name { get; set; }

        public string company_description { get; set; }

        public string company_business_scale { get; set; }

        public int company_phone_number { get; set; }

        public string company_email { get; set; }

        public string company_website { get; set; }

        public string company_logo { get; set; }

        public AdvertisementFirstPageDto companyAdvertisements { get; set; }
    }
}
