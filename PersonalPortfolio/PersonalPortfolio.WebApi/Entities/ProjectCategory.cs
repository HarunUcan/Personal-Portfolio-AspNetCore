﻿namespace PersonalPortfolio.WebApi.Entities
{
    public class ProjectCategory
    {
        public int ProjectCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Project>? Projects { get; set; }
    }
}
