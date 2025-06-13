using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;initial catalog = PersonalPortfolioDb;User Id=sa; Password=sa1234SA; MultipleActiveResultSets=true; Trust Server Certificate=true;");
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<HomeFeature> HomeFeatures { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<ProjectTag> ProjectTags { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<VisitorLog> VisitorLogs { get; set; }
        public DbSet<Resume> Resumes { get; set; }
    }
}
