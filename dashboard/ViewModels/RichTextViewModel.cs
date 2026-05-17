using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels.Slides
{
    public class RichTextViewModel : SlideBaseViewModel
    {
        public RichTextViewModel() => MasterSlideTypeId = 7;

        [Required(ErrorMessage = "The expected answer content is required.")]
        public string WriteWordAnswer { get; set; } = string.Empty;
    }
}
