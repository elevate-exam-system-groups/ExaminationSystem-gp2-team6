using ExaminationSystem.Domain.Entities.AnswerOption;
using ExaminationSystem.Domain.Entities.AttemptAnswer;
using ExaminationSystem.Domain.Entities.AttemptResult;
using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Domain.Entities.QuizAttempt;
using ExaminationSystem.Domain.Entities.User;

namespace ExaminationSystem.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Quiz, int> Quizzes { get; }
        IGenericRepository<Question, int> Questions { get; }
        IGenericRepository<AnswerOption, int> AnswerOptions { get; }
        IGenericRepository<QuizAttempt, int> QuizAttempts { get; }
        IGenericRepository<Diploma, int> Diplomas { get; }
        IGenericRepository<StudentDiplomaEnrollment, int> StudentDiplomaEnrollments { get; }
        IGenericRepository<AttemptAnswer, int> AttemptAnswers { get; }
        IGenericRepository<LoginLog, int> LoginLogs { get; }
        IGenericRepository<AttemptResult, int> AttemptResults { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
