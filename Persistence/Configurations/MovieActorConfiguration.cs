using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
{
    public void Configure(EntityTypeBuilder<MovieActor> builder)
    {
        builder.HasKey(ma => new { ma.MovieId, ma.ActorId });

        builder.HasOne(ma => ma.Movie)
               .WithMany(m => m.MovieActors)
               .HasForeignKey(ma => ma.MovieId);

        builder.HasOne(ma => ma.Actor)
               .WithMany(a => a.MovieActors)
               .HasForeignKey(ma => ma.ActorId);
    }
}
