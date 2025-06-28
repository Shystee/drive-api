using Wolverine;
using Wolverine.Persistence.Sagas;

namespace Drive.Api;

public record S3UploadCompleted([property: SagaIdentity] string S3Key);

public record UploadTimeout([property: SagaIdentity] string S3Key, TimeSpan ValidFor) : TimeoutMessage(ValidFor);

public class Upload : Saga
{
	public Upload(Ulid fileId, string s3Key, string name, string contentType, Ulid albumId)
	{
		FileId = fileId;
		S3Key = s3Key;
		Name = name;
		ContentType = contentType;
		AlbumId = albumId;
	}
	
	public Ulid AlbumId { get; }

	public string ContentType { get; }

	public Ulid FileId { get; }

	public string Name { get; }

	[SagaIdentity]
	public string S3Key { get; }

	public CreateFileCommand Handle(S3UploadCompleted upload, ILogger<Upload> logger, ApplicationDbContext context)
	{
		logger.LogInformation("Applying timeout to order {Key}", upload.S3Key);
		MarkCompleted();
		return new CreateFileCommand(S3Key, FileId, Name, ContentType, 10, AlbumId);
	}

	public void Handle(UploadTimeout timeout, ILogger<Upload> logger)
	{
		logger.LogInformation("Applying timeout to order {Key}", timeout.S3Key);
		MarkCompleted();
	}
}