namespace PersonalPortfolio.WebUI.Areas.Admin.Dtos.HomeDtos
{
    public class ResultHomeDto
    {
        public int HomeFeatureId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Description { get; set; }
        public string? BgImage { get; set; }
        public string? BgImageContentType { get; set; }
    }
}
