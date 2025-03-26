using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
       public void Configure(EntityTypeBuilder<Movie> builder)
       {
              builder.HasKey(m => m.Id);

              builder.Property(m => m.Title)
                     .IsRequired()
                     .HasMaxLength(200);

              builder.Property(m => m.Description)
                     .IsRequired()
                     .HasMaxLength(1000);

              builder.Property(m => m.ReleaseYear)
                     .IsRequired();

              builder.Property(m => m.Rating)
                     .IsRequired();

              builder.HasMany(m => m.MovieGenres)
                     .WithOne(mg => mg.Movie)
                     .HasForeignKey(mg => mg.MovieId);
       }
}
