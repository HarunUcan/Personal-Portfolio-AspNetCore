namespace PersonalPortfolio.WebApi.Dtos.HomeFeatureDtos
{
    public class UpdateHomeFeatureDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Description { get; set; }
        public string? BgImageUrl { get; set; }
    }
}
