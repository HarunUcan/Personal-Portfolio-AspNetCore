namespace PersonalPortfolio.WebApi.Dtos.VisitorLogDtos
{
    public class VisitorsLogDto
    {
        // Toplam ziyaretçi sayısı
        public int? TotalVisitors { get; set; }

        // Günlük ziyaretçi sayısı
        public int? DailyVisitors { get; set; }

        // Günlük toplam istek sayısı
        public int? DailyRequests { get; set; }

        // İnstagramdan gelen günlük ziyaretçi sayısı
        public int? InstagramDailyVisitors { get; set; }

        // Linkedin'den gelen günlük ziyaretçi sayısı
        public int? LinkedinDailyVisitors { get; set; }

        // Android cihazlarından gelen günlük ziyaretçi sayısı
        public int? AndroidDailyVisitors { get; set; }

        // Apple cihazlarından gelen günlük ziyaretçi sayısı
        public int? AppleDailyVisitors { get; set; }

        // Diğer cihazlardan gelen günlük ziyaretçi sayısı
        public int? OtherDailyVisitors { get; set; }

    }
}
