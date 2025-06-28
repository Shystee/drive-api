using Drive.Api.Domain;
using Drive.Api.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Drive.Api.Persistence.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
	public void Configure(EntityTypeBuilder<Album> builder)
	{
		builder.ToTable("albums");
		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.HasColumnName("id")
			.HasConversion<UlidToStringConverter>()
			.HasColumnType("char(26)")
			.IsRequired();

		builder.Property(a => a.Title)
			.HasColumnName("title")
			.HasColumnType("text")
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(a => a.CreatedOn)
			.HasColumnName("created_on")
			.HasColumnType("timestamp with time zone")
			.IsRequired();
		
		builder.Property(a => a.CreatedBy)
			.HasColumnName("created_by")
			.HasColumnType("text")
			.HasMaxLength(100)
			.IsRequired();
		
		builder.Navigation(a => a.Files)
			.UsePropertyAccessMode(PropertyAccessMode.Field);

		builder.HasMany(a => a.Files)
			.WithOne()
			.HasForeignKey("AlbumId");
	}
}