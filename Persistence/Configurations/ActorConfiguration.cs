using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasMany(a => a.MovieActors)
               .WithOne(ma => ma.Actor)
               .HasForeignKey(ma => ma.ActorId);
    }
}
