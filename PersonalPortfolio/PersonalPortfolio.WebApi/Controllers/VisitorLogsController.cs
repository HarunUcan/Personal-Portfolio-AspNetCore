using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos.VisitorLogDtos;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorLogsController : ControllerBase
    {
        private readonly ApiContext _context;

        public VisitorLogsController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult LogVisitor([FromBody] CreateVisitorLogDto createVisitorLogDto)
        {
            if (createVisitorLogDto == null)
            {
                return BadRequest("Visitor log cannot be null.");
            }

            // Set the visit date to the current UTC time
            createVisitorLogDto.VisitDate = DateTime.UtcNow;

            VisitorLog visitorLog = new VisitorLog
            {
                IpAddress = createVisitorLogDto.IpAddress,
                UserAgent = createVisitorLogDto.UserAgent,
                VisitDate = createVisitorLogDto.VisitDate,
                ReferrerUrl = createVisitorLogDto.ReferrerUrl,
                PageVisited = createVisitorLogDto.PageVisited
            };

            // Add the visitor log to the context
            _context.VisitorLogs.Add(visitorLog);
            _context.SaveChanges();

            return Ok("Visitor log saved successfully.");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetVisitorLogs()
        {
            var visitorLogs = _context.VisitorLogs.ToList();
            if (visitorLogs == null || !visitorLogs.Any())
            {
                return NotFound("No visitor logs found.");
            }

            return Ok(visitorLogs);
        }

        [Authorize]
        [HttpGet("analytics")]
        public IActionResult GetVisitorAnalytics()
        {
            var today = DateTime.UtcNow.Date;

            var logsToday = _context.VisitorLogs.Where(v => v.VisitDate.Date == today);

            // Farklı IP adreslerinden gelen toplam ziyaretçi sayısı
            var totalVisitors = _context.VisitorLogs
                .Where(v => v.IpAddress != null)
                .Select(v => v.IpAddress)
                .Distinct()
                .Count();

            // Farklı IP adreslerinden gelen bugünkü ziyaretçi sayısı
            var dailyVisitors = logsToday
                .Where(v => v.IpAddress != null)
                .Select(v => v.IpAddress)
                .Distinct()
                .Count();

            // Günlük toplam istek sayısı (aynı IP ve sayfa birden çok defa sayılır)
            var dailyRequests = logsToday.Count();

            // Instagram'dan gelen günlük ziyaretçi sayısı (ReferrerUrl içinde "instagram")
            var instagramDailyVisitors = logsToday
                .Where(v => v.ReferrerUrl != null && v.ReferrerUrl.ToLower().Contains("instagram"))
                .Select(v => v.IpAddress)
                .Distinct()
                .Count();

            // Linkedin'den gelen günlük ziyaretçi sayısı (ReferrerUrl içinde "linkedin")
            var linkedinDailyVisitors = logsToday
                .Where(v => v.ReferrerUrl != null && v.ReferrerUrl.ToLower().Contains("linkedin"))
                .Select(v => v.IpAddress)
                .Distinct()
                .Count();

            // Android cihazlarından gelen günlük ziyaretçi sayısı (UserAgent içinde "android")
            var androidDailyVisitors = logsToday
                .Where(v => v.UserAgent != null && v.UserAgent.ToLower().Contains("android"))
                .Select(v => v.IpAddress)
                .Distinct()
                .Count();

            // Apple cihazlarından gelen günlük ziyaretçi sayısı (UserAgent içinde iphone, ipad, ipod)
            var appleDailyVisitors = logsToday
                .Where(v => v.UserAgent != null &&
                    (v.UserAgent.ToLower().Contains("iphone")
                    || v.UserAgent.ToLower().Contains("ipad")
                    || v.UserAgent.ToLower().Contains("ipod")))
                .Select(v => v.IpAddress)
                .Distinct()
                .Count();

            // Diğer cihazlardan gelen günlük ziyaretçi sayısı (Android veya Apple olmayan)
            var otherDailyVisitors = logsToday
                .Where(v => v.UserAgent != null
                    && !v.UserAgent.ToLower().Contains("android")
                    && !v.UserAgent.ToLower().Contains("iphone")
                    && !v.UserAgent.ToLower().Contains("ipad")
                    && !v.UserAgent.ToLower().Contains("ipod"))
                .Select(v => v.IpAddress)
                .Distinct()
                .Count();

            var result = new VisitorsLogDto
            {
                TotalVisitors = totalVisitors,
                DailyVisitors = dailyVisitors,
                DailyRequests = dailyRequests,
                InstagramDailyVisitors = instagramDailyVisitors,
                LinkedinDailyVisitors = linkedinDailyVisitors,
                AndroidDailyVisitors = androidDailyVisitors,
                AppleDailyVisitors = appleDailyVisitors,
                OtherDailyVisitors = otherDailyVisitors
            };

            return Ok(result);
        }
    }
}
