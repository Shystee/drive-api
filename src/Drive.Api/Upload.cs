using Wolverine;
using Wolverine.Persistence.Sagas;

namespace Drive.Api;

public record StartUpload(
	string FileId,
	[property: SagaIdentity] string S3Key,
	string OriginalFileName,
	string ContentType,
	DateTimeOffset InitiatedAt,
	TimeSpan ValidFor);

public record S3UploadCompleted([property: SagaIdentity] string S3Key);

public record UploadTimeout([property: SagaIdentity] string S3Key, TimeSpan ValidFor) : TimeoutMessage(ValidFor);

public class Upload : Saga
{
	public Upload(string fileId, string s3Key, string originalFileName, string contentType, DateTimeOffset initiatedAt)
	{
		FileId = fileId;
		S3Key = s3Key;
		OriginalFileName = originalFileName;
		ContentType = contentType;
		InitiatedAt = initiatedAt;
	}

	public string ContentType { get; }

	public string FileId { get; }

	public DateTimeOffset InitiatedAt { get; }

	public string OriginalFileName { get; }

	[SagaIdentity]
	public string S3Key { get; }

	public static (Upload, UploadTimeout) Start(StartUpload upload, ILogger<Upload> logger)
	{
		logger.LogInformation("Applying timeout to order {Key}", upload.S3Key);
		return (
			new Upload(upload.FileId, upload.S3Key, upload.OriginalFileName, upload.ContentType, upload.InitiatedAt),
			new UploadTimeout(upload.S3Key, upload.ValidFor)
		);
	}

	public CreateFileCommand Handle(S3UploadCompleted upload, ILogger<Upload> logger, ApplicationDbContext context)
	{
		logger.LogInformation("Applying timeout to order {Key}", upload.S3Key);
		MarkCompleted();
		return new CreateFileCommand(S3Key, FileId, OriginalFileName, ContentType, 10);
	}

	public void Handle(UploadTimeout timeout, ILogger<Upload> logger)
	{
		logger.LogInformation("Applying timeout to order {Key}", timeout.S3Key);
		MarkCompleted();
	}
}