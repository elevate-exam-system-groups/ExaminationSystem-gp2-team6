namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.ViewModel
{
    public class UpdateQuizResponseViewModel()
    {
        public int Id;
        public string Title;
        public int PassScore;
        public int? MaxAttempts;
        public string Status;
        public string? Instructions;
        public int DurationMinutes;
    }
}