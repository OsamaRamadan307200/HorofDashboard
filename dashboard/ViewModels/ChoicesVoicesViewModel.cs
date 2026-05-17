using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels.Slides
{
    public class ChoicesVoicesViewModel : SlideBaseViewModel
    {
        public ChoicesVoicesViewModel() => MasterSlideTypeId = 8;

        [MinLength(2, ErrorMessage = "At least 2 choices are required.")]
        public List<string> Choices { get; set; } = new();

        [Range(1, int.MaxValue, ErrorMessage = "Please select the correct answer.")]
        public int CorrectChoiceIndex { get; set; }
    }
}
