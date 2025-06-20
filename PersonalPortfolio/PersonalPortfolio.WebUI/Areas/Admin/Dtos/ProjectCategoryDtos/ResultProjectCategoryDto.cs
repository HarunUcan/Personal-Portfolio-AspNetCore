﻿namespace PersonalPortfolio.WebUI.Areas.Admin.Dtos.ProjectCategoryDtos
{
    public class ResultProjectCategoryDto
    {
        public int ProjectCategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
