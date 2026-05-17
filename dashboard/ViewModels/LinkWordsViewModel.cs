using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels.Slides
{
    public class LinkWordsViewModel : SlideBaseViewModel
    {
        public LinkWordsViewModel() => MasterSlideTypeId = 2;

        [MinLength(1, ErrorMessage = "At least one item is required in the first column.")]
        public List<string> FirstColumn { get; set; } = new();

        [MinLength(1, ErrorMessage = "At least one item is required in the second column.")]
        public List<string> SecondColumn { get; set; } = new();

        // Key = 1-based first-column index, Value = 1-based second-column index
        public Dictionary<int, int> LinkWordsAnswer { get; set; } = new();
    }
}
