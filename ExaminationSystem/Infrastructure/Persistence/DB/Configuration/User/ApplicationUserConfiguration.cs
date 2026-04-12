using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Domain.Entities.User.ApplicationUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Configuration.User;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.FullName).HasComputedColumnSql("[FirstName] + ' ' + [LastName]").IsRequired();
        builder.Property(u => u.ProfileImageUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(u => u.UserType).HasConversion<string>().IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();
    }
}