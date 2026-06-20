namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Dtos
{
    public class OptionDto
    {
        public int? Id { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
