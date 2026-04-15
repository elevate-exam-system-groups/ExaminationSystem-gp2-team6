using MediatR;

namespace ExaminationSystem.Features.QuizEngine.SubmitQuiz.Commands
{
    public sealed record SubmitQuizCommand(int AttemptId, int StudentId)
    : IRequest<SubmitQuizResult>;

    public sealed record SubmitQuizResult(
        int AttemptId,
        double Score,
        bool Passed
    );
}
