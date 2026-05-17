using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels.Slides
{
    public abstract class SlideBaseViewModel
    {
        [Required(ErrorMessage = "Slide name is required.")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Position must be at least 1.")]
        public int? Position { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Points must be 0 or more.")]
        public int? Points { get; set; }

        [Required(ErrorMessage = "Please select a lesson.")]
        public int LessonId { get; set; }

        public int MasterSlideTypeId { get; protected set; }
    }
}
