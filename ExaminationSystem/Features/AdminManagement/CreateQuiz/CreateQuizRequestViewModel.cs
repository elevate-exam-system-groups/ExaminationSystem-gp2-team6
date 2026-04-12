using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz
{
    public class CreateQuizRequestViewModel
    {
        public string Title;
        public TimeSpan Duration;
        public int PassScore;
        public int? MaxAttempts;
        public QuizStatus Status;
        public string? Instructions;         
    }
}