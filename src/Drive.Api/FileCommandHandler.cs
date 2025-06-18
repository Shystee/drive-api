using Wolverine.Attributes;
using File = Drive.Api.Domain.File;

namespace Drive.Api;

public record CreateFileCommand(
	string S3Key,
	string FileId,
	string OriginalFileName,
	string ContentType,
	long SizeInBytes);

public class FileCommandHandler
{
	[Transactional]
	public static async Task Handle(CreateFileCommand command, ApplicationDbContext context, ILogger<FileCommandHandler> logger)
	{
		logger.LogInformation("Creating file record for {S3Key}", command.S3Key);
		var file = new File(
			command.FileId,
			command.OriginalFileName,
			command.S3Key,
			command.ContentType,
			command.SizeInBytes
		);

		await context.Files.AddAsync(file);
	}
}