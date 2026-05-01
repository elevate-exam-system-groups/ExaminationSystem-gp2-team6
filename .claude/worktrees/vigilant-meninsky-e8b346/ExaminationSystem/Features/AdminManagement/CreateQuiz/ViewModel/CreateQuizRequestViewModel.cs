
namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.ViewModel
{
    public class CreateQuizRequestViewModel
    {
        public string Title { get; set; } = string.Empty;

        public int DiplomaId { get; set; }

        public int DurationMinutes { get; set; }

        public int PassScore { get; set; } = 60;

        public int? MaxAttempts { get; set; }

        public string? Instructions { get; set; }
    }
}