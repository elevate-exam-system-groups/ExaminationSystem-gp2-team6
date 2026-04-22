
namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.ViewModel
{
    public class UpdateQuizRequestViewModel
    {
        public string? Title { get; set; } = string.Empty;

        public int DiplomaId { get; set; }

        public int? DurationMinutes { get; set; }
        public int? PassScore { get; set; } 

        public int? MaxAttempts { get; set; }

        public string? Instructions { get; set; }
    }
}