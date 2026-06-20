using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configuration.AnswerOption;

public class AnswerOptionConfiguration : IEntityTypeConfiguration<Domain.Entities.AnswerOption.AnswerOption>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.AnswerOption.AnswerOption> builder)
    {
        builder.ToTable("AnswerOptions");
        builder.Property(a => a.Text).HasMaxLength(1000).IsRequired();
        builder.Property(a => a.IsCorrect).IsRequired().IsRequired();
        
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}