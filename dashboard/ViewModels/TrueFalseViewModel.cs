using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels.Slides
{
    public class TrueFalseViewModel : SlideBaseViewModel
    {
        public TrueFalseViewModel() => MasterSlideTypeId = 3;

        [Required(ErrorMessage = "Please select True or False as the correct answer.")]
        public string? TrueFalseAnswer { get; set; }

        public bool CorrectAnswer =>
            string.Equals(TrueFalseAnswer, "true", StringComparison.OrdinalIgnoreCase);
    }
}
