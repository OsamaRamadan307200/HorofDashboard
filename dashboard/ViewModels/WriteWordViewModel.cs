using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels.Slides
{
    public class WriteWordViewModel : SlideBaseViewModel
    {
        public WriteWordViewModel() => MasterSlideTypeId = 5;

        [Required(ErrorMessage = "The correct answer is required.")]
        [MaxLength(500)]
        public string WriteWordAnswer { get; set; } = string.Empty;
    }
}
