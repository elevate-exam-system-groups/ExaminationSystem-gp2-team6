using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Configuration.Question;

public class QuestionConfiguration : IEntityTypeConfiguration<Domain.Entities.Question.Question>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Question.Question> builder)
    {
        builder.ToTable("Questions");
        builder.HasIndex(q => new { q.QuizId, q.OrderIndex }).IsUnique(); // Composite Index
        
        builder.Property(q => q.QuestionText).HasMaxLength(1000).IsRequired();
        builder.Property(q => q.Explanation).HasMaxLength(1000);
        builder.Property(q => q.OrderIndex).HasDefaultValue(0).IsRequired();
        
        builder.HasMany(q => q.AnswerOptions)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasQueryFilter(q => !q.IsDeleted);
    }
}