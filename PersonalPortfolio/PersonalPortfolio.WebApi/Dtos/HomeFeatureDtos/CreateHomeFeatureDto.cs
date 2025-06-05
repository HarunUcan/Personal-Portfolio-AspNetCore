namespace PersonalPortfolio.WebApi.Dtos.HomeFeatureDtos
{
    public class CreateHomeFeatureDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Description { get; set; }
        public IFormFile? BgImage { get; set; }
    }
}
