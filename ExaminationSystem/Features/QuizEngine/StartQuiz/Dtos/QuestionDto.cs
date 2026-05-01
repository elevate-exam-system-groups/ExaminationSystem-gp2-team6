namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public List<OptionDto> Options { get; set; } = new();
    }
}
