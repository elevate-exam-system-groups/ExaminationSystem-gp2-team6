using MediatR;

namespace ExaminationSystem.Features.QuizEngine.SubmitQuiz.Commands
{
    public sealed record SubmitQuizCommand(int AttemptId, Guid StudentId)
    : IRequest<SubmitQuizResult>;

    public sealed record SubmitQuizResult(
        int AttemptId,
        double Score,
        bool Passed
    );
}
