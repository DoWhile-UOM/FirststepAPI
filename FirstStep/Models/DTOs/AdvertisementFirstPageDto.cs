namespace FirstStep.Models.DTOs
{
    public struct AdvertisementFirstPageDto
    {
        public IEnumerable<AdvertisementShortDto> FirstPageAdvertisements { get; set; }

        public IEnumerable<int> allAdvertisementIds { get; set; }
    }
}
