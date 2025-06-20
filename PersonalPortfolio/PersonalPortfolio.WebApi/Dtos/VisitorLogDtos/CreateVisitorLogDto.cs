﻿namespace PersonalPortfolio.WebApi.Dtos.VisitorLogDtos
{
    public class CreateVisitorLogDto
    {
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.UtcNow;
        public string? ReferrerUrl { get; set; }
        public string? PageVisited { get; set; }
    }
}
