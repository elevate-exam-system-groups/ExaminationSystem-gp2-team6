using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Configuration.PasswordResetTemporaryToken;

public class PasswordResetTemporaryTokenConfiguration : IEntityTypeConfiguration<Domain.Entities.PasswordResetTemporaryToken.PasswordResetTemporaryToken>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.PasswordResetTemporaryToken.PasswordResetTemporaryToken> builder)
    {
        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Token);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}