using ExaminationSystem.Domain.Entities.QuizAttempt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configuration.QuizAttempts;

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.ToTable("QuizAttempts");

        builder.Property(a => a.Score).IsRequired();
        builder.Property(a => a.SubmittedAt).IsRequired();

        builder.HasOne(a => a.Student)
            .WithMany(u => u.QuizAttempts)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Quiz)
            .WithMany(q => q.QuizAttempts)
            .HasForeignKey(a => a.QuizId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.StudentId);
        builder.HasIndex(a => a.QuizId);

        builder.HasQueryFilter(a => !a.IsDeleted && !a.Quiz.IsDeleted);
    }
}
