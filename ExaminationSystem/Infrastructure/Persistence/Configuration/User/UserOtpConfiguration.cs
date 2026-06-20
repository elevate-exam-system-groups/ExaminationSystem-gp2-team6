using ExaminationSystem.Domain.Entities.User.UserOtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Configuration.User;

public class UserOtpConfiguration : IEntityTypeConfiguration<UserOtp>
{
    public void Configure(EntityTypeBuilder<UserOtp> builder)
    {
        builder.ToTable("UserOtps");
        builder.HasIndex(o => new { o.UserId, o.IsUsed, o.ExpiresAt, o.Code });
        builder.Property(o => o.Code).IsRequired().HasMaxLength(200);
        builder.Property(o => o.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}