using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels
{
    public class UnitCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Position must be greater than 0")]
        public int? Position { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public int LevelId { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [StringLength(200, ErrorMessage = "URL cannot exceed 200 characters")]
        public string Url { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Hours work must be non-negative")]
        public int? HoursWork { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Lessons count must be non-negative")]
        public int? LessonsCount { get; set; }
    }

    public class UnitEditViewModel : UnitCreateViewModel
    {
        public int Id { get; set; }
    }

    public class UnitDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int Position { get; set; }
        public int LevelId { get; set; }
        public string? LevelName { get; set; }
        public string Url { get; set; } = string.Empty;
        public int? HoursWork { get; set; }
        public int? LessonsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ActualLessonsCount { get; set; }
    }

    public class UnitCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        public int? Position { get; set; }

        [Required]
        public int LevelId { get; set; }

        [Required]
        [StringLength(200)]
        public string Url { get; set; } = string.Empty;

        public int? HoursWork { get; set; }

        public int? LessonsCount { get; set; }
    }

    public class UnitUpdateDto : UnitCreateDto
    {
        public int Id { get; set; }
    }

    public class UnitPositionUpdateDto
    {
        public string? Direction { get; set; } // "up" or "down"
        public int? NewPosition { get; set; }
    }

    public class UnitStatsDto
    {
        public int TotalUnits { get; set; }
        public int ActiveUnits { get; set; }
        public int InactiveUnits { get; set; }
        public int TotalLessons { get; set; }
        public int TotalHours { get; set; }
        public double AverageHoursPerUnit { get; set; }
    }
}
