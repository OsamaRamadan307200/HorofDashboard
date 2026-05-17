using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels
{
    public class LessonCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [StringLength(2000, ErrorMessage = "Details cannot exceed 2000 characters")]
        public string? Details { get; set; }

        public bool Status { get; set; } = true;

        [Range(0, int.MaxValue, ErrorMessage = "Minutes work must be non-negative")]
        public int? MinutesWork { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Position must be greater than 0")]
        public int? Position { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [StringLength(200, ErrorMessage = "URL cannot exceed 200 characters")]
        public string Url { get; set; } = string.Empty;
    }

    public class LessonEditViewModel : LessonCreateViewModel
    {
        public int Id { get; set; }
    }

    public class LessonDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Details { get; set; }
        public bool Status { get; set; }
        public int? MinutesWork { get; set; }
        public int Position { get; set; }
        public int UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? LevelName { get; set; }
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int SlidesCount { get; set; }
    }

    public class LessonCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(2000)]
        public string? Details { get; set; }

        public bool Status { get; set; } = true;

        public int? MinutesWork { get; set; }

        public int? Position { get; set; }

        [Required]
        public int UnitId { get; set; }

        [Required]
        [StringLength(200)]
        public string Url { get; set; } = string.Empty;
    }

    public class LessonUpdateDto : LessonCreateDto
    {
        public int Id { get; set; }
    }

    public class LessonPositionUpdateDto
    {
        public string? Direction { get; set; } // "up" or "down"
        public int? NewPosition { get; set; }
    }

    public class LessonStatsDto
    {
        public int TotalLessons { get; set; }
        public int ActiveLessons { get; set; }
        public int InactiveLessons { get; set; }
        public int TotalSlides { get; set; }
        public int TotalMinutes { get; set; }
        public double AverageMinutesPerLesson { get; set; }
        public double AverageSlidesPerLesson { get; set; }
    }
}