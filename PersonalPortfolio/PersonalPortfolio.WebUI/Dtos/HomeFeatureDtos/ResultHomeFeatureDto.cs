namespace PersonalPortfolio.WebUI.Dtos.HomeFeatureDtos
{
    public class ResultHomeFeatureDto
    {
        public int HomeFeatureId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Description { get; set; }
        public string? BgImage { get; set; }
        public string? BgImageContentType { get; set; }
    }
}
