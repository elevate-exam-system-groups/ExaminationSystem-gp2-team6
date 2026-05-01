using ExaminationSystem.Domain.Entities.AnswerOption;
using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.PasswordResetTemporaryToken;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Domain.Entities.User.UserOtp;
using ExaminationSystem.Domain.Entities.User.UserRefreshToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Apply base EF Core configurations.
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Diploma> Diplomas { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }

    public DbSet<Domain.Entities.QuizAttempt.QuizAttempt> QuizAttempts { get; set; }
    public DbSet<StudentDiplomaEnrollment> StudentDiplomaEnrollments { get; set; }
    public DbSet<LoginLog> LoginLogs { get; set; }

    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public DbSet<UserOtp> UserOtps { get; set; }
    public DbSet<PasswordResetTemporaryToken> PasswordResetTemporaryTokens { get; set; }
}
