namespace HorofDashboard.ViewModels.Slides
{
    public class SlideTypeSelectionViewModel
    {
        public int LessonId { get; set; }
        public List<SlideTypeOption> SlideTypes { get; set; } = new();
    }

    public class SlideTypeOption
    {
        public int TypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "fa-question";
        public string Action { get; set; } = string.Empty;
    }
}
