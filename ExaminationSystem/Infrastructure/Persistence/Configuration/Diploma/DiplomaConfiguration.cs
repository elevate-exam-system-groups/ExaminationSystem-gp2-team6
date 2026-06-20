using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configuration.Diploma;

public class DiplomaConfiguration : IEntityTypeConfiguration<Domain.Entities.Diploma.Diploma>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Diploma.Diploma> builder)
    {
        builder.ToTable("Diplomas");
        builder.Property(d => d.Title).HasMaxLength(200).IsRequired();
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.Status).HasConversion<string>().IsRequired();
        
        builder.HasMany(d => d.Quizzes)
            .WithOne(q => q.Diploma)
            .HasForeignKey(q => q.DiplomaId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // builder.HasQueryFilter(d => !d.IsDeleted);
    }
}