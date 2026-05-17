using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels
{
    public class LevelCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Position must be greater than 0")]
        public int? Position { get; set; }

        public string? Pic_Icon { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [StringLength(200, ErrorMessage = "URL cannot exceed 200 characters")]
        public string Url { get; set; } = string.Empty;
    }

    public class LevelEditViewModel : LevelCreateViewModel
    {
        public int Id { get; set; }
        public string? CurrentIconPath { get; set; }
    }

    public class LevelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int? Position { get; set; }
        public string? Pic_Icon { get; set; }
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int UnitCount { get; set; }
    }

    public class LevelCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        public int? Position { get; set; }

        public string? Pic_Icon { get; set; }

        [Required]
        [StringLength(200)]
        public string Url { get; set; } = string.Empty;
    }

    public class LevelUpdateDto : LevelCreateDto
    {
        public int Id { get; set; }
    }

    public class PositionUpdateDto
    {
        public string? Direction { get; set; } // "up" or "down"
        public int? NewPosition { get; set; }
    }

    public class LevelStatsDto
    {
        public int TotalLevels { get; set; }
        public int ActiveLevels { get; set; }
        public int InactiveLevels { get; set; }
        public int TotalUnits { get; set; }
    }
}