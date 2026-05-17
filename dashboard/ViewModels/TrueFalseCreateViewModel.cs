// Simple ViewModels for True/False slides
using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels
{
    public class TrueFalseCreateViewModel
    {
        [Required(ErrorMessage = "Question is required")]
        [StringLength(500, ErrorMessage = "Question cannot exceed 500 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a lesson")]
        public int? LessonId { get; set; }

        [Range(0, 100)]
        public int? Points { get; set; } = 5;

        [Required(ErrorMessage = "Please select the correct answer")]
        public bool CorrectAnswer { get; set; }

        public bool Status { get; set; } = true;
    }

    public class TrueFalseEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [StringLength(500, ErrorMessage = "Question cannot exceed 500 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a lesson")]
        public int? LessonId { get; set; }

        [Range(0, 100)]
        public int? Points { get; set; }

        [Required(ErrorMessage = "Please select the correct answer")]
        public bool CorrectAnswer { get; set; }

        public bool Status { get; set; }
    }
}