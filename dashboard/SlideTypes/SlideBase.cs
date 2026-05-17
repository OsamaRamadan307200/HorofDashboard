
namespace HorofDashboard.SlideType
{
    public class SlideBase
    {
        public string? Name { get; set; }
        public bool ValidateAnswer(string userAnswer , string CorrectAnswer)
        {
            return CorrectAnswer.Equals(userAnswer);
        }

    }
}
