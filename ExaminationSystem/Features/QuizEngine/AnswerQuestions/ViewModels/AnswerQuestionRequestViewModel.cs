namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.ViewModels
{
    public class AnswerQuestionRequestViewModel
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public Guid StudentId { get; set; }
    }
}