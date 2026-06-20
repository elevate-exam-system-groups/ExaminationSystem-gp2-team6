using ExaminationSystem.Domain.Entities.Diploma;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configuration.Diploma;

public class StudentDiplomaEnrollmentConfiguration : IEntityTypeConfiguration<StudentDiplomaEnrollment>
{
    public void Configure(EntityTypeBuilder<StudentDiplomaEnrollment> builder)
    {
        builder.ToTable("StudentDiplomaEnrollments");

        builder.HasKey(e => new { e.StudentId, e.DiplomaId });

        builder.Property(e => e.EnrolledAt).IsRequired();

        builder.HasOne(e => e.Student)
            .WithMany(u => u.StudentDiplomaEnrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Diploma)
            .WithMany(d => d.StudentEnrollments)
            .HasForeignKey(e => e.DiplomaId)
            .OnDelete(DeleteBehavior.Cascade);

        // builder.HasQueryFilter(e => !e.Diploma.IsDeleted);
    }
}
