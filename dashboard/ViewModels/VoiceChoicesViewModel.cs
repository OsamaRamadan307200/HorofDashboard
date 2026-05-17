using System.ComponentModel.DataAnnotations;

namespace HorofDashboard.ViewModels.Slides
{
    public class VoiceChoicesViewModel : SlideBaseViewModel
    {
        public VoiceChoicesViewModel() => MasterSlideTypeId = 6;

        [Required(ErrorMessage = "Voice link/URL is required.")]
        public string VoiceLink { get; set; } = string.Empty;

        [MinLength(2, ErrorMessage = "At least 2 voice choices are required.")]
        public List<string> VoiceChoices { get; set; } = new();

        // Fixed: now 1-based (consistent with MultipleChoice and ChoicesVoices)
        [Range(1, int.MaxValue, ErrorMessage = "Please select the correct answer.")]
        public int CorrectVoiceChoiceIndex { get; set; }
    }
}
