namespace ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel
{
    public class StartQuizResponseViewModel
    {
        public int QuizId { get; set; }
        public Guid StudentId { get; set; }
        public IEnumerable<QuestionViewModel> Questions { get; set; } = null!;
    }
}