using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.AnswerOption;
using ExaminationSystem.Domain.Entities.AttemptAnswer;
using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Domain.Entities.QuizAttempt;
using ExaminationSystem.Infrastructure.Persistence.Context;
using Hotel.Persistence.Repositories;

namespace ExaminationSystem.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Lazy repositories
        private IGenericRepository<Quiz, int>? _quizzes;
        private IGenericRepository<Question, int>? _questions;
        private IGenericRepository<AnswerOption, int>? _answerOptions;
        private IGenericRepository<QuizAttempt, int>? _quizAttempts;
        private IGenericRepository<Diploma, int>? _diplomas;
        private IGenericRepository<StudentDiplomaEnrollment, int>? _enrollments;
        private IGenericRepository<AttemptAnswer, int>? _attemptAnswers;

        // Repositories (lazy init)
        public IGenericRepository<Quiz, int> Quizzes
            => _quizzes ??= new GenericRepository<Quiz, int>(_context);

        public IGenericRepository<Question, int> Questions
            => _questions ??= new GenericRepository<Question, int>(_context);

        public IGenericRepository<AnswerOption, int> AnswerOptions
            => _answerOptions ??= new GenericRepository<AnswerOption, int>(_context);

        public IGenericRepository<QuizAttempt, int> QuizAttempts
            => _quizAttempts ??= new GenericRepository<QuizAttempt, int>(_context);

        public IGenericRepository<Diploma, int> Diplomas
            => _diplomas ??= new GenericRepository<Diploma, int>(_context);

        public IGenericRepository<StudentDiplomaEnrollment, int> StudentDiplomaEnrollments
            => _enrollments ??= new GenericRepository<StudentDiplomaEnrollment, int>(_context);

        public IGenericRepository<AttemptAnswer, int> AttemptAnswers
            => _attemptAnswers ??= new GenericRepository<AttemptAnswer, int>(_context);

        // Save changes
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // Dispose (optional but clean)
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
