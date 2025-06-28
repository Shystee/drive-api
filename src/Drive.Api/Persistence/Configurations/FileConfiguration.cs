using Drive.Api.Domain;
using Drive.Api.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Drive.Api.Domain.File;

namespace Drive.Api.Persistence.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
	public void Configure(EntityTypeBuilder<File> builder)
	{
		builder.ToTable("files");

		builder.HasKey(f => f.Id);
        
		builder.Property(a => a.Id)
			.HasColumnName("id")
			.HasConversion<UlidToStringConverter>()
			.HasColumnType("char(26)")
			.IsRequired();

		builder.Property(f => f.Name)
			.HasColumnName("name")
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(f => f.S3Key)
			.HasColumnName("s3_key")
			.HasMaxLength(500)
			.IsRequired();

		builder.Property(f => f.ContentType)
			.HasColumnName("content_type")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(f => f.Size)
			.HasColumnName("size")
			.IsRequired();

		builder.HasIndex(f => f.S3Key)
			.HasDatabaseName("ix_files_s3_key")
			.IsUnique();
		
		builder.Property<Ulid>("AlbumId")
			.HasColumnName("album_id")
			.HasConversion<UlidToStringConverter>()
			.HasColumnType("char(26)")
			.IsRequired();

		builder.HasIndex("AlbumId")
			.HasDatabaseName("ix_files_album_id");

		builder.HasOne<Album>()
			.WithMany(a => a.Files)
			.HasForeignKey("AlbumId")
			.HasConstraintName("fk_files_album_id")
			.OnDelete(DeleteBehavior.Cascade);
	}
}