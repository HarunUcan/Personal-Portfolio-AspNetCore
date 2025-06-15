namespace PersonalPortfolio.WebUI.Areas.Admin.Dtos.AboutDtos
{
    public class UpdateAboutDto
    {
        public string? Title { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description3 { get; set; }
        public DateTime Birthday { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? Degree { get; set; }
        public string? Email { get; set; }
        public string? Freelance { get; set; }
    }
}
