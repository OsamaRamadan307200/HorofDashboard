using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels
{
    public class SlideCreateViewModel
    {
        //[Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Position must be greater than 0")]
        public int? Position { get; set; }

        [Required(ErrorMessage = "Lesson is required")]
        public int LessonId { get; set; }

        [Required(ErrorMessage = "Slide type is required")]
        public int MasterSlideTypeId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Points must be non-negative")]
        public int? Points { get; set; }

        public string? SlideContent { get; set; }
        public string? Answer { get; set; }

        // Type-specific properties
        public string[]? Choices { get; set; }
        public int? CorrectChoiceIndex { get; set; }

        public string[]? FirstColumn { get; set; }
        public string[]? SecondColumn { get; set; }
        public Dictionary<string, int>? LinkWordsAnswer { get; set; }

        public string? TrueFalseAnswer { get; set; }

        public string? VoiceLink { get; set; }
        public string[]? VoiceChoices { get; set; }
        public int? CorrectVoiceChoiceIndex { get; set; }

        public string? WriteWordAnswer { get; set; }
    }

    public class SlideEditViewModel : SlideCreateViewModel
    {
        public int Id { get; set; }
        public Guid? Url_Id { get; set; }
    }

    public class SlideDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Guid? Url_Id { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int? Position { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? SlideContent { get; set; }
        public string? Answer { get; set; }
        public int? Points { get; set; }
        public int? MasterSlideTypeId { get; set; }
        public string? MasterSlideTypeName { get; set; }
        public int LessonId { get; set; }
        public string? LessonName { get; set; }
        public string? UnitName { get; set; }
        public string? LevelName { get; set; }
    }

    public class SlideCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        public int? Position { get; set; }

        [Required]
        public int LessonId { get; set; }

        [Required]
        public int MasterSlideTypeId { get; set; }

        public int? Points { get; set; }

        public string? SlideContent { get; set; }

        public string? Answer { get; set; }
    }

    public class SlideUpdateDto : SlideCreateDto
    {
        public int Id { get; set; }
        public Guid? Url_Id { get; set; }
    }

    public class SlideStatsDto
    {
        public int TotalSlides { get; set; }
        public int ActiveSlides { get; set; }
        public int InactiveSlides { get; set; }
        public int TotalPoints { get; set; }
        public double AveragePointsPerSlide { get; set; }
        public Dictionary<string, int> SlideTypeDistribution { get; set; } = new();
    }

    public class SlidePositionUpdateDto
    {
        public string? Direction { get; set; } // "up" or "down"
        public int? NewPosition { get; set; }
    }
}