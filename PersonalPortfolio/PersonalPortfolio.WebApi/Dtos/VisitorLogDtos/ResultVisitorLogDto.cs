namespace PersonalPortfolio.WebApi.Dtos.VisitorLogDtos
{
    public class ResultVisitorLogDto
    {
        public int VisitorLogId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime VisitDate { get; set; }
        public string? ReferrerUrl { get; set; }
        public string? PageVisited { get; set; }
    }
}
