using Amazon.S3;
using Amazon.S3.Model;
using Drive.Api.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Wolverine.Http;

namespace Drive.Api;

public sealed record AlbumUploadRequest(string FileName);

public sealed record AlbumUploadResponse(string UploadUrl, string FileUri);

public class AlbumUploadEndpoint
{
	private static readonly FileExtensionContentTypeProvider Provider = new();

	[WolverinePost("/api/albums/{id}/upload")]
	[Authorize]
	public static async Task<(AlbumUploadResponse, Upload, UploadTimeout)> AlbumUpload(
		AlbumUploadRequest request,
		Album album,
		IAmazonS3 s3Service)
	{
		var extension = Path.GetExtension(request.FileName);
		var contentType = Provider.Mappings[extension];
		var validFor = TimeSpan.FromMinutes(30);
		var initiatedDate = DateTime.UtcNow;
		var expirationDate = initiatedDate.Add(validFor);
		var fileId = Ulid.NewUlid();
		var s3Key = $"files/{fileId}{extension}";
		var fileUri = $"https://d2znyj7nafxno9.cloudfront.net/{s3Key}";
		var presignedUrl = await s3Service.GetPreSignedURLAsync(new GetPreSignedUrlRequest
		{
			BucketName = "drive-api-test-bucket",
			Key = s3Key,
			Verb = HttpVerb.PUT,
			Expires = expirationDate,
			ContentType = contentType,
		});

		return (
			new AlbumUploadResponse(presignedUrl, fileUri),
			new Upload(fileId, s3Key, request.FileName, contentType, album.Id),
			new UploadTimeout(s3Key, validFor)
		);
	}

	public static async Task<(Album? album, IResult result)> LoadAsync(string id, ApplicationDbContext context)
	{
		if (!Ulid.TryParse(id, out var albumId))
		{
			return (null, Results.NotFound());
		}

		var album = await context.Albums.FindAsync(albumId);
		return album is null
			? (album, Results.NotFound())
			: (album, WolverineContinue.Result());
	}
}