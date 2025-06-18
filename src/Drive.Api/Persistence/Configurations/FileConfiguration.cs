using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Drive.Api.Domain.File;

namespace Drive.Api.Persistence.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
	public void Configure(EntityTypeBuilder<File> builder)
	{
		builder.ToTable("files");

		// Primary key
		builder.HasKey(f => f.Id);
        
		// Id configuration
		builder.Property(f => f.Id)
			.HasColumnName("id")
			.HasMaxLength(8)
			.IsRequired();

		builder.Property(f => f.OriginalFileName)
			.HasColumnName("original_file_name")
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

		builder.Property(f => f.SizeInBytes)
			.HasColumnName("size_in_bytes")
			.IsRequired();

		builder.HasIndex(f => f.S3Key)
			.HasDatabaseName("ix_files_s3_key")
			.IsUnique(); // S3 keys should be unique
	}
}