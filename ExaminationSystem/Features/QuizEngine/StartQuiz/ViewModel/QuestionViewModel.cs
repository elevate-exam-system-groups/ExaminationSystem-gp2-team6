using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public List<OptionDto> Options { get; set; } = new();
    }
}