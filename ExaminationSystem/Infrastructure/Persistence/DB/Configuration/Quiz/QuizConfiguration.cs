using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Configuration.Quiz;

public class QuizConfiguration : IEntityTypeConfiguration<Domain.Entities.Quiz.Quiz>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Quiz.Quiz> builder)
    {
        builder.ToTable("Quizzes");
        builder.Property(q => q.Title).HasMaxLength(200).IsRequired();
        builder.Property(q => q.Duration).IsRequired();
        builder.Property(q => q.PassScore).HasDefaultValue(60).IsRequired();
        builder.Property(q => q.MaxAttempts).IsRequired(false);
        builder.Property(q => q.Status).HasConversion<string>().IsRequired();
        builder.Property(q => q.Instructions).HasMaxLength(1000).IsRequired(false);
        
        builder.HasMany(q => q.Questions)
            .WithOne(q => q.Quiz)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasQueryFilter(q => !q.IsDeleted);
    }
}