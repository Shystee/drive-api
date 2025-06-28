using Wolverine.Attributes;
using File = Drive.Api.Domain.File;

namespace Drive.Api;

public record CreateFileCommand(
	string S3Key,
	Ulid FileId,
	string OriginalFileName,
	string ContentType,
	long Size,
	Ulid AlbumId);

public class FileCommandHandler
{
	[Transactional]
	public static async Task Handle(CreateFileCommand command, ApplicationDbContext context, ILogger<FileCommandHandler> logger)
	{
		var album = await context.Albums.FindAsync(command.AlbumId);
		if (album is null)
		{
			logger.LogError("Album {AlbumId} not found for file {S3Key}", command.AlbumId, command.S3Key);
			throw new OperationCanceledException("Album not found for file " + command.AlbumId);
		}
		
		
		logger.LogInformation("Adding a file record {S3Key} to album {AlbumId}", command.S3Key, command.AlbumId);
		album.AddFile(new File(
			command.FileId,
			command.OriginalFileName,
			command.S3Key,
			command.ContentType,
			command.Size
		));
	}
}